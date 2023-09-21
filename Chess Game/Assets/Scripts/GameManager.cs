using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] tiles;

    public GameObject tilePrefab;
    public GameObject board;

    void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    { 
        for (int x = 0; x < 8; x++) 
        {
            for (int y = 0; y < 8; y++)
            {
                tiles.Append(CreateTile(x,y));
            }
        }
    }

    private GameObject CreateTile(int x, int y)
    {
        GameObject newTile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, board.transform);
        newTile.name = ((x*8)+y).ToString();
        return newTile;
    }
}
