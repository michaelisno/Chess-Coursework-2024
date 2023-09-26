using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material[] tileMaterials;
    public Vector2 position;

    public void SetColour(int colour)
    {
        GetComponent<MeshRenderer>().material = tileMaterials[colour];
    }

    public void InitatePieces()
    {
        if (position.y == 0 || position.y == 7)
        {
            // pawns
            GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetPieceType(Piece.PieceType.pawn);
            GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetPieceColour(Convert.ToInt32(position.y <= 3));
        }
    }
}
