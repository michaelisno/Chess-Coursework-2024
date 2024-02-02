using System;
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

    private PieceType pieceType;
    private int colour;

    private bool hasPieceMoved = false;

    // Setters
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

        if (pieceType == PieceType.none)
        {
            gameObject.SetActive(false);
            return;
        }

        GetComponent<MeshFilter>().mesh = pieceMeshes[(int)pieceType - 1];
        gameObject.SetActive(true);
    }

    public void SetHasPieceMoved(bool _hasPieceMoved) { hasPieceMoved = _hasPieceMoved; }

    // Getters
    public int GetPieceColour() { return colour; }
    public PieceType GetPieceType() { return pieceType; }
    public bool GetHasPieceMoved() { return hasPieceMoved; }
}
