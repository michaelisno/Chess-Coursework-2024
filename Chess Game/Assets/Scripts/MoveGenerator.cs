using System;
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

        return GenerateLegalMoves(pieceType, position, tiles, hasPieceMoved, piece);
    }

    private List<GameObject> GenerateLegalMoves(PieceManager.PieceType pieceType, Vector2 position, GameObject[,] tiles, bool hasPieceMoved, PieceManager piece)
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

            // up 2 squares
            newY = (int)position.y + 2;
            if (tiles[(int)position.x, newY].transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.none 
                && !hasPieceMoved)
            {
                legalMoves.Add(tiles[(int)position.x, newY]);
            }
        }

        // rook
        if (pieceType == PieceManager.PieceType.rook)
        {
            // up movement: Go from the y position to the top end of the board
            for (int newY = (int)position.y + 1; newY < 8; newY++) 
            {
                if (tiles[(int)position.x, newY].transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() != PieceManager.PieceType.none)
                    break;

                legalMoves.Add(tiles[(int)position.x, newY]);
            }
            // down movement
            for (int newY = (int)position.y - 1; newY >= 0; newY--)
            {
                if (tiles[(int)position.x, newY].transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() != PieceManager.PieceType.none)
                    break;

                legalMoves.Add(tiles[(int)position.x, newY]);
            }
            // right movement
            for (int newX = (int)position.x + 1; newX < 8; newX++)
            {
                if (tiles[newX, (int)position.y].transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() != PieceManager.PieceType.none)
                    break;

                legalMoves.Add(tiles[newX, (int)position.y]);
            }
            // left movement
            for (int newX = (int)position.x - 1; newX >= 0; newX--)
            {
                if (tiles[newX, (int)position.y].transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() != PieceManager.PieceType.none)
                    break;

                legalMoves.Add(tiles[newX, (int)position.y]);
            }
        }

        // bishop
        if (pieceType == PieceManager.PieceType.bishop)
        {
            
        }

        return legalMoves;
    }
}
