using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveGenerator : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject[,] tiles;

    private bool isPlayerWhite;

    public List<GameObject> GetMoves(GameObject selectedTile, bool ignoreKing = false, bool _isPlayerWhite = true)
    {
        // References to tile and piece
        TileManager tile = selectedTile.GetComponent<TileManager>();
        PieceManager piece = selectedTile.transform.GetChild(0).GetComponent<PieceManager>();
        gameManager = GetComponent<GameManager>();

        // Get information from tile and piece
        PieceManager.PieceType pieceType = piece.GetPieceType();
        Vector2 position = tile.GetTilePosition();
        bool hasPieceMoved = piece.GetHasPieceMoved();
        tiles = gameManager.tiles;

        isPlayerWhite = GetComponent<GameManager>().isPlayerWhite;

        if (ignoreKing)
            isPlayerWhite = !GetComponent<GameManager>().isPlayerWhite;

        if (_isPlayerWhite != GetComponent<GameManager>().isPlayerWhite)
        {
            isPlayerWhite = false;
        }

        return GenerateLegalMoves(pieceType, position, tiles, hasPieceMoved, piece, ignoreKing);
    }

    private List<GameObject> GenerateLegalMoves(PieceManager.PieceType pieceType, Vector2 position, GameObject[,] tiles, bool hasPieceMoved, PieceManager piece, bool ignoreKing)
    {
        List<GameObject> legalMoves = new List<GameObject>();

        // pawn
        if (pieceType == PieceManager.PieceType.pawn)
        {
            if (isPlayerWhite)
            {
                if (!ignoreKing)
                {
                    // up 1 square
                    int newY = (int)position.y + 1;
                    if (CheckMove(new Vector2(position.x, newY)))
                    {
                        legalMoves.Add(tiles[(int)position.x, newY]);
                    }

                    // up 2 squares
                    newY = (int)position.y + 2;
                    if (CheckMove(new Vector2(position.x, newY))
                        && !hasPieceMoved)
                    {
                        legalMoves.Add(tiles[(int)position.x, newY]);
                    }
                    // enemy taking
                    // up 1, left 1
                    if (CheckMove(new Vector2(position.x - 1, position.y + 1), true))
                    {
                        legalMoves.Add(tiles[(int)position.x - 1, (int)position.y + 1]);
                    }

                    // up 1, right 1
                    if (CheckMove(new Vector2(position.x + 1, position.y + 1), true))
                    {
                        legalMoves.Add(tiles[(int)position.x + 1, (int)position.y + 1]);
                    }
                }
                else 
                { 
                    // enemy taking
                    // up 1, left 1
                    if (CheckMove(new Vector2(position.x - 1, position.y + 1), true)
                        || CheckMove(new Vector2(position.x - 1, position.y + 1)))
                    {
                        legalMoves.Add(tiles[(int)position.x - 1, (int)position.y + 1]);
                    }

                    // up 1, right 1
                    if (CheckMove(new Vector2(position.x + 1, position.y + 1), true)
                        || CheckMove(new Vector2(position.x + 1, position.y + 1)))
                    {
                        legalMoves.Add(tiles[(int)position.x + 1, (int)position.y + 1]);
                    }
                }
            }
            else
            {
                if (!ignoreKing)
                {
                    // up 1 square
                    int newY = (int)position.y - 1;
                    if (CheckMove(new Vector2(position.x, newY)))
                    {
                        legalMoves.Add(tiles[(int)position.x, newY]);
                    }

                    // up 2 squares
                    newY = (int)position.y - 2;
                    if (CheckMove(new Vector2(position.x, newY))
                        && !hasPieceMoved)
                    {
                        legalMoves.Add(tiles[(int)position.x, newY]);
                    }

                    // enemy taking
                    // up 1, left 1
                    if (CheckMove(new Vector2(position.x - 1, position.y - 1), true))
                    {
                        legalMoves.Add(tiles[(int)position.x - 1, (int)position.y - 1]);
                    }

                    // up 1, right 1
                    if (CheckMove(new Vector2(position.x + 1, position.y - 1), true))
                    {
                        legalMoves.Add(tiles[(int)position.x + 1, (int)position.y - 1]);
                    }
                }

                else
                {
                    if (CheckMove(new Vector2(position.x - 1, position.y - 1), true)
                        || CheckMove(new Vector2(position.x - 1, position.y - 1)))
                    {
                        legalMoves.Add(tiles[(int)position.x - 1, (int)position.y - 1]);
                    }

                    // up 1, right 1
                    if (CheckMove(new Vector2(position.x + 1, position.y - 1), true)
                        || CheckMove(new Vector2(position.x + 1, position.y - 1)))
                    {
                        legalMoves.Add(tiles[(int)position.x + 1, (int)position.y - 1]);
                    }
                }
            }
        }

        // rook and straight queen
        if (pieceType == PieceManager.PieceType.rook || pieceType == PieceManager.PieceType.queen)
        {
            // up movement: Go from the y position to the top end of the board
            for (int newY = (int)position.y + 1; newY < 8; newY++)
            {
                if (!CheckMove(new Vector2((int)position.x, newY))) 
                { 
                    if (CheckMove(new Vector2((int)position.x, newY), true))
                    {
                        legalMoves.Add(tiles[(int)position.x, newY]);

                    }
                    break;
                }

                legalMoves.Add(tiles[(int)position.x, newY]);
            }
            // down movement
            for (int newY = (int)position.y - 1; newY >= 0; newY--)
            {
                if (!CheckMove(new Vector2((int)position.x, newY)))
                {
                    if (CheckMove(new Vector2((int)position.x, newY), true))
                    {
                        legalMoves.Add(tiles[(int)position.x, newY]);

                    }
                    break;
                }

                legalMoves.Add(tiles[(int)position.x, newY]);
            }
            // right movement
            for (int newX = (int)position.x + 1; newX < 8; newX++)
            {
                if (!CheckMove(new Vector2(newX, (int)position.y)))
                {
                    if (CheckMove(new Vector2(newX, (int)position.y), true))
                    {
                        legalMoves.Add(tiles[newX, (int)position.y]);

                    }
                    break;
                }

                legalMoves.Add(tiles[newX, (int)position.y]);
            }
            // left movement
            for (int newX = (int)position.x - 1; newX >= 0; newX--)
            {
                if (!CheckMove(new Vector2(newX, (int)position.y)))
                {
                    if (CheckMove(new Vector2(newX, (int)position.y), true))
                    {
                        legalMoves.Add(tiles[newX, (int)position.y]);

                    }
                    break;
                }

                legalMoves.Add(tiles[newX, (int)position.y]);
            }
        }

        // bishop and diagonal queen
        if (pieceType == PieceManager.PieceType.bishop || pieceType == PieceManager.PieceType.queen)
        {
            // right, up
            for (int extra = 1; extra <= 8; extra++)
            {
                if (!CheckMove(new Vector2(position.x + extra, position.y + extra)))
                {
                    if (CheckMove(new Vector2(position.x + extra, position.y + extra), true))
                    {
                        legalMoves.Add(tiles[(int)position.x + extra, (int)position.y + extra]);

                    }
                    break;
                }

                legalMoves.Add(tiles[(int)position.x + extra, (int)position.y + extra]);
            }

            // left, up
            for (int extra = 1; extra <= 8; extra++)
            {
                if (!CheckMove(new Vector2(position.x - extra, position.y + extra)))
                {
                    if (CheckMove(new Vector2(position.x - extra, position.y + extra), true))
                    {
                        legalMoves.Add(tiles[(int)position.x - extra, (int)position.y + extra]);

                    }
                    break;
                }

                legalMoves.Add(tiles[(int)position.x - extra, (int)position.y + extra]);
            }

            // right, down
            for (int extra = 1; extra <= 8; extra++)
            {
                if (!CheckMove(new Vector2(position.x + extra, position.y - extra)))
                {
                    if (CheckMove(new Vector2(position.x + extra, position.y - extra), true))
                    {
                        legalMoves.Add(tiles[(int)position.x + extra, (int)position.y - extra]);

                    }
                    break;
                }

                legalMoves.Add(tiles[(int)position.x + extra, (int)position.y - extra]);
            }

            // left, down
            for (int extra = 1; extra <= 8; extra++)
            {
                if (!CheckMove(new Vector2(position.x - extra, position.y - extra)))
                {
                    if (CheckMove(new Vector2(position.x - extra, position.y - extra), true))
                    {
                        legalMoves.Add(tiles[(int)position.x - extra, (int)position.y - extra]);

                    }
                    break;
                }

                legalMoves.Add(tiles[(int)position.x - extra, (int)position.y - extra]);
            }
        }

        // knight
        if (pieceType == PieceManager.PieceType.knight)
        {
            // 2r1u
            if (CheckMove(new Vector2(position.x + 2, position.y + 1)) 
                || CheckMove(new Vector2(position.x + 2, position.y + 1), true))
            {
                legalMoves.Add(tiles[(int)position.x + 2, (int)position.y + 1]);
            }

            // 2l1d
            if (CheckMove(new Vector2(position.x - 2, position.y - 1))
                || CheckMove(new Vector2(position.x - 2, position.y - 1), true))
            {
                legalMoves.Add(tiles[(int)position.x - 2, (int)position.y - 1]);
            }

            // 2r1d
            if (CheckMove(new Vector2(position.x + 2, position.y - 1))
                || CheckMove(new Vector2(position.x + 2, position.y - 1), true))
            {
                legalMoves.Add(tiles[(int)position.x + 2, (int)position.y - 1]);
            }

            // 2l1u
            if (CheckMove(new Vector2(position.x - 2, position.y + 1))
                || CheckMove(new Vector2(position.x - 2, position.y + 1), true))
            {
                legalMoves.Add(tiles[(int)position.x - 2, (int)position.y + 1]);
            }

            // 2u1l
            if (CheckMove(new Vector2(position.x - 1, position.y + 2))
                || CheckMove(new Vector2(position.x - 1, position.y + 2), true))
            {
                legalMoves.Add(tiles[(int)position.x - 1, (int)position.y + 2]);
            }

            // 2u1r
            if (CheckMove(new Vector2(position.x + 1, position.y + 2))
                || CheckMove(new Vector2(position.x + 1, position.y + 2), true))
            {
                legalMoves.Add(tiles[(int)position.x +1, (int)position.y +2]);
            }

            // 2d1l
            if (CheckMove(new Vector2(position.x - 1, position.y - 2))
                || CheckMove(new Vector2(position.x - 1, position.y - 2), true))
            {
                legalMoves.Add(tiles[(int)position.x - 1, (int)position.y - 2]);
            }

            // 2d1r
            if (CheckMove(new Vector2(position.x + 1, position.y - 2))
                || CheckMove(new Vector2(position.x + 1, position.y - 2), true))
            {
                legalMoves.Add(tiles[(int)position.x + 1, (int)position.y - 2]);
            }
        }

        // king
        if (pieceType == PieceManager.PieceType.king && ignoreKing == false)
        {
            Debug.Log("hi");
            List<GameObject> potentialLegalMoves = new List<GameObject>();

            // 1u
            if (CheckMove(new Vector2(position.x, position.y + 1))
                || CheckMove(new Vector2(position.x, position.y + 1), true))
            {
                Debug.Log("u1");
                potentialLegalMoves.Add(tiles[(int)position.x, (int)position.y + 1]);
            }
            // 1u1r
            if (CheckMove(new Vector2(position.x+1, position.y + 1))
                || CheckMove(new Vector2(position.x+1, position.y + 1), true))
            {
                potentialLegalMoves.Add(tiles[(int)position.x+1, (int)position.y + 1]);
            }
            // 1u1l
            if (CheckMove(new Vector2(position.x - 1, position.y + 1))
                || CheckMove(new Vector2(position.x - 1, position.y + 1), true))
            {
                potentialLegalMoves.Add(tiles[(int)position.x - 1, (int)position.y + 1]);
            }
            // 1l
            if (CheckMove(new Vector2(position.x - 1, position.y))
                || CheckMove(new Vector2(position.x - 1, position.y), true))
            {
                potentialLegalMoves.Add(tiles[(int)position.x - 1, (int)position.y]);
            }
            // 14
            if (CheckMove(new Vector2(position.x + 1, position.y))
                || CheckMove(new Vector2(position.x + 1, position.y), true))
            {
                potentialLegalMoves.Add(tiles[(int)position.x + 1, (int)position.y]);
            }
            // 1d
            if (CheckMove(new Vector2(position.x, position.y - 1))
                || CheckMove(new Vector2(position.x, position.y - 1), true))
            {
                potentialLegalMoves.Add(tiles[(int)position.x, (int)position.y - 1]);
            }
            // 1d1r
            if (CheckMove(new Vector2(position.x + 1, position.y - 1))
                || CheckMove(new Vector2(position.x + 1, position.y - 1), true))
            {
                potentialLegalMoves.Add(tiles[(int)position.x + 1, (int)position.y - 1]);
            }
            // 1d1l
            if (CheckMove(new Vector2(position.x - 1, position.y - 1))
                || CheckMove(new Vector2(position.x - 1, position.y - 1), true))
            {
                potentialLegalMoves.Add(tiles[(int)position.x - 1, (int)position.y - 1]);
            }

            // Get List of all enemy pieces' legal moves
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.GetChild(0).gameObject.activeInHierarchy && Convert.ToBoolean(tile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour())
                    != GetComponent<GameManager>().isPlayerWhite)
                {
                    List<GameObject> tempLegalMoves = GetMoves(tile, true);
                    foreach (GameObject legalMove in tempLegalMoves)
                    {
                        if (potentialLegalMoves.Contains(legalMove))
                        {
                            potentialLegalMoves.Remove(legalMove);
                        }
                    }
                }
            }

            // Removed all conflicting moves
            legalMoves = potentialLegalMoves;
        }

        return legalMoves;
    }

    private bool CheckMove(Vector2 newPos, bool isTakingEnemy = false)
    {
        if (!isTakingEnemy)
        {
            if (newPos.x >= 0 && newPos.x <= 7 && newPos.y >= 0 && newPos.y <= 7)
                return tiles[(int)newPos.x, (int)newPos.y].
                    transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.none;
            else
                return false;
        }
        else
        {
            if (newPos.x >= 0 && newPos.x <= 7 && newPos.y >= 0 && newPos.y <= 7)
                return tiles[(int)newPos.x, (int)newPos.y].
                    transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() != PieceManager.PieceType.none 
                    && Convert.ToBoolean(tiles[(int)newPos.x, (int)newPos.y].transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour()) 
                    != isPlayerWhite;
            else
                return false;
        }
    }
}
