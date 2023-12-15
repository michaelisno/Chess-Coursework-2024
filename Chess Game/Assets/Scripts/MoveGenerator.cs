using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveGenerator : MonoBehaviour
{
    public List<GameObject> GenerateMoves(bool isPlayerWhite, Vector2 position, Piece.PieceType pieceType, 
        bool hasPieceMoved, bool testKing, bool isChecking = false)
    {
        List<GameObject> legalMoves = new List<GameObject>();

        int tileIndex = 0;

        // pawn
        if (pieceType == Piece.PieceType.pawn)
        {
            // 1 up
            if (isPlayerWhite) tileIndex = Convert.ToInt32(8 * position.x + position.y + 1);
            else tileIndex = Convert.ToInt32(8 * position.x + position.y - 1);
            if (tileIndex >= 0 && tileIndex <= 63 && GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).
                GetComponent<Piece>().
                GetType() == Piece.PieceType.none)
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
            }

            // 2 up
            if (!hasPieceMoved && !isChecking)
            {
                if (isPlayerWhite) tileIndex = Convert.ToInt32(8 * position.x + position.y + 2);
                else tileIndex = Convert.ToInt32(8 * position.x + position.y - 2);
                if (tileIndex >= 0 && tileIndex <= 63 && GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).
                    GetComponent<Piece>().
                    GetType() == Piece.PieceType.none)
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
                }
            }

            // capture: up, left
            if (isPlayerWhite) tileIndex = Convert.ToInt32(8 * position.x + position.y - 7);
            else tileIndex = Convert.ToInt32(8 * position.x + position.y - 9);
            if (CheckLegality(tileIndex, Piece.PieceType.pawn, isPlayerWhite, testKing))
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
            }

            // capture: up, right
            if (isPlayerWhite) tileIndex = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32(position.y + 9);
            else tileIndex = Convert.ToInt32(8 * position.x + position.y + 7);
            if (CheckLegality(tileIndex, Piece.PieceType.pawn, isPlayerWhite, testKing))
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
            }
        }

        // knight
        if (pieceType == Piece.PieceType.knight)
        {
            // not in row 7 or 6
            if (position.y != 7 && position.y != 6 || (position.y > 1 && position.y < 6))
            {
                // up left
                tileIndex = Convert.ToInt32(8 * position.x + position.y - 6);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // up right
                tileIndex = Convert.ToInt32(8 * position.x + position.y + 10);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // left up
                tileIndex = Convert.ToInt32(8 * position.x + position.y - 15);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // right up
                tileIndex = Convert.ToInt32(8 * position.x + position.y + 17);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                if (position.y == 1)
                {
                    // left down
                    tileIndex = Convert.ToInt32(8 * position.x + position.y + 15);
                    if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                        legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                    // right down
                    tileIndex = Convert.ToInt32(8 * position.x + position.y - 17);
                    if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                        legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
                }
            }
            // in row 0 or 1
            if (position.y != 0 && position.y != 1 || (position.y > 1 && position.y < 6))
            {
                // up left
                tileIndex = Convert.ToInt32(8 * position.x + position.y + 6);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // up right
                tileIndex = Convert.ToInt32(8 * position.x + position.y - 10);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // left up
                tileIndex = Convert.ToInt32(8 * position.x + position.y + 15);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // right up
                tileIndex = Convert.ToInt32(8 * position.x + position.y - 17);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                if (position.y == 6)
                {
                    // left down
                    tileIndex = Convert.ToInt32(8 * position.x + position.y - 15);
                    if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                        legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                    // right down
                    tileIndex = Convert.ToInt32(8 * position.x + position.y + 17);
                    if (CheckLegality(tileIndex, Piece.PieceType.knight, isPlayerWhite, testKing))
                        legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
                }
            }
        }

        // rook AND lateral queen
        if (pieceType == Piece.PieceType.rook || pieceType == Piece.PieceType.queen)
        {
            // up
            for (tileIndex = Convert.ToInt32(position.y); tileIndex < 7; tileIndex++)
            {
                int tempIndex = Convert.ToInt32(position.x) * 8 + tileIndex + 1;
                if (CheckLegality(tempIndex, Piece.PieceType.rook, isPlayerWhite, testKing))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tempIndex]);

                    if (GetComponent<GameManager>().tiles[tempIndex].transform.
                        GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().
                        tiles[tempIndex].transform.GetChild(0).GetComponent<Piece>().GetColour() != Convert.ToInt32(isPlayerWhite))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // down
            for (tileIndex = Convert.ToInt32(position.y); tileIndex > 0; tileIndex--)
            {
                int tempIndex = Convert.ToInt32(position.x) * 8 + tileIndex - 1;
                if (CheckLegality(tempIndex, Piece.PieceType.rook, isPlayerWhite, testKing))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tempIndex]);

                    if (GetComponent<GameManager>().tiles[tempIndex].transform.
                        GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().
                        tiles[tempIndex].transform.GetChild(0).GetComponent<Piece>().GetColour() != Convert.ToInt32(isPlayerWhite))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // right
            for (tileIndex = Convert.ToInt32(position.x); tileIndex < 7; tileIndex++)
            {
                int tempIndex = Convert.ToInt32(position.y) + (tileIndex + 1) * 8;
                if (CheckLegality(tempIndex, Piece.PieceType.rook, isPlayerWhite, testKing))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tempIndex]);

                    if (GetComponent<GameManager>().tiles[tempIndex].transform.
                        GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().
                        tiles[tempIndex].transform.GetChild(0).GetComponent<Piece>().GetColour() != Convert.ToInt32(isPlayerWhite))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // left
            for (tileIndex = Convert.ToInt32(position.x); tileIndex > 0; tileIndex--)
            {
                int tempIndex = Convert.ToInt32(position.y) + (tileIndex - 1) * 8;
                if (CheckLegality(tempIndex, Piece.PieceType.rook, isPlayerWhite, testKing))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tempIndex]);

                    if (GetComponent<GameManager>().tiles[tempIndex].transform.
                        GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().
                        tiles[tempIndex].transform.GetChild(0).GetComponent<Piece>().GetColour() != Convert.ToInt32(isPlayerWhite))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        // bishop AND diagonal queen
        if (pieceType == Piece.PieceType.bishop || pieceType == Piece.PieceType.queen)
        {
            int originPosition = Convert.ToInt32(8 * position.x + position.y);

            // right up
            for (int testedPosition = Convert.ToInt32(position.x * 8 + position.y) + 9; testedPosition < 63; testedPosition += 9)
            {
                if (CheckLegality(testedPosition, Piece.PieceType.bishop, isPlayerWhite, testKing, originPosition))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[testedPosition]);

                    if (GetComponent<GameManager>().tiles[testedPosition].transform.
                        GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().
                        tiles[testedPosition].transform.GetChild(0).GetComponent<Piece>().GetColour() != 
                        Convert.ToInt32(isPlayerWhite))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // left up
            for (int testedPosition = Convert.ToInt32(position.x * 8 + position.y) - 7; testedPosition > 0; testedPosition -= 7)
            {
                if (CheckLegality(testedPosition, Piece.PieceType.bishop, isPlayerWhite, testKing, originPosition))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[testedPosition]);

                    if (GetComponent<GameManager>().tiles[testedPosition].transform.
                        GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().
                        tiles[testedPosition].transform.GetChild(0).GetComponent<Piece>().GetColour() !=
                        Convert.ToInt32(isPlayerWhite))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // right down
            for (int testedPosition = Convert.ToInt32(position.x * 8 + position.y) + 7; testedPosition < 63; testedPosition += 7)
            {
                if (CheckLegality(testedPosition, Piece.PieceType.bishop, isPlayerWhite, testKing, originPosition))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[testedPosition]);

                    if (GetComponent<GameManager>().tiles[testedPosition].transform.
                        GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().
                        tiles[testedPosition].transform.GetChild(0).GetComponent<Piece>().GetColour() !=
                        Convert.ToInt32(isPlayerWhite))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // left down
            for (int testedPosition = Convert.ToInt32(position.x * 8 + position.y) - 9; testedPosition > 0; testedPosition -= 9)
            {
                if (CheckLegality(testedPosition, Piece.PieceType.bishop, isPlayerWhite, testKing, originPosition))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[testedPosition]);

                    if (GetComponent<GameManager>().tiles[testedPosition].transform.
                        GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().
                        tiles[testedPosition].transform.GetChild(0).GetComponent<Piece>().GetColour() !=
                        Convert.ToInt32(isPlayerWhite))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        // king
        if (pieceType == Piece.PieceType.king)
        {
            int currentPosition = Convert.ToInt32(position.x * 8 + position.y);
            if (CheckLegality(currentPosition + 1, Piece.PieceType.king, isPlayerWhite, testKing) && position.y < 7)
                legalMoves.Add(GetComponent<GameManager>().tiles[currentPosition + 1]);
                GetComponent<GameManager>().SetKingIndex(currentPosition + 1, Convert.ToInt32(isPlayerWhite));
            if (CheckLegality(currentPosition - 1, Piece.PieceType.king, isPlayerWhite, testKing) && position.y > 0)
                legalMoves.Add(GetComponent<GameManager>().tiles[currentPosition - 1]);
                GetComponent<GameManager>().SetKingIndex(currentPosition - 1, Convert.ToInt32(isPlayerWhite));
            if (CheckLegality(currentPosition - 7, Piece.PieceType.king, isPlayerWhite, testKing) && position.y < 7)
                legalMoves.Add(GetComponent<GameManager>().tiles[currentPosition - 7]);
                GetComponent<GameManager>().SetKingIndex(currentPosition - 7, Convert.ToInt32(isPlayerWhite));
            if (CheckLegality(currentPosition + 9, Piece.PieceType.king, isPlayerWhite, testKing) && position.y < 7)
                legalMoves.Add(GetComponent<GameManager>().tiles[currentPosition + 9]);
                GetComponent<GameManager>().SetKingIndex(currentPosition + 9, Convert.ToInt32(isPlayerWhite));
            if (CheckLegality(currentPosition + 8, Piece.PieceType.king, isPlayerWhite, testKing))
                legalMoves.Add(GetComponent<GameManager>().tiles[currentPosition + 8]);
                GetComponent<GameManager>().SetKingIndex(currentPosition + 8, Convert.ToInt32(isPlayerWhite));
            if (CheckLegality(currentPosition - 8, Piece.PieceType.king, isPlayerWhite, testKing))
                legalMoves.Add(GetComponent<GameManager>().tiles[currentPosition - 8]);
                GetComponent<GameManager>().SetKingIndex(currentPosition - 8, Convert.ToInt32(isPlayerWhite));
            if (CheckLegality(currentPosition - 9, Piece.PieceType.king, isPlayerWhite, testKing) && position.y > 0)
                legalMoves.Add(GetComponent<GameManager>().tiles[currentPosition - 9]);
                GetComponent<GameManager>().SetKingIndex(currentPosition - 9, Convert.ToInt32(isPlayerWhite));
            if (CheckLegality(currentPosition + 7, Piece.PieceType.king, isPlayerWhite, testKing) && position.y > 0)
                legalMoves.Add(GetComponent<GameManager>().tiles[currentPosition + 7]);
                GetComponent<GameManager>().SetKingIndex(currentPosition + 7, Convert.ToInt32(isPlayerWhite));
        }

        return legalMoves;
    }

    // Seperate function used to check for legality; to prevent repeated identitcal if statements
    private bool CheckLegality(int tileIndex, Piece.PieceType type, bool isPlayerWhite, bool testKing, int originPosition = 0)
    {
        if (tileIndex >= 0 && tileIndex <= 63)
        {
            Piece piece = GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).GetComponent<Piece>();
            Tile tile = GetComponent<GameManager>().tiles[tileIndex].GetComponent<Tile>();

            if (testKing)
            {
                switch (type)
                {
                    case Piece.PieceType.knight:
                        return piece.GetType() == Piece.PieceType.none || (piece.GetType() != Piece.PieceType.none && piece.GetColour()
                            != Convert.ToInt32(isPlayerWhite)) && piece.GetType() != Piece.PieceType.king;
                    case Piece.PieceType.pawn:
                        return piece.GetType() != Piece.PieceType.none && piece.GetColour() != Convert.ToInt32(isPlayerWhite) &&
                            piece.GetType() != Piece.PieceType.king;
                    case Piece.PieceType.rook:
                        return piece.GetType() == Piece.PieceType.none || (piece.GetType() != Piece.PieceType.none && piece.GetColour()
                            != Convert.ToInt32(isPlayerWhite)) && piece.GetType() != Piece.PieceType.king;
                    case Piece.PieceType.bishop:
                        return ((piece.GetType() == Piece.PieceType.none) || piece.GetType() != Piece.PieceType.none && piece.GetColour()
                            != Convert.ToInt32(isPlayerWhite)) && tile.GetColour() == GetComponent<GameManager>().
                            tiles[originPosition].GetComponent<Tile>().GetColour() && piece.GetType() != Piece.PieceType.king;
                    case Piece.PieceType.king:
                        return (piece.GetType() == Piece.PieceType.none || (piece.GetType() != Piece.PieceType.none && piece.GetColour()
                            != Convert.ToInt32(isPlayerWhite)) && piece.GetType() != Piece.PieceType.king) && testCheck(tileIndex);
                    default:
                        return false;
                }
            }
            else
            {
                switch (type)
                {
                    case Piece.PieceType.knight:
                        return piece.GetType() == Piece.PieceType.none || (piece.GetType() != Piece.PieceType.none && piece.GetColour()
                            != Convert.ToInt32(isPlayerWhite));
                    case Piece.PieceType.pawn:
                        return piece.GetType() != Piece.PieceType.none && piece.GetColour() != Convert.ToInt32(isPlayerWhite);
                    case Piece.PieceType.rook:
                        return piece.GetType() == Piece.PieceType.none || (piece.GetType() != Piece.PieceType.none && piece.GetColour()
                            != Convert.ToInt32(isPlayerWhite));
                    case Piece.PieceType.bishop:
                        return ((piece.GetType() == Piece.PieceType.none) || piece.GetType() != Piece.PieceType.none && piece.GetColour()
                            != Convert.ToInt32(isPlayerWhite)) && tile.GetColour() == GetComponent<GameManager>().
                            tiles[originPosition].GetComponent<Tile>().GetColour();
                    case Piece.PieceType.king:
                        return (piece.GetType() == Piece.PieceType.none || (piece.GetType() != Piece.PieceType.none && piece.GetColour()
                            != Convert.ToInt32(isPlayerWhite)) && piece.GetType() != Piece.PieceType.king) && testCheck(tileIndex);
                    default:
                        return false;
                }
            }
        }
        else
        {
            return false;
        }
    }

    public bool testCheck(int tileIndex)
    {
        List<GameObject> tiles = GetComponent<GameManager>().tiles;

        bool canMove = true;

        for (int i = 0; i < tiles.Count(); i++)
        {
            if (!canMove) break;

            if (tiles[i].transform.GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none
                && tiles[i].transform.GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.king
                && tiles[i].transform.GetChild(0).GetComponent<Piece>().GetColour() !=
                Convert.ToInt32(GetComponent<GameManager>().isPlayerWhite))
            {
                List<GameObject> legalMoves = GenerateMoves(!GetComponent<GameManager>().isPlayerWhite, 
                    tiles[i].GetComponent<Tile>().GetPosition(), tiles[i].transform.GetChild(0).GetComponent<Piece>().
                    GetType(), tiles[i].transform.GetChild(0).GetComponent<Piece>().hasPieceMoved, true);

                for (int j = 0; j < legalMoves.Count(); j++)
                {
                    if (tileIndex == GetComponent<GameManager>().tiles.IndexOf(legalMoves[j]))
                    {
                        canMove = false;
                        break;
                    }
                }
            }
        }

        return canMove;
    }
}