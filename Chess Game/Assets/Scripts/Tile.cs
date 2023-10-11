using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Material[] tileMaterials;

    private Vector2 position;

    private int colour;

    // Setters
    public void SetColour(int _colour) { GetComponent<MeshRenderer>().material = tileMaterials[_colour]; colour = _colour; }
    public void SetPosition(Vector2 _position) { position = _position; }

    // Getters
    public Vector2 GetPosition() { return position; }
    public int GetColour() { return colour; }

    public void InitatePieces()
    {
        if (position.y == 1 || position.y == 6) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetType(Piece.PieceType.pawn);
        if (position.y == 0 || position.y == 7)
        {
            if (position.x == 0 || position.x == 7) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetType(Piece.PieceType.rook);
            if (position.x == 1 || position.x == 6) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetType(Piece.PieceType.knight);
            if (position.x == 2 || position.x == 5) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetType(Piece.PieceType.bishop);
            if (position.x == 3) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetType(Piece.PieceType.queen);
            if (position.x == 4) GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetType(Piece.PieceType.king);
        }

        // Set colour based on position
        GetComponent<Transform>().GetChild(0).GetComponent<Piece>().SetColour(Convert.ToInt32(position.y <= 3));
    }
}
