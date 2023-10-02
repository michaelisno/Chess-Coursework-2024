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
            int n = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32(position.y) + 1;
            if (n >= 0 && n <= 63 && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type 
                == Piece.PieceType.none)
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }

            // 2 up
            if (!hasPieceMoved)
            {
                n = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32(position.y + 2);
                if (n >= 0 && n <= 63 && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type
                    == Piece.PieceType.none)
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);
                }
            }

            // piece taking diagonally up, left
            n = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32(position.y - 7);
            if (n >= 0 && n <= 63 && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type != Piece.PieceType.none 
                && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().colour 
                != Convert.ToInt32(currentTeamColour))
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }

            // piece taking diagonally up, right
            n = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32(position.y + 9);
            if (n >= 0 && n <= 63 && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type != Piece.PieceType.none
                && GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().colour
                != Convert.ToInt32(currentTeamColour))
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }
        }

        // knight
        if (pieceType == Piece.PieceType.knight)
        {
            // up, left
            int n = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32(position.y - 6);
            if (n >= 0 && n <= 63 && (GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().colour 
                != Convert.ToInt32(currentTeamColour)) || GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type
                == Piece.PieceType.none)
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }

            // up, right
            n = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32(position.y + 10);
            if (n >= 0 && n <= 63 && (GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().colour
                != Convert.ToInt32(currentTeamColour)) || GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type
                == Piece.PieceType.none)
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }
        }

        return legalMoves;
    }
}
