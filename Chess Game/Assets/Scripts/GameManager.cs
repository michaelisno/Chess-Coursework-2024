using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] tiles;

    public GameObject tilePrefab;
    public GameObject board;

    private void Start()
    {
        InitiateBoard();
    }

    private void InitiateBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                tiles.Append(CreateTile(x, y));
            }
        }
    }

    private GameObject CreateTile(int x, int y)
    {
        GameObject newTile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, board.transform);

        // set tile attributes
        newTile.name = ((8 * x) + y).ToString();
        newTile.GetComponent<Tile>().position = new Vector2(x, y);
        newTile.GetComponent<Tile>().SetColour((x + y) % 2);
        newTile.GetComponent<Tile>().InitatePieces();

        return newTile;
    }
}
