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

    [SerializeField]
    private Mesh[] pieceModels;
    [SerializeField]
    private Material[] pieceMaterials;

    public bool hasPieceMoved;

    private PieceType type;
    private int colour;

    // Getters
    public PieceType GetType() { return type; }
    public int GetColour() { return colour; }

    // Setters
    public void SetType(PieceType _type)
    {
        gameObject.SetActive(true);
        type = _type;

        switch(type)
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
    }

    public void SetColour(int _colour)
    {
        colour = _colour;
        Material[] materials = GetComponent<MeshRenderer>().materials;
        materials[1] = pieceMaterials[colour];
        GetComponent<MeshRenderer>().materials = materials;
    }
}
