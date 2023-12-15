using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public void MoveAI()
    {
        StartCoroutine(wait(UnityEngine.Random.Range(1, 4)));
    }

    IEnumerator wait(int time)
    {
        yield return new WaitForSeconds(time);
        Move();
    }

    private void Move()
    {
        List<GameObject> pieces = GetComponent<GameManager>().tiles;
        List<GameObject> myPieces = new List<GameObject>();

        foreach (GameObject piece in pieces) 
        {
            if (piece.transform.GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none 
                && piece.transform.GetChild(0).GetComponent<Piece>().GetColour() != Convert.ToInt32(
                    GetComponent<GameManager>().isPlayerWhite))
            { 
                myPieces.Add(piece);
            }
        }

        List<GameObject> legalMoves = new List<GameObject>();

        int selectedPiece = 0;
        bool foundMoves = false;

        while (foundMoves == false)
        {
            // most basic move type; select a random piece and do a random movement
            selectedPiece = UnityEngine.Random.Range(0, myPieces.Count);
            legalMoves = GetComponent<MoveGenerator>().GenerateMoves(!GetComponent<GameManager>().
                isPlayerWhite, myPieces[selectedPiece].GetComponent<Tile>().GetPosition(),
                myPieces[selectedPiece].transform.GetChild(0).GetComponent<Piece>().GetType(),
                myPieces[selectedPiece].transform.GetChild(0).GetComponent<Piece>().hasPieceMoved, true);

            if (legalMoves.Count > 0) foundMoves = true;
        }

        int randomMove = UnityEngine.Random.Range(0, legalMoves.Count);
        GameObject myMove = legalMoves[randomMove];
        GameObject selectedTile = GetComponent<GameManager>().tiles[GetComponent<GameManager>().tiles.IndexOf(myPieces[selectedPiece])];

        // Move
        if (myMove.transform.GetChild(0).GetComponent<Piece>().GetType() != Piece.PieceType.none)
        {
            Piece takenPiece = new Piece();

            // add to taken pieces
            if (myMove.transform.GetChild(0).GetComponent<Piece>().GetColour() == 1)
            {
                takenPiece = myMove.transform.GetChild(0).GetComponent<Piece>();
                GetComponent<GameManager>().whiteTakenPieces.Add(takenPiece);
            }
            if (myMove.transform.GetChild(0).GetComponent<Piece>().GetColour() == 0)
            {
                takenPiece = myMove.transform.GetChild(0).GetComponent<Piece>();
                GetComponent<GameManager>().blackTakenPieces.Add(takenPiece);
            }
        }

        myMove.transform.GetChild(0).GetComponent<Piece>().SetType(selectedTile.transform.GetChild(0).GetComponent<Piece>().GetType());
        myMove.transform.GetChild(0).GetComponent<Piece>().SetColour(Convert.ToInt32(!GetComponent<GameManager>().isPlayerWhite));
        myMove.transform.GetChild(0).GetComponent<Piece>().hasPieceMoved = true;
        selectedTile.transform.GetChild(0).GetComponent<Piece>().SetType(Piece.PieceType.none);
        selectedTile.GetComponent<Tile>().SetColour(Convert.ToInt32(selectedTile.GetComponent<Tile>().GetPosition().x
            + selectedTile.GetComponent<Tile>().GetPosition().y) % 2);

        // switch player
        GetComponent<GameManager>().SwitchPlayer();
    }
}
