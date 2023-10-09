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
        for (int xPos = 0; xPos < 8; xPos++)
        {
            for (int yPos = 0; yPos < 8; yPos++)
            {
                tiles.Add(CreateTile(xPos, yPos));
            }
        }
    }

    private GameObject CreateTile(int xPos, int yPos)
    {
        // Create tile object
        GameObject newTile = Instantiate(tilePrefab, new Vector3(xPos, 0, yPos), Quaternion.identity, board.transform);

        // Set tile attributes
        newTile.name = (8 * xPos + yPos).ToString();
        newTile.GetComponent<Tile>().SetColour((xPos + yPos) % 2);
        newTile.GetComponent<Tile>().SetPosition(new Vector2(xPos, yPos));
        newTile.GetComponent<Tile>().InitatePieces();

        return newTile;
    }
}
