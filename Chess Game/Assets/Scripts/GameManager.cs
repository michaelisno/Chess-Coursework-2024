using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> tiles = new List<GameObject>();
    public List<Piece> whiteTakenPieces, blackTakenPieces = new List<Piece>();

    [SerializeField]
    private GameObject tilePrefab, board;

    public bool isPlayerWhite = true;

    private void Start()
    {
        // Initialise Board
        for (int xPosition = 0; xPosition < 8; xPosition++)
        {
            for (int yPosition = 0; yPosition < 8; yPosition++)
            {
                tiles.Add(CreateTile(xPosition, yPosition));
            }
        }
    }

    private GameObject CreateTile(int xPosition, int yPosition)
    {
        // Create tile object
        GameObject newTile = Instantiate(tilePrefab, new Vector3(xPosition, 0, yPosition), Quaternion.identity, board.transform);

        // Set tile attributes
        newTile.name = (8 * xPosition + yPosition).ToString();
        newTile.GetComponent<Tile>().SetColour((xPosition + yPosition) % 2);
        newTile.GetComponent<Tile>().SetPosition(new Vector2(xPosition, yPosition));
        newTile.GetComponent<Tile>().InitatePieces();

        return newTile;
    }
}
