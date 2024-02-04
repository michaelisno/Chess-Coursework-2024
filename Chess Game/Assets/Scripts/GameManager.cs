using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[,] tiles = new GameObject[8, 8];

    public List<GameObject> takenWhitePieces, takenBlackPieces;

    public GameObject tilePrefab;
    public GameObject chessBoard;

    public bool isPlayerWhite = true;
    public bool isWhiteMoving = true;

    private void Start()
    {
        // Initiate tiles and pieces
        CreateBoard();
    }

    public void PieceMoved()
    {
        isWhiteMoving = !isWhiteMoving;

        if (isWhiteMoving != isPlayerWhite)
        {
            // enemy is moving, run AI move
            GetComponent<AIMovement>().Move();
        }
    }

    private void CreateBoard()
    {
        // Loop from 1 to 8 for x and y coordinates
        for (int x = 0; x < 8; x++)
        { 
            for (int y = 0; y < 8; y++)
            {
                // Create tile object and sign it to position in 2D array
                GameObject newTile = CreateTile(x, y);
                tiles[x, y] = newTile;
            }
        }
    }

    private GameObject CreateTile(int x, int y)
    {
        // Instantiate tile object at x,y
        GameObject tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, chessBoard.transform);
        
        tile.name = x.ToString() + ":" + y.ToString();

        tile.GetComponent<TileManager>().SetTilePosition(x, y);
        tile.GetComponent<TileManager>().SetTileColour((x + y) % 2);
        tile.GetComponent<TileManager>().InitializePiece();

        return tile;
    }
}
