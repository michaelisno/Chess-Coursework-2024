using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[,] tiles = new GameObject[8, 8];

    public List<GameObject> takenWhitePieces, takenBlackPieces;

    public GameObject tilePrefab;
    public GameObject chessBoard;

    public bool isPlayerWhite = true;
    public bool isWhiteMoving = true;

    public TextMeshProUGUI whiteTimerText;
    public TextMeshProUGUI blackTimerText;

    public GameObject winScreenPanel;
    public GameObject lossScreenPanel;

    public Vector2 whiteKingPosition, blackKingPosition;

    private void Start()
    {
        isWhiteMoving = true;
        whiteKingPosition = new Vector2(4, 0);
        blackKingPosition = new Vector2(4, 7);

        // Initiate tiles and pieces
        CreateBoard();
        StartCoroutine(Timer());

        if (!isPlayerWhite)
            GetComponent<AIMovement>().Move();
    }

    IEnumerator Timer()
    {
        while (true)
        {
            if (isWhiteMoving)
                whiteTimerText.SetText("White Time: " + Convert.ToString(Convert.ToInt32(whiteTimerText.text.
                    Replace("White Time: ", "").Replace("s", "")) - 1) + "s");

            if (!isWhiteMoving)
                blackTimerText.SetText("Black Time: " + Convert.ToString(Convert.ToInt32(blackTimerText.text.
                    Replace("Black Time: ", "").Replace("s", "")) - 1) + "s");

            if (Convert.ToInt32(whiteTimerText.text.Replace("White Time: ", "").Replace("s", "")) <= 0)
                EndGame(1);
            if (Convert.ToInt32(blackTimerText.text.Replace("Black Time: ", "").Replace("s", "")) <= 0)
                EndGame(0);

            yield return new WaitForSeconds(1);
        }
    }

    private void EndGame(int winner)
    {
        StopAllCoroutines();

        if (winner == 1)
        {
            Debug.Log("black has won");
            if (isPlayerWhite)
                lossScreenPanel.gameObject.SetActive(true);
            else
                winScreenPanel.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("white has won");
            if (isPlayerWhite)
                winScreenPanel.gameObject.SetActive(true);
            else
                lossScreenPanel.gameObject.SetActive(true);
        }
    }

    public void PieceMoved()
    {
        isWhiteMoving = !isWhiteMoving;

        GameObject checker = DetectCheck(Convert.ToInt32(isWhiteMoving));

        if (checker == null)
        {
            if (isWhiteMoving != isPlayerWhite)
            {
                // enemy is moving, run AI move
                GetComponent<AIMovement>().Move();
            }
        }
        else
        {
            if (isWhiteMoving == false)
            {
                // black king checked
                Debug.Log("GAME STOPPED: BLACK KING CHECKED");
                if (isPlayerWhite)
                {
                    Debug.Log("player is white");
                    // player is white, ie, black is AI
                    GetComponent<AIMovement>().RunDecheckMove(tiles[(int)blackKingPosition.x, (int)blackKingPosition.y], false, checker);
                }
            }
            else 
            {
                // white king checked
                Debug.Log("GAME STOPPED: WHITE KING CHECKED");
            }
        }

        checker = null;
    }

    private GameObject DetectCheck(int whichPlayer)
    {
        // whichPlayer -> 1 is white, 0 is black
        if (whichPlayer == 0)
        {
            // Get List of all enemy pieces' legal moves
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.GetChild(0).gameObject.activeInHierarchy &&
                    Convert.ToBoolean(tile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour()) == true)
                {
                    List<GameObject> tempLegalMoves = GetComponent<MoveGenerator>().GetMoves(tile, false, true);
                    if (tempLegalMoves.Contains(tiles[(int)blackKingPosition.x, (int)blackKingPosition.y]))
                    {
                        return tile;
                    }
                }
            }
        }

        if (whichPlayer == 1)
        {
            // Get List of all enemy pieces' legal moves
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.GetChild(0).gameObject.activeInHierarchy &&
                    Convert.ToBoolean(tile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceColour()) == false)
                {
                    List<GameObject> tempLegalMoves = GetComponent<MoveGenerator>().GetMoves(tile, false, false);
                    if (tempLegalMoves.Contains(tiles[(int)whiteKingPosition.x, (int)whiteKingPosition.y]))
                    {
                        return tile;
                    }
                }
            }
        }

        return null;
    }

    private void CreateBoard()
    {
        // Loop from 1 to 8 for x and y coordinates
        for (int x = 0; x < 8; x++)
        { 
            for (int y = 0; y < 8; y++)
            {
                // Create tile object and sign it to position in 2D array
                GameObject newTile = CreateTile(x, y);
                tiles[x, y] = newTile;
            }
        }
    }

    private GameObject CreateTile(int x, int y)
    {
        // Instantiate tile object at x,y
        GameObject tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, chessBoard.transform);
        
        tile.name = x.ToString() + ":" + y.ToString();

        tile.GetComponent<TileManager>().SetTilePosition(x, y);
        tile.GetComponent<TileManager>().SetTileColour((x + y) % 2);
        tile.GetComponent<TileManager>().InitializePiece();

        return tile;
    }
}
