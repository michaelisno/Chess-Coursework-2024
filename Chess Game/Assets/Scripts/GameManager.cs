using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[,] tiles = new GameObject[8, 8];

    public GameObject tilePrefab;
    public GameObject chessBoard;
    
    private void Start()
    {
        // Initiate tiles and pieces
        CreateBoard();
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

        return tile;
    }
}
