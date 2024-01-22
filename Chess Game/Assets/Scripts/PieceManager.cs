using UnityEngine;

public class PieceManager : MonoBehaviour
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

    public Mesh[] pieceMeshes;
    public Material[] pieceMaterials;

    private PieceType pieceType = PieceType.none;
    private int colour;

    public void SetPieceColour(int _colour)
    { 
        colour = _colour;
        Material[] materials = GetComponent<MeshRenderer>().materials;
        materials[1] = pieceMaterials[colour];
        GetComponent<MeshRenderer>().materials = materials;
    }

    public void SetPieceType(PieceType newType) 
    { 
        pieceType = newType;

        if (pieceType != PieceType.none)
        {
            gameObject.SetActive(true);
            GetComponent<MeshFilter>().mesh = pieceMeshes[(int)pieceType-1];
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }
}
