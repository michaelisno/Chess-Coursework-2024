using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private RaycastHit hit;
    private GameObject selectedTile = null;
    private List<GameObject> highlightedTiles = new List<GameObject>();

    private void Update()
    {
        // Click left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {                
                // no tile currently selected
                if (selectedTile == null)
                { 
                    SelectNewPiece();
                }
                else if (selectedTile != null)
                {
                    // case one: currently selected tile == newly selected tile
                    if (hit.collider.gameObject == selectedTile)
                    {
                        // reset colour
                        Vector2 tilePosition = selectedTile.GetComponent<TileManager>().GetTilePosition();
                        selectedTile.GetComponent<TileManager>().SetTileColour(Convert.ToInt32(tilePosition.x + tilePosition.y) % 2);

                        // remove all highlighted legal tiles
                        HighlightLegalMoves(new List<GameObject>());

                        // deselect tile
                        selectedTile = null;
                    }

                    // case two: clicked tile not selected tile and clicked tile in legal moves, select new tile
                    else if (hit.collider.gameObject != selectedTile && highlightedTiles.Contains(hit.collider.gameObject))
                    {
                        // Set new piece info to old piece info
                        PieceManager oldPiece = selectedTile.transform.GetChild(0).GetComponent<PieceManager>();
                        PieceManager newPiece = hit.collider.transform.GetChild(0).GetComponent<PieceManager>();

                        newPiece.SetHasPieceMoved(oldPiece.GetHasPieceMoved());
                        newPiece.SetPieceColour(oldPiece.GetPieceColour());
                        newPiece.SetPieceType(oldPiece.GetPieceType());

                        // Set old piece info to no piece
                        oldPiece.SetPieceType(PieceManager.PieceType.none);
                        oldPiece.SetHasPieceMoved(false);

                        // Reset colours
                        Vector2 oldTilePosition = selectedTile.GetComponent<TileManager>().GetTilePosition();
                        selectedTile.GetComponent<TileManager>().SetTileColour(Convert.ToInt32(oldTilePosition.x + oldTilePosition.y) % 2);
                        HighlightLegalMoves(new List<GameObject>());

                        selectedTile = null;
                    }
                    // case three: clicked tile not selected tile and clicked tile is friendly, deselect current tile, select new tile
                    else if (hit.collider.gameObject != selectedTile && 
                        Convert.ToBoolean(hit.collider.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour()) 
                        == GetComponent<GameManager>().isPlayerWhite)
                    {
                        // reset colour
                        Vector2 tilePosition = selectedTile.GetComponent<TileManager>().GetTilePosition();
                        selectedTile.GetComponent<TileManager>().SetTileColour(Convert.ToInt32(tilePosition.x + tilePosition.y) % 2);

                        // remove all highlighted legal tiles
                        HighlightLegalMoves(new List<GameObject>());

                        // deselect tile
                        selectedTile = null;

                        SelectNewPiece();
                    }
                }
            }
        }
    }

    private void SelectNewPiece()
    {
        // Selected tile is empty or enemy piece, return
        if (hit.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.none || 
            hit.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour() != Convert.ToInt32(GetComponent<GameManager>().isPlayerWhite)) 
            return;

        selectedTile = hit.collider.gameObject;

        // Set tile colour to selected colour
        selectedTile.GetComponent<TileManager>().SetTileColour(2);

        // Get list of legal moves
        List<GameObject> legalMoves = GetComponent<MoveGenerator>().GetMoves(selectedTile);

        if (legalMoves.Count > 0) { HighlightLegalMoves(legalMoves); }
        else { Debug.Log("NO LEGAL MOVES AVAILABLE."); }
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
