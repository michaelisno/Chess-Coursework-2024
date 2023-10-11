using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MoveGenerator : MonoBehaviour
{
    public List<GameObject> GenerateMoves(bool currentTeamColour, Vector2 position, Piece.PieceType pieceType, bool hasPieceMoved)
    {
        List<GameObject> legalMoves = new List<GameObject>();

        int tileIndex = 0;

        // pawn
        if (pieceType == Piece.PieceType.pawn)
        {
            // 1 up
            tileIndex = Convert.ToInt32(8 * position.x + position.y + 1);
            if (tileIndex >= 0 && tileIndex <= 63 && GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).GetComponent<Piece>().GetType()
                == Piece.PieceType.none)
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
            }

            // 2 up
            if (!hasPieceMoved)
            {
                tileIndex = Convert.ToInt32(8 * position.x + position.y + 2);
                if (tileIndex >= 0 && tileIndex <= 63 && GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).GetComponent<Piece>().GetType()
                    == Piece.PieceType.none)
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
                }
            }

            // capture: up, left
            tileIndex = Convert.ToInt32(8 * position.x + position.y - 7);
            if (CheckLegality(tileIndex, Piece.PieceType.pawn, currentTeamColour))
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
            }

            // capture: up, right
            tileIndex = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32(position.y + 9);
            if (CheckLegality(tileIndex, Piece.PieceType.pawn, currentTeamColour))
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
                if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // up right
                tileIndex = Convert.ToInt32(8 * position.x + position.y + 10);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // left up
                tileIndex = Convert.ToInt32(8 * position.x + position.y - 15);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // right up
                tileIndex = Convert.ToInt32(8 * position.x + position.y + 17);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                if (position.y == 1)
                {
                    // left down
                    tileIndex = Convert.ToInt32(8 * position.x + position.y + 15);
                    if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                        legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                    // right down
                    tileIndex = Convert.ToInt32(8 * position.x + position.y - 17);
                    if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                        legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
                }
            }
            // in row 0 or 1
            if (position.y != 0 && position.y != 1 || (position.y > 1 && position.y < 6))
            {
                // up left
                tileIndex = Convert.ToInt32(8 * position.x + position.y + 6);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // up right
                tileIndex = Convert.ToInt32(8 * position.x + position.y - 10);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // left up
                tileIndex = Convert.ToInt32(8 * position.x + position.y + 15);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                // right up
                tileIndex = Convert.ToInt32(8 * position.x + position.y - 17);
                if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                if (position.y == 6)
                {
                    // left down
                    tileIndex = Convert.ToInt32(8 * position.x + position.y - 15);
                    if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                        legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);

                    // right down
                    tileIndex = Convert.ToInt32(8 * position.x + position.y + 17);
                    if (CheckLegality(tileIndex, Piece.PieceType.knight, currentTeamColour))
                        legalMoves.Add(GetComponent<GameManager>().tiles[tileIndex]);
                }
            }
        }

        // rook
        if (pieceType == Piece.PieceType.rook)
        {
            // up
            for (tileIndex = Convert.ToInt32(position.y); tileIndex < 7; tileIndex++)
            {
                int tempIndex = Convert.ToInt32(position.x) * 8 + tileIndex + 1;
                if (CheckLegality(tempIndex, Piece.PieceType.rook, currentTeamColour))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tempIndex]);
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
                if (CheckLegality(tempIndex, Piece.PieceType.rook, currentTeamColour))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tempIndex]);
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
                if (CheckLegality(tempIndex, Piece.PieceType.rook, currentTeamColour))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tempIndex]);
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
                if (CheckLegality(tempIndex, Piece.PieceType.rook, currentTeamColour))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[tempIndex]);
                }
                else
                {
                    break;
                }
            }
        }

        // bishop
        if (pieceType == Piece.PieceType.bishop)
        {
            int originPosition = Convert.ToInt32(8 * position.x + position.y);

            // right up
            for (int initialPosition = Convert.ToInt32(position.x * 8 + position.y) + 9; initialPosition < 63; initialPosition += 9)
            {
                if (CheckLegality(initialPosition, Piece.PieceType.bishop, currentTeamColour, originPosition))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[initialPosition]);
                }
                else
                {
                    break;
                }
            }

            // left up
            for (int initialPosition = Convert.ToInt32(position.x * 8 + position.y) - 7; initialPosition > 0; initialPosition -= 7)
            {
                if (CheckLegality(initialPosition, Piece.PieceType.bishop, currentTeamColour, originPosition))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[initialPosition]);
                }
                else
                {
                    break;
                }
            }

            // right down
            for (int initialPosition = Convert.ToInt32(position.x * 8 + position.y) + 7; initialPosition < 63; initialPosition += 7)
            {
                if (CheckLegality(initialPosition, Piece.PieceType.bishop, currentTeamColour, originPosition))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[initialPosition]);
                }
                else
                {
                    break;
                }
            }

            // left down
            for (int initialPosition = Convert.ToInt32(position.x * 8 + position.y) - 9; initialPosition > 0; initialPosition -= 9)
            {

                if (CheckLegality(initialPosition, Piece.PieceType.bishop, currentTeamColour, originPosition))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[initialPosition]);
                }
                else
                {
                    break;
                }
            }
        }

        return legalMoves;
    }

    // Seperate function used to check for legality; to prevent repeated identitcal if statements
    private bool CheckLegality(int tileIndex, Piece.PieceType type, bool currentTeamColour, int originPosition = 0)
    {
        switch (type)
        {
            case Piece.PieceType.knight:
                return tileIndex >= 0 && tileIndex <= 63 && (GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).GetComponent<Piece>().
                    GetType() == Piece.PieceType.none || (GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).
                    GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().tiles[tileIndex].transform.
                    GetChild(0).GetComponent<Piece>().GetColour() != Convert.ToInt32(currentTeamColour)));
            case Piece.PieceType.pawn:
                return tileIndex >= 0 && tileIndex <= 63 && GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).GetComponent<Piece>().
                    GetType() != Piece.PieceType.none && GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).
                    GetComponent<Piece>().GetColour() != Convert.ToInt32(currentTeamColour);
            case Piece.PieceType.rook:
                return tileIndex >= 0 && tileIndex <= 63 && (GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).
                    GetComponent<Piece>().GetType() == Piece.PieceType.none) || (GetComponent<GameManager>().tiles[tileIndex].transform.
                    GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().tiles[tileIndex].
                    transform.GetChild(0).GetComponent<Piece>().GetColour() != Convert.ToInt32(currentTeamColour));
            case Piece.PieceType.bishop:
                return (tileIndex >= 0 && tileIndex <= 63 && (GetComponent<GameManager>().tiles[tileIndex].transform.GetChild(0).
                    GetComponent<Piece>().GetType() == Piece.PieceType.none) || GetComponent<GameManager>().tiles[tileIndex].transform.
                    GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none && GetComponent<GameManager>().tiles[tileIndex].
                    transform.GetChild(0).GetComponent<Piece>().GetColour() != Convert.ToInt32(currentTeamColour)) &&
                    GetComponent<GameManager>().tiles[tileIndex].GetComponent<Tile>().GetColour() == GetComponent<GameManager>().
                    tiles[originPosition].GetComponent<Tile>().GetColour();
            default:
                return false;
        }
    }
}
