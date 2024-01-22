using System;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Material[] tileMaterials;

    private Vector2 position;
    private int colour;

    [SerializeField]
    private PieceManager pieceManager;

    public void SetPosition(int x, int y) {  position = new Vector2(x, y); }
    public void SetColour(int _colour)
    { 
        colour = _colour;
        GetComponent<Renderer>().material = tileMaterials[colour];
    }

    public void InitializePiece()
    {
        // pawns
        if (position.y == 1 || position.y == 6)
        {
            pieceManager.SetPieceType(PieceManager.PieceType.pawn);
            pieceManager.SetPieceColour(Convert.ToInt32(position.y <= 4));
        }
    }
}
