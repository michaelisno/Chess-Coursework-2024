using Unity.VisualScripting;
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

    public Mesh[] pieceModels;
    public Material blackPieceMaterial, whitePieceMaterial;

    public PieceType SetPieceType(PieceType pieceType)
    {
        gameObject.SetActive(true);

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

    public void SetPieceColour(int colour)
    {
        Material[] materials = GetComponent<MeshRenderer>().materials;
        if (colour == 0)
        {
            materials[1] = blackPieceMaterial;
        }
        else
        {
            materials[1] = whitePieceMaterial;
        }
        GetComponent<MeshRenderer>().materials = materials;
    }
}
