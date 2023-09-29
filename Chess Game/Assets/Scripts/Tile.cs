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
        if (position.y == 1 || position.y == 6) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetPieceType(Piece.PieceType.pawn);
        if (position.y == 0 || position.y == 7)
        {
            if (position.x == 0 || position.x == 7) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetPieceType(Piece.PieceType.rook);
            if (position.x == 1 || position.x == 6) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetPieceType(Piece.PieceType.knight);
            if (position.x == 2 || position.x == 5) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetPieceType(Piece.PieceType.bishop);
            if (position.x == 3) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetPieceType(Piece.PieceType.queen);
            if (position.x == 4) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetPieceType(Piece.PieceType.king);
        }

        // set piece colour based on position
        GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetPieceColour(Convert.ToInt32(position.y <= 3));
    }
}
