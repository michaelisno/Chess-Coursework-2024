using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveGenerator : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject[,] tiles;

    private bool isPlayerWhite;

    public List<GameObject> GetMoves(GameObject selectedTile, bool ignoreKing = false, bool _isPlayerWhite = true, int direction = -1)
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

        isPlayerWhite = _isPlayerWhite;

        //if (ignoreKing)
        //   isPlayerWhite = !GetComponent<GameManager>().isPlayerWhite;

        //if (_isPlayerWhite != GetComponent<GameManager>().isPlayerWhite)
        //{
        //    isPlayerWhite = false;
        //}

        return GenerateLegalMoves(pieceType, position, tiles, hasPieceMoved, piece, ignoreKing, direction);
    }

    private List<GameObject> GenerateLegalMoves(PieceManager.PieceType pieceType, Vector2 position, 
        GameObject[,] tiles, bool hasPieceMoved, PieceManager piece, bool ignoreKing, int direction)
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

                        // up 2 squares
                        newY = (int)position.y + 2;
                        if (CheckMove(new Vector2(position.x, newY))
                            && !hasPieceMoved)
                        {
                            legalMoves.Add(tiles[(int)position.x, newY]);
                        }
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

                        // up 2 squares
                        newY = (int)position.y - 2;
                        if (CheckMove(new Vector2(position.x, newY))
                            && !hasPieceMoved)
                        {
                            legalMoves.Add(tiles[(int)position.x, newY]);
                        }
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
            if (direction == -1)
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
            else if (direction == 0)
            {
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
            }
            else if (direction == 1)
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
            }
            else if (direction == 2)
            {
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
            else if (direction == 3)
            {
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
            }
        }

        // bishop and diagonal queen
        if (pieceType == PieceManager.PieceType.bishop || pieceType == PieceManager.PieceType.queen)
        {
            if (direction == -1)
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
            else if (direction == 0)
            {
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
            }
            else if (direction == 1)
            {
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
            else if (direction == 2)
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
            }
            else if (direction == 3)
            {
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
            List<GameObject> potentialLegalMoves = new List<GameObject>();

            // 1u
            if (CheckMove(new Vector2(position.x, position.y + 1))
                || CheckMove(new Vector2(position.x, position.y + 1), true))
            {
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

            foreach (GameObject t in potentialLegalMoves)
            {
                Debug.Log(t.name);
            }

            // Get List of all enemy pieces' legal moves
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.GetChild(0).gameObject.activeInHierarchy && 
                    Convert.ToBoolean(tile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour()) != isPlayerWhite)
                {
                    List<GameObject> tempLegalMoves = GetMoves(tile, true, isPlayerWhite);
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

        if (!GetComponent<GameManager>().isPlayerChecked)
            return legalMoves;
        else
        {
            List<GameObject> checkerLegalMoves = new List<GameObject>();
            GameObject checkerTile = GetComponent<GameManager>().checkerTile;
            GameObject checkedKingTile = GetComponent<GameManager>().
                tiles[(int)GetComponent<GameManager>().whiteKingPosition.x,
                (int)GetComponent<GameManager>().whiteKingPosition.y];

            // If checker is rook or queen
            if (checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.rook
                || checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.queen)
            {
                Debug.Log("CHECKING PIECE IS A ROOK! or Queen");
                int xDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().x
                    - (int)checkerTile.GetComponent<TileManager>().GetTilePosition().x;
                int yDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().y
                    - (int)checkerTile.GetComponent<TileManager>().GetTilePosition().y;

                if (xDiff == 0)
                {
                    if (yDiff >= 0)
                    {
                        // king is below rook
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isPlayerWhite, 1);
                    }
                    if (yDiff < 0)
                    {
                        // king is above rook
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isPlayerWhite, 0);
                    }
                }
                else
                {
                    if (xDiff >= 0)
                    {
                        // king is left of rook
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isPlayerWhite, 3);
                    }
                    if (yDiff < 0)
                    {
                        // king is right of rook
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isPlayerWhite, 2);
                    }
                }
            }

            // If checker is bishop or queen
            if (checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.bishop
                || checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.queen)
            {
                Debug.Log("CHECKING PIECE IS A BISHOP! or Queen");
                int xDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().x
                    - (int)checkerTile.GetComponent<TileManager>().GetTilePosition().x;
                int yDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().y
                    - (int)checkerTile.GetComponent<TileManager>().GetTilePosition().y;

                if (xDiff < 0 && yDiff < 0)
                {
                    // king is up, left from bishop
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isPlayerWhite, 0);
                }
                else if (xDiff < 0 && yDiff > 0)
                {
                    // king is down, left from bishop
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isPlayerWhite, 1);
                }
                else if (xDiff > 0 && yDiff < 0)
                {
                    // king is up, right from bishop
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isPlayerWhite, 2);
                }
                else
                {
                    // king is down, right from bishop
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isPlayerWhite, 3);
                }
            }

            // if checker is knight or pawn
            if (checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.knight
                || checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.pawn)
            {
                checkerLegalMoves.Add(checkerTile);
            }

            checkerLegalMoves.Add(GetComponent<GameManager>().tiles[(int)checkerTile.GetComponent<TileManager>().GetTilePosition().x,
            (int)checkerTile.GetComponent<TileManager>().GetTilePosition().y]);

            List<GameObject> finalMoves = new List<GameObject>();

            foreach (GameObject move in legalMoves)
            {
                if (checkerLegalMoves.Contains(move))
                { 
                    finalMoves.Add(move);
                }
            }

            return finalMoves;
        }
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
                    && Convert.ToBoolean(tiles[(int)newPos.x, (int)newPos.y].transform.GetChild(0).
                    GetComponent<PieceManager>().GetPieceColour()) 
                    != isPlayerWhite;
            else
                return false;
        }
    }
}
