using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> tiles = new List<GameObject>();

    public List<Piece> whiteTakenPieces = new List<Piece>();
    public List<Piece> blackTakenPieces = new List<Piece>();

    [SerializeField]
    private GameObject tilePrefab, board;

    public bool isPlayerWhite = true;

    private void Start()
    {
        // Initialise Board
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                tiles.Add(CreateTile(x, y));
            }
        }
    }

    private GameObject CreateTile(int x, int y)
    {
        // Create tile object
        GameObject newTile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, board.transform);

        // Set tile attributes
        newTile.name = (8 * x + y).ToString();
        newTile.GetComponent<Tile>().position = new Vector2(x, y);
        newTile.GetComponent<Tile>().SetColour((x + y) % 2);
        newTile.GetComponent<Tile>().InitatePieces();

        return newTile;
    }
}
