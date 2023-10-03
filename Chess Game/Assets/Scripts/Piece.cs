using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceType
    { 
        none,
        pawn,
        rook,
        knight,
        bishop,
        queen,
        king
    }

    public PieceType type;
    public int colour;

    public Mesh[] pieceModels;
    public Material[] pieceMaterials;

    public bool hasPieceMoved;

    public PieceType SetPieceType(PieceType pieceType)
    {
        gameObject.SetActive(true);
        type = pieceType;

        switch(pieceType)
        {
            case PieceType.none:
                gameObject.SetActive(false);
                break;
            case PieceType.pawn:
                GetComponent<MeshFilter>().mesh = pieceModels[0];
                break;
            case PieceType.rook:
                GetComponent<MeshFilter>().mesh = pieceModels[1];
                break;
            case PieceType.knight:
                GetComponent<MeshFilter>().mesh = pieceModels[2];
                break;
            case PieceType.bishop:
                GetComponent<MeshFilter>().mesh = pieceModels[3];
                break;
            case PieceType.queen:
                GetComponent<MeshFilter>().mesh = pieceModels[4];
                break;
            case PieceType.king:
                GetComponent<MeshFilter>().mesh = pieceModels[5];
                break;
        }

        return pieceType;
    }

    public void SetPieceColour(int _colour)
    {
        colour = _colour;

        Material[] materials = GetComponent<MeshRenderer>().materials;
        materials[1] = pieceMaterials[_colour];
        GetComponent<MeshRenderer>().materials = materials;
    }
}
