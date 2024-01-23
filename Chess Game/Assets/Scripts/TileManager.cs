using System;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Material[] tileMaterials;

    private Vector2 position;
    private int colour;

    [SerializeField]
    private PieceManager pieceManager;

    // Setters
    public void SetTilePosition(int x, int y) {  position = new Vector2(x, y); }
    public void SetTileColour(int _colour)
    { 
        colour = _colour;
        GetComponent<Renderer>().material = tileMaterials[colour];
    }

    // Getters
    public Vector2 GetTilePosition() { return position; }
    public int GetTileColour() { return colour; }

    public void InitializePiece()
    {
        int colour = Convert.ToInt32(position.y <= 4);

        if (position.y == 1 || position.y == 6)
        {
            // pawns
            pieceManager.SetPieceType(PieceManager.PieceType.pawn);
            pieceManager.SetPieceColour(colour);
        }
        else if (position.y == 0 && (position.x == 0 || position.x == 7) || position.y == 7 && (position.x == 0 || position.x == 7))
        {
            // rooks
            pieceManager.SetPieceType(PieceManager.PieceType.rook);
            pieceManager.SetPieceColour(colour);
        }
        else if (position.y == 0 && (position.x == 1 || position.x == 6) || position.y == 7 && (position.x == 1 || position.x == 6))
        {
            // knights
            pieceManager.SetPieceType(PieceManager.PieceType.knight);
            pieceManager.SetPieceColour(colour);
        }
        else if (position.y == 0 && (position.x == 2 || position.x == 5) || position.y == 7 && (position.x == 2 || position.x == 5))
        {
            // bishops
            pieceManager.SetPieceType(PieceManager.PieceType.bishop);
            pieceManager.SetPieceColour(colour);
        }
        else if (position.x == 3 && (position.y == 0 || position.y == 7))
        {
            // queens
            pieceManager.SetPieceType(PieceManager.PieceType.queen);
            pieceManager.SetPieceColour(colour);
        }
        else if (position.x == 4 && (position.y == 0 || position.y == 7))
        {
            // kings
            pieceManager.SetPieceType(PieceManager.PieceType.king);
            pieceManager.SetPieceColour(colour);
        }
        else
        {
            pieceManager.SetPieceType(PieceManager.PieceType.none);
        }
    }
}
