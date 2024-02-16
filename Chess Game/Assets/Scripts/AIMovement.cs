using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
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

    private void MovePiece(GameObject moveTo, GameObject moveFrom, bool informGameManager = true)
    {
        PieceManager newPiece = moveTo.transform.GetChild(0).GetComponent<PieceManager>();
        PieceManager oldPiece = moveFrom.transform.GetChild(0).GetComponent<PieceManager>();

        if (oldPiece.GetPieceType() == PieceManager.PieceType.king)
        {
            if (newPiece.GetPieceColour() == 0)
            {
                GetComponent<GameManager>().blackKingPosition =
                    newPiece.GetComponentInParent<TileManager>().GetTilePosition();
            }
            else
            {
                GetComponent<GameManager>().whiteKingPosition =
                    newPiece.GetComponentInParent<TileManager>().GetTilePosition();
            }
        }

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

        if (informGameManager)
            GetComponent<GameManager>().PieceMoved();
    }

    public void RunDecheckMove(GameObject checkedKingTile, bool isKingWhite, GameObject checkerTile)
    {
        Debug.Log("Running Decheck Move Now");
        // TODO:
        // 1: Generate list of kings legal moves
        List<GameObject> kingsLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkedKingTile, false, isKingWhite);
        // 4: If any kings legal moves remain, choose a random one and move to it
        foreach (GameObject kLM in kingsLegalMoves)
        {
            Debug.Log("King Legal Move AT: " + kLM.name);
        }
        if (kingsLegalMoves.Count > 0)
        {
            Debug.Log("kings legal moves > 0");
            int randomIndex = UnityEngine.Random.Range(0, kingsLegalMoves.Count);
            MovePiece(kingsLegalMoves[randomIndex], checkedKingTile, false);
            GetComponent<GameManager>().PieceMoved();
        }
        else
        {
            Debug.Log("kings legal moves == 0");
            // 5: If no kings legal moves remain, generate list of friendly legal moves excluding king
            List<GameObject> friendlyTiles = new List<GameObject>();

            foreach (GameObject tile in GetComponent<GameManager>().tiles)
            {
                if (Convert.ToBoolean(tile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour()) 
                    == isKingWhite && tile.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    Debug.Log("Added " + tile.name + " to friendlyTiles");
                    friendlyTiles.Add(tile);
                }
            }

            List<List<GameObject>> friendlyLegalMoves = new List<List<GameObject>>();
            List<GameObject> relaventFriendlyTile = new List<GameObject>();

            foreach (GameObject friendly in friendlyTiles)
            {
                List<GameObject> legalMoves = new List<GameObject>();

                if (friendly.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() != PieceManager.PieceType.king)
                {
                    legalMoves = GetComponent<MoveGenerator>().GetMoves(friendly, false, isKingWhite);
                }

                friendlyLegalMoves.Add(legalMoves);
                relaventFriendlyTile.Add(friendly);

                Debug.Log("Added legal moves from " + friendly.name + "to friendlyLegalMoves");
            }
            // 6: Generate list of enemy checkers legal moves IN DIRECTION TOWARDS FRIENDLY KING, add checkers position to list

            List<GameObject> checkerLegalMoves = new List<GameObject>();

            // If checker is rook or queen
            if (checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.rook
                || checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.queen)
            {
                Debug.Log("CHECKING PIECE IS A ROOK! or Queen");
                int xDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().x 
                    - (int)checkerTile.GetComponent<TileManager>().GetTilePosition().x;
                int yDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().y
                    - (int)checkerTile.GetComponent<TileManager>().GetTilePosition().y;

                if (xDiff == 0)
                {
                    if (yDiff >= 0)
                    {
                        // king is below rook
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 1);
                    }
                    if (yDiff < 0)
                    {
                        // king is above rook
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 0);
                    }
                }
                else
                {
                    if (xDiff >= 0)
                    {
                        // king is left of rook
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 3);
                    }
                    if (yDiff < 0)
                    {
                        // king is right of rook
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 2);
                    }
                }
            }

            // If checker is bishop or queen
            if (checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.bishop
                || checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.queen)
            {
                Debug.Log("CHECKING PIECE IS A BISHOP! or Queen");
                int xDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().x
                    - (int)checkerTile.GetComponent<TileManager>().GetTilePosition().x;
                int yDiff = (int)checkedKingTile.GetComponent<TileManager>().GetTilePosition().y
                    - (int)checkerTile.GetComponent<TileManager>().GetTilePosition().y;

                if (xDiff < 0 && yDiff < 0)
                {
                    // king is up, left from bishop
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 0);
                }
                else if (xDiff < 0 && yDiff > 0)
                {
                    // king is down, left from bishop
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 1);
                }
                else if (xDiff > 0 && yDiff < 0)
                {
                    // king is up, right from bishop
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 2);
                }
                else
                {
                    // king is down, right from bishop
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 3);
                }
            }

            // if checker is knight or pawn
            if (checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.knight
                || checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.pawn)
            {
                checkerLegalMoves.Add(checkerTile);
            }

                checkerLegalMoves.Add(GetComponent<GameManager>().tiles[(int)checkerTile.GetComponent<TileManager>().GetTilePosition().x,
                (int)checkerTile.GetComponent<TileManager>().GetTilePosition().y]);

            foreach (GameObject item in checkerLegalMoves)
            {
                Debug.Log("Checker Legal Move: " + item.name);
            }

            // 7: Foreach friendly legal move, add to new legal list, 'finalLegalMoves', if legal move in enemy checkers legal move list
            List<List<GameObject>> finalLegalMoves = new List<List<GameObject>>();
            List<GameObject> relaventFinalMoves = new List<GameObject>();

            foreach (GameObject tile in relaventFriendlyTile)
            {
                List<GameObject> myLegalMoves = friendlyLegalMoves[relaventFriendlyTile.IndexOf(tile)];
                List<GameObject> tempFinalMoves = new List<GameObject>();
                foreach (GameObject legalMove in myLegalMoves)
                {
                    if (checkerLegalMoves.Contains(legalMove))
                    { 
                        tempFinalMoves.Add(legalMove);
                        Debug.Log("Added " + legalMove.name + " to tempFinalMoves as is in checkersLegalMoves");
                    }
                }
                if (tempFinalMoves.Count > 0)
                {
                    Debug.Log("temp final moves > 0");
                    finalLegalMoves.Add(tempFinalMoves);
                    relaventFinalMoves.Add(tile);
                }
            }
            // 8: if 'finalLegalMoves' contains items, randomly choose one and complete move to block check or take checker.
            if (finalLegalMoves.Count > 0)
            {
                Debug.Log("final Legal moves > 0");
                int pieceIndex = UnityEngine.Random.Range(0, finalLegalMoves.Count);
                int moveIndex = UnityEngine.Random.Range(0, finalLegalMoves[pieceIndex].Count);

                Debug.Log("piece: " + finalLegalMoves[pieceIndex]);
                Debug.Log("move: " + finalLegalMoves[pieceIndex][moveIndex]);

                MovePiece(finalLegalMoves[pieceIndex][moveIndex], relaventFinalMoves[pieceIndex], false);
                GetComponent<GameManager>().PieceMoved();
            }
            else
            {
                // 9: If no moves in 'finalLegalMoves', friendly king is check mated!
                Debug.Log("KING IN CHECKMATE!");
                GetComponent<GameManager>().EndGame(0);
            }
        }
    }
}
