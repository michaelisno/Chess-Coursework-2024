using System;
using System.Collections.Generic;
using UnityEditor;
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
            if (GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type 
                == Piece.PieceType.none)
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }

            // 2 up
            if (!hasPieceMoved)
            {
                n = (Convert.ToInt32(position.x) * 8) + Convert.ToInt32((position.y) + 2);
                if (GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type
                    == Piece.PieceType.none)
                {
                    legalMoves.Add(GetComponent<GameManager>().tiles[n]);
                }
            }
        }

        return legalMoves;
    }
}
