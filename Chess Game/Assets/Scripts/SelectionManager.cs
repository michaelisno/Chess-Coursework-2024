using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private RaycastHit hit;
    private GameObject selectedTile = null;
    private List<GameObject> highlightedTiles = new List<GameObject>();

    private void Update()
    {
        // Click left mouse button
        if (Input.GetMouseButtonDown(0) && (GetComponent<GameManager>().isWhiteMoving == GetComponent<GameManager>().isPlayerWhite))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (selectedTile == null)
                { 
                    SelectNewPiece();
                    return;
                }

                // reset colour and currently selected tile
                Vector2 tilePosition = selectedTile.GetComponent<TileManager>().GetTilePosition();
                PieceManager oldPiece = selectedTile.transform.GetChild(0).GetComponent<PieceManager>();
                PieceManager newPiece = hit.collider.transform.GetChild(0).GetComponent<PieceManager>();

                if ((newPiece.GetPieceType() != PieceManager.PieceType.none && Convert.ToBoolean(newPiece.GetPieceColour()) 
                    == GetComponent<GameManager>().isPlayerWhite) || highlightedTiles.Contains(hit.collider.gameObject)) 
                {
                    selectedTile.GetComponent<TileManager>().SetTileColour(Convert.ToInt32(tilePosition.x + tilePosition.y) % 2);
                    selectedTile = null; 
                }

                // case one: currently selected tile == newly selected tile
                if (hit.collider.gameObject == oldPiece.transform.parent.gameObject)
                {
                    // remove all highlighted legal tiles
                    HighlightLegalMoves(new List<GameObject>());
                    return;
                }

                // case two: clicked tile not selected tile and clicked tile in legal moves, select new tile
                if (highlightedTiles.Contains(hit.collider.gameObject) 
                    && hit.collider.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.none)
                {
                    if (oldPiece.GetPieceType() == PieceManager.PieceType.king)
                    {
                        Debug.Log("is king");
                        if (hit.collider.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour() == 0)
                        {
                            GetComponent<GameManager>().blackKingPosition =
                                newPiece.GetComponentInParent<TileManager>().GetTilePosition();
                        }
                        else
                        {
                            GetComponent<GameManager>().whiteKingPosition =
                                newPiece.GetComponentInParent<TileManager>().GetTilePosition();
                        }
                    }

                    // Set new piece info to old piece info   
                    newPiece.SetHasPieceMoved(true);
                    newPiece.SetPieceColour(oldPiece.GetPieceColour());
                    newPiece.SetPieceType(oldPiece.GetPieceType());

                    // Set old piece info to no piece
                    oldPiece.SetPieceType(PieceManager.PieceType.none);
                    oldPiece.SetHasPieceMoved(false);

                    // remove all highlighted legal tiles
                    HighlightLegalMoves(new List<GameObject>());
                    GetComponent<GameManager>().PieceMoved();
                    return;
                }
                // case three: clicked tile not selected tile and clicked tile is friendly, deselect current tile, select new tile
                else if (Convert.ToBoolean(newPiece.GetPieceColour()) == GetComponent<GameManager>().isPlayerWhite)
                {
                    // remove all highlighted legal tiles
                    HighlightLegalMoves(new List<GameObject>());
                    SelectNewPiece();
                    return;
                }
                // case four: clicked tile was legal move hence take enemy piece and move
                if (highlightedTiles.Contains(hit.collider.gameObject)
                    && hit.collider.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() != PieceManager.PieceType.none
                    && Convert.ToBoolean(hit.collider.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour())
                    != GetComponent<GameManager>().isPlayerWhite)
                {
                    if (oldPiece.GetPieceType() == PieceManager.PieceType.king)
                    {
                        if (hit.collider.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour() == 0)
                        {
                            GetComponent<GameManager>().blackKingPosition =
                                newPiece.GetComponentInParent<TileManager>().GetTilePosition();
                        }
                        else
                        {
                            GetComponent<GameManager>().whiteKingPosition =
                                newPiece.GetComponentInParent<TileManager>().GetTilePosition();
                        }
                    }

                    GameObject pieceToTake = hit.transform.GetChild(0).gameObject.CloneViaFakeSerialization();
                    pieceToTake.name = pieceToTake.GetComponent<PieceManager>().GetPieceType().ToString();

                    // take piece
                    if (GetComponent<GameManager>().isPlayerWhite)
                        GetComponent<GameManager>().takenBlackPieces.Add(pieceToTake);
                    else
                        GetComponent<GameManager>().takenWhitePieces.Add(pieceToTake);

                    // Set new piece info to old piece info   
                    newPiece.SetHasPieceMoved(true);
                    newPiece.SetPieceColour(oldPiece.GetPieceColour());
                    newPiece.SetPieceType(oldPiece.GetPieceType());

                    // Set old piece info to no piece
                    oldPiece.SetPieceType(PieceManager.PieceType.none);
                    oldPiece.SetHasPieceMoved(false);

                    // remove all highlighted legal tiles
                    HighlightLegalMoves(new List<GameObject>());
                    GetComponent<GameManager>().PieceMoved();

                    return;
                }
            }
        }
    }

    private void SelectNewPiece()
    {
        // Selected tile is empty or enemy piece, return
        if (hit.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.none || 
            hit.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour() != Convert.ToInt32(GetComponent<GameManager>().
            isPlayerWhite)) 
            return;

        selectedTile = hit.collider.gameObject;

        // Set tile colour to selected colour
        selectedTile.GetComponent<TileManager>().SetTileColour(2);

        // Get list of legal moves
        List<GameObject> legalMoves = GetComponent<MoveGenerator>().GetMoves(selectedTile, false, GetComponent<GameManager>().isPlayerWhite);

        if (legalMoves.Count > 0) { HighlightLegalMoves(legalMoves); }
    }

    private void HighlightLegalMoves(List<GameObject> legalMoves)
    {
        foreach (GameObject tile in highlightedTiles)
        {
            Vector2 tilePosition = tile.GetComponent<TileManager>().GetTilePosition();
            tile.GetComponent<TileManager>().SetTileColour(Convert.ToInt32(tilePosition.x + tilePosition.y) % 2);
        }

        highlightedTiles.Clear();

        foreach (GameObject tile in legalMoves)
        {
            tile.GetComponent<TileManager>().SetTileColour(3);
            highlightedTiles.Add(tile);
        }
    }
}
