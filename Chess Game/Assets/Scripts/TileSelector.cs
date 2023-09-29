using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public GameObject selectedTile = null;

    private Ray ray;
    private RaycastHit hit;

    private List<GameObject> highlightedTiles = new List<GameObject>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "tile") SelectTile();
            } 
        }
    }

    private void SelectTile()
    {
        GameObject tile = hit.collider.gameObject;

        if (selectedTile == null)
        {
            if (tile.transform.GetChild(0).gameObject.GetComponent<Piece>().type != Piece.PieceType.none
                && tile.transform.GetChild(0).gameObject.GetComponent<Piece>().colour != 0)
            {
                selectedTile = tile;
                tile.GetComponent<Tile>().SetColour(3);

                // generate legal moves
                HighlightLegalTiles(GetComponent<MovesGenerator>().GenerateMoves(GetComponent<GameManager>().isPlayerWhite, 
                    selectedTile.GetComponent<Tile>().position, selectedTile.transform.GetChild(0).GetComponent<Piece>().type));
            }
        }
        else
        {
            if (selectedTile == tile)
            {
                selectedTile.GetComponent<Tile>().SetColour(Convert.ToInt32(selectedTile.GetComponent<Tile>().position.x + selectedTile.GetComponent<Tile>().position.y)%2);
                selectedTile = null;
                HighlightLegalTiles(new List<GameObject>());
            }
            else
            {
                if (tile.transform.GetChild(0).gameObject.GetComponent<Piece>().type != Piece.PieceType.none
                    && tile.transform.GetChild(0).gameObject.GetComponent<Piece>().colour != 0)
                {
                    selectedTile.GetComponent<Tile>().SetColour(Convert.ToInt32(selectedTile.GetComponent<Tile>().position.x + selectedTile.GetComponent<Tile>().position.y) % 2);
                    selectedTile = tile;
                    tile.GetComponent<Tile>().SetColour(3);

                    // generate legal moves
                    HighlightLegalTiles(GetComponent<MovesGenerator>().GenerateMoves(GetComponent<GameManager>().isPlayerWhite,
                        selectedTile.GetComponent<Tile>().position, selectedTile.transform.GetChild(0).GetComponent<Piece>().type));
                }
            }
        }
    }

    private void HighlightLegalTiles(List<GameObject> legalTiles)
    { 
        // highlight legal tiles from legalTiles array
        // reset all highlighted tiles
        foreach (GameObject tile in highlightedTiles) 
        {
            tile.GetComponent<Tile>().SetColour(Convert.ToInt32(tile.GetComponent<Tile>().position.x
                + tile.GetComponent<Tile>().position.y) % 2);
        }

        // highlight new tiles
        foreach (GameObject tile in legalTiles)
        {
            tile.GetComponent<Tile>().SetColour(2);
            highlightedTiles.Add(tile);
        }
    }
}
