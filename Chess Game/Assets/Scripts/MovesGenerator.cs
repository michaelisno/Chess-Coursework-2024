using System;
using System.Collections.Generic;
using UnityEngine;

public class MovesGenerator : MonoBehaviour
{
    public List<GameObject> GenerateMoves(bool currentTeamColour, Vector2 position, Piece.PieceType pieceType)
    {
        List<GameObject> legalMoves = new List<GameObject>();

        // pawn
        if (pieceType == Piece.PieceType.pawn)
        {
            int n = (Convert.ToInt32(position.x) *8) + Convert.ToInt32(position.y) + 1;
            // 1 up
            if (GetComponent<GameManager>().tiles[n].transform.GetChild(0).GetComponent<Piece>().type 
                == Piece.PieceType.none)
            {
                legalMoves.Add(GetComponent<GameManager>().tiles[n]);
            }
        }

        return legalMoves;
    }
}
