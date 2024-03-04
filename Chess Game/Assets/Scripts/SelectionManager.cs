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
                if (!GetComponent<GameManager>().isPlayerChecked)
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
                else
                {
                    Debug.Log("SelectionManager whilst checked, activated.");
                    if (hit.collider.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.king
                        && Convert.ToBoolean(hit.collider.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour()) ==
                        GetComponent<GameManager>().isPlayerWhite)
                    {
                        Debug.Log("Attempted to move king after checked.");
                        List<GameObject> kingLegalMoves = GetComponent<MoveGenerator>().GetMoves(hit.collider.gameObject, true, 
                            GetComponent<GameManager>().isPlayerWhite);

                        if (kingLegalMoves.Count > 0)
                            HighlightLegalMoves(kingLegalMoves);
                        else
                            Debug.Log("No Legal moves for king");
                    }
                    else
                    {
                        Debug.Log("Moving some piece other than king.");
                        // Generate enemy checker legal moves IN DIRECTION OF FRIENDLY KING, add Checker position to list
                        GameObject enemyChecker = GetComponent<GameManager>().checkerTile;
                        List<GameObject> enemyCheckerLegalMoves = new List<GameObject>();

                        Vector2 kingPos = new Vector2();
                        Vector2 enemyPos = enemyChecker.GetComponent<TileManager>().GetTilePosition();

                        bool isKingWhite = GetComponent<GameManager>().isPlayerWhite;

                        if (GetComponent<GameManager>().isPlayerWhite)
                            kingPos = GetComponent<GameManager>().whiteKingPosition;
                        else
                            kingPos = GetComponent<GameManager>().blackKingPosition;

                        GameObject checkedKingTile = GetComponent<GameManager>().tiles[(int)kingPos.x, (int)kingPos.y];

                        if (enemyChecker.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.bishop
                            || enemyChecker.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.queen)
                        {
                            int xDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().x
                                - (int)enemyChecker.GetComponent<TileManager>().GetTilePosition().x;
                            int yDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().y
                                - (int)enemyChecker.GetComponent<TileManager>().GetTilePosition().y;

                            if (xDiff < 0 && yDiff < 0)
                            {
                                // king is up, left from bishop
                                enemyCheckerLegalMoves = GetComponent<MoveGenerator>().GetMoves(enemyChecker, false, !isKingWhite, 0);
                            }
                            else if (xDiff < 0 && yDiff > 0)
                            {
                                // king is down, left from bishop
                                enemyCheckerLegalMoves = GetComponent<MoveGenerator>().GetMoves(enemyChecker, false, !isKingWhite, 1);
                            }
                            else if (xDiff > 0 && yDiff < 0)
                            {
                                // king is up, right from bishop
                                enemyCheckerLegalMoves = GetComponent<MoveGenerator>().GetMoves(enemyChecker, false, !isKingWhite, 2);
                            }
                            else
                            {
                                // king is down, right from bishop
                                enemyCheckerLegalMoves = GetComponent<MoveGenerator>().GetMoves(enemyChecker, false, !isKingWhite, 3);
                            }
                        }

                        enemyCheckerLegalMoves.Add(enemyChecker);

                        // For each friendly tile, generate legal moves and add to seperate list if coincide with enemy checker legal moves
                        List<GameObject> viableFriendlyTiles = new List<GameObject>();
                        List<List<GameObject>> viableFriendlyMoves = new List<List<GameObject>>();

                        foreach (GameObject tile in GetComponent<GameManager>().tiles)
                        {
                            if (tile.transform.GetChild(0).gameObject.activeInHierarchy
                                && Convert.ToBoolean(tile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour())
                                == GetComponent<GameManager>().isPlayerWhite)
                            {
                                List<GameObject> tempFriendlyLegalMoves =
                                    GetComponent<MoveGenerator>().GetMoves(tile, false, GetComponent<GameManager>().isPlayerWhite);

                                List<GameObject> finalLegalMoves = new List<GameObject>();

                                foreach (GameObject tempMove in tempFriendlyLegalMoves)
                                {
                                    if (enemyCheckerLegalMoves.Contains(tempMove))
                                    { 
                                        finalLegalMoves.Add(tempMove);
                                    }
                                }

                                if (finalLegalMoves.Count > 0)
                                {
                                    viableFriendlyTiles.Add(tile);
                                    viableFriendlyMoves.Add(finalLegalMoves);
                                }
                            }
                        }
                        // Check if selected tile is friendly tile with viable legal move, if not, ignore
                        if (viableFriendlyTiles.Contains(hit.collider.gameObject))
                        {
                            // HIGHLIGHT MOVE HERE
                            int index = viableFriendlyTiles.IndexOf(hit.collider.gameObject);
                            HighlightLegalMoves(viableFriendlyMoves[index]);
                            GetComponent<GameManager>().isPlayerChecked = false;
                            
                        }

                        // If no friendly viable legal moves generated, and no legal king moves avaiable, checkmate achieved - end game.
                        if (viableFriendlyTiles.Count == 0)
                        {
                            List<GameObject> kingLegalMoves = GetComponent<MoveGenerator>().GetMoves(hit.collider.gameObject, true,
                                GetComponent<GameManager>().isPlayerWhite);
                            if (kingLegalMoves.Count == 0)
                            {
                                Debug.Log("END GAME, CHECKMATE");
                                GetComponent<GameManager>().EndGame(Convert.ToInt32(GetComponent<GameManager>().isPlayerWhite));
                            }
                        }
                    }
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
