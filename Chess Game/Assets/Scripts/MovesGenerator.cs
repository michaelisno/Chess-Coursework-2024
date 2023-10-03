using System;
using System.Collections.Generic;
using UnityEngine;

public class MovesGenerator : MonoBehaviour
{
    public List<GameObject> GenerateMoves(bool currentTeamColour, Vector2 position, Piece.PieceType pieceType, bool hasPieceMoved)
    {
        List<GameObject> legalMoves = new List<GameObject>();

        // pawn
        if (pieceType == Piece.PieceType.pawn)
        {
            // 1 up
            int n = Convert.ToInt32(8 * position.x + position.y + 1);
            if (n >= 0 && n <= 63 && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type 
                == Piece.PieceType.none)
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }

            // 2 up
            if (!hasPieceMoved)
            {
                n = Convert.ToInt32(8 * position.x + position.y + 2);
                if (n >= 0 && n <= 63 && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type
                    == Piece.PieceType.none)
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);
                }
            }

            // capture: up, left
            n = Convert.ToInt32(8 * position.x + position.y - 7);
            if (CheckLegality(n, Piece.PieceType.pawn, currentTeamColour))
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }

            // capture: up, right
            n = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32(position.y + 9);
            if (CheckLegality(n, Piece.PieceType.pawn, currentTeamColour))
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }
        }

        // knight
        if (pieceType == Piece.PieceType.knight)
        {
            int n = 0;
            // not in row 7 or 6
            if (position.y != 7 && position.y != 6 || (position.y > 1 && position.y < 6)) 
            {
                // up left
                n = Convert.ToInt32(8 * position.x + position.y - 6);
                if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                // up right
                n = Convert.ToInt32(8 * position.x + position.y + 10);
                if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                // left up
                n = Convert.ToInt32(8 * position.x + position.y - 15);
                if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                // right up
                n = Convert.ToInt32(8 * position.x + position.y + 17);
                if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                if (position.y == 1)
                {
                    // left down
                    n = Convert.ToInt32(8 * position.x + position.y + 15);
                    if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                        legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                    // right down
                    n = Convert.ToInt32(8 * position.x + position.y - 17);
                    if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                        legalMoves.Add(GetComponent<GameManager>().tiles[n]);
                }    
            }
            // in row 0 or 1
            if (position.y != 0 && position.y != 1 || (position.y > 1 && position.y < 6))
            {
                // up left
                n = Convert.ToInt32(8 * position.x + position.y + 6);
                if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                // up right
                n = Convert.ToInt32(8 * position.x + position.y - 10);
                if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                // left up
                n = Convert.ToInt32(8 * position.x + position.y + 15);
                if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                // right up
                n = Convert.ToInt32(8 * position.x + position.y - 17);
                if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                if (position.y == 6)
                {
                    // left down
                    n = Convert.ToInt32(8 * position.x + position.y - 15);
                    if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                        legalMoves.Add(GetComponent<GameManager>().tiles[n]);

                    // right down
                    n = Convert.ToInt32(8 * position.x + position.y + 17);
                    if (CheckLegality(n, Piece.PieceType.knight, currentTeamColour))
                        legalMoves.Add(GetComponent<GameManager>().tiles[n]);
                }
            } 
        }

        // rook
        if (pieceType == Piece.PieceType.rook)
        {
            // up
            for (int n = Convert.ToInt32(position.y); n < 7; n++) 
            {
                if (GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n+1].transform.GetChild(0).
                    GetComponent<Piece>().type == Piece.PieceType.none)
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n+1]);
                }
                else if (GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n+1].transform.GetChild(0).
                    GetComponent<Piece>().type != Piece.PieceType.none && GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n+1].transform.GetChild(0).
                    GetComponent<Piece>().colour != Convert.ToInt32(currentTeamColour))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n+1]);
                    break;
                }
                else
                {
                    break;
                }
            }

            // down
            for (int n = Convert.ToInt32(position.y); n > 0; n--)
            {
                if (GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n - 1].transform.GetChild(0).
                    GetComponent<Piece>().type == Piece.PieceType.none)
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n - 1]);
                }
                else if (GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n -1].transform.GetChild(0).
                    GetComponent<Piece>().type != Piece.PieceType.none && GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n - 1].transform.GetChild(0).
                    GetComponent<Piece>().colour != Convert.ToInt32(currentTeamColour))
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[Convert.ToInt32(position.x) * 8 + n - 1]);
                    break;
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
    private bool CheckLegality(int n, Piece.PieceType type, bool currentTeamColour)
    {
        switch (type)
        {
            case Piece.PieceType.knight:
                return n >= 0 && n <= 63 && (GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().
                    type == Piece.PieceType.none || (GetComponent<GameManager>().tiles[n].transform.GetChild(0).
                    GetComponent<Piece>().type != Piece.PieceType.none && GetComponent<GameManager>().tiles[n].transform.
                    GetChild(0).GetComponent<Piece>().colour != Convert.ToInt32(currentTeamColour)));
            case Piece.PieceType.pawn:
                return n >= 0 && n <= 63 && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type != Piece.PieceType.none
                && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().colour
                != Convert.ToInt32(currentTeamColour);
            default:
                return false;
        }
    }
}
