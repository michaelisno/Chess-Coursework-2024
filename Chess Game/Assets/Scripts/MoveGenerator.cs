using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveGenerator : MonoBehaviour
{
    public List<GameObject> GetMoves(GameObject selectedTile)
    {
        // References to tile and piece
        TileManager tile = selectedTile.GetComponent<TileManager>();
        PieceManager piece = selectedTile.transform.GetChild(0).GetComponent<PieceManager>();
        GameManager gameManager = GetComponent<GameManager>();

        // Get information from tile and piece
        PieceManager.PieceType pieceType = piece.GetPieceType();
        Vector2 position = tile.GetTilePosition();
        bool hasPieceMoved = piece.GetHasPieceMoved();
        GameObject[,] tiles = gameManager.tiles;

        return GenerateLegalMoves(pieceType, position, tiles, hasPieceMoved);
    }

    private List<GameObject> GenerateLegalMoves(PieceManager.PieceType pieceType, Vector2 position, GameObject[,] tiles, bool hasPieceMoved)
    {
        List<GameObject> legalMoves = new List<GameObject>();

        // pawn
        if (pieceType == PieceManager.PieceType.pawn)
        {
            // up 1 square
            int newY = (int)position.y + 1;
            if (tiles[(int)position.x, newY].transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.none)
            {
                legalMoves.Add(tiles[(int)position.x, newY]);
            }
        }

        return legalMoves;
    }
}
