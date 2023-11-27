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
        if (Input.GetMouseButtonDown(0) && GetComponent<GameManager>().nextTurn == Convert.ToInt32(GetComponent<GameManager>().isPlayerWhite))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "tile")
                {
                    SelectTile();
                }
            } 
        }
    }

    private void SelectTile()
    {
        GameObject tile = hit.collider.gameObject;

        if (highlightedTiles.Contains(tile))
        {
            if (tile.transform.GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none)
            {
                Piece takenPiece = new Piece();

                // add to taken pieces
                if (tile.transform.GetChild(0).GetComponent<Piece>().GetColour() == 1)
                {
                    takenPiece = tile.transform.GetChild(0).GetComponent<Piece>();
                    GetComponent<GameManager>().whiteTakenPieces.Add(takenPiece);
                }
                if (tile.transform.GetChild(0).GetComponent<Piece>().GetColour() == 0)
                {
                    takenPiece = tile.transform.GetChild(0).GetComponent<Piece>();
                    GetComponent<GameManager>().blackTakenPieces.Add(takenPiece);
                }
            }

            // move piece
            tile.transform.GetChild(0).GetComponent<Piece>().SetType(selectedTile.transform.GetChild(0).GetComponent<Piece>().GetType());
            tile.transform.GetChild(0).GetComponent<Piece>().SetColour(Convert.ToInt32(GetComponent<GameManager>().isPlayerWhite));
            tile.transform.GetChild(0).GetComponent<Piece>().hasPieceMoved = true;
            selectedTile.transform.GetChild(0).GetComponent<Piece>().SetType(Piece.PieceType.none);
            selectedTile.GetComponent<Tile>().SetColour(Convert.ToInt32(selectedTile.GetComponent<Tile>().GetPosition().x
                + selectedTile.GetComponent<Tile>().GetPosition().y) % 2);
            selectedTile = null;

            // switch player
            GetComponent<GameManager>().SwitchPlayer();
        
            HighlightLegalTiles(new List<GameObject>());
        }
        else
        {
            // select piece
            if (selectedTile == null)
            {
                if (tile.transform.GetChild(0).gameObject.GetComponent<Piece>().GetType() != Piece.PieceType.none
                    && tile.transform.GetChild(0).gameObject.GetComponent<Piece>().GetColour() == Convert.ToInt32(GetComponent<GameManager>().
                    isPlayerWhite))
                {
                    selectedTile = tile;
                    tile.GetComponent<Tile>().SetColour(3);

                    // generate legal moves
                    HighlightLegalTiles(GetComponent<MoveGenerator>().GenerateMoves(GetComponent<GameManager>().isPlayerWhite,
                        selectedTile.GetComponent<Tile>().GetPosition(), selectedTile.transform.GetChild(0).GetComponent<Piece>().GetType(), 
                        selectedTile.transform.GetChild(0).GetComponent<Piece>().hasPieceMoved));
                }
            }
            else
            {
                if (selectedTile == tile)
                {
                    selectedTile.GetComponent<Tile>().SetColour(Convert.ToInt32(selectedTile.GetComponent<Tile>().GetPosition().x + selectedTile.
                        GetComponent<Tile>().GetPosition().y) % 2);
                    selectedTile = null;
                    HighlightLegalTiles(new List<GameObject>());
                }
                else
                {
                    if (tile.transform.GetChild(0).gameObject.GetComponent<Piece>().GetType() != Piece.PieceType.none
                        && tile.transform.GetChild(0).gameObject.GetComponent<Piece>().GetColour() == Convert.ToInt32(GetComponent<GameManager>().
                        isPlayerWhite))
                    {
                        selectedTile.GetComponent<Tile>().SetColour(Convert.ToInt32(selectedTile.GetComponent<Tile>().GetPosition().x + selectedTile.
                            GetComponent<Tile>().GetPosition().y) % 2);
                        selectedTile = tile;
                        tile.GetComponent<Tile>().SetColour(3);

                        // generate legal moves
                        HighlightLegalTiles(GetComponent<MoveGenerator>().GenerateMoves(GetComponent<GameManager>().isPlayerWhite,
                            selectedTile.GetComponent<Tile>().GetPosition(), selectedTile.transform.GetChild(0).GetComponent<Piece>().GetType(), 
                            selectedTile.transform.GetChild(0).GetComponent<Piece>().hasPieceMoved));
                    }
                }
            }
        }
    }

    private void HighlightLegalTiles(List<GameObject> legalTiles)
    {
        // Reset all highlighted tiles
        foreach (GameObject tile in highlightedTiles) 
        {
            tile.GetComponent<Tile>().SetColour(Convert.ToInt32(tile.GetComponent<Tile>().GetPosition().x
                + tile.GetComponent<Tile>().GetPosition().y) % 2);
        }

        highlightedTiles.Clear();

        // highlight new tiles
        foreach (GameObject tile in legalTiles)
        {
            tile.GetComponent<Tile>().SetColour(2);
            highlightedTiles.Add(tile);
        }
    }
}
