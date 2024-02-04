using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public void Move()
    {
        bool myColour = !GetComponent<GameManager>().isPlayerWhite;
        List<GameObject> myTiles = new List<GameObject>();

        GameObject selectedPiece = new GameObject();

        foreach (GameObject tile in GetComponent<GameManager>().tiles)
        {
            if (tile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() != PieceManager.PieceType.none
                && Convert.ToBoolean(tile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour()) == myColour)
            { 
                myTiles.Add(tile);
            }
        }

        List<GameObject> legalMoves = new List<GameObject>();
        while (legalMoves.Count == 0)
        {
            int randomTileIndex = UnityEngine.Random.Range(0, myTiles.Count);
            selectedPiece = myTiles[randomTileIndex];
            legalMoves = GetComponent<MoveGenerator>().GetMoves(selectedPiece, false, false);
        }

        int selectedLegalMove = UnityEngine.Random.Range(0, legalMoves.Count);

        MovePiece(legalMoves[selectedLegalMove], selectedPiece);
    }

    private void MovePiece(GameObject moveTo, GameObject moveFrom)
    {
        PieceManager newPiece = moveTo.transform.GetChild(0).GetComponent<PieceManager>();
        PieceManager oldPiece = moveFrom.transform.GetChild(0).GetComponent<PieceManager>();

        // case two: clicked tile not selected tile and clicked tile in legal moves, select new tile
        if (moveTo.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() != PieceManager.PieceType.none)
        {
            GameObject pieceToTake = moveTo.transform.GetChild(0).gameObject.CloneViaFakeSerialization();
            pieceToTake.name = pieceToTake.GetComponent<PieceManager>().GetPieceType().ToString();

            // take piece
            if (!GetComponent<GameManager>().isPlayerWhite)
                GetComponent<GameManager>().takenBlackPieces.Add(pieceToTake);
            else
                GetComponent<GameManager>().takenWhitePieces.Add(pieceToTake);
        }

        // Set new piece info to old piece info   
        newPiece.SetHasPieceMoved(true);
        newPiece.SetPieceColour(oldPiece.GetPieceColour());
        newPiece.SetPieceType(oldPiece.GetPieceType());

        // Set old piece info to no piece
        oldPiece.SetPieceType(PieceManager.PieceType.none);
        oldPiece.SetHasPieceMoved(false);

        // remove all highlighted legal tiles
        GetComponent<GameManager>().PieceMoved();
    }
}
