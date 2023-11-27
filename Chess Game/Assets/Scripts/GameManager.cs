using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> tiles = new List<GameObject>();
    public List<Piece> whiteTakenPieces, blackTakenPieces = new List<Piece>();

    [SerializeField]
    private GameObject tilePrefab, board;
    [SerializeField]
    private GameObject whiteTimerText, blackTimerText;

    public bool isPlayerWhite = true;

    // 1 is white, 0 is black
    public int nextTurn = 1;
    // white, black timer in seconds (10 mins)
    public int whiteTime = 600, blackTime = 600;

    private int blackKingIndex = 39, whiteKingIndex = 32;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        // Initialise Board
        for (int xPosition = 0; xPosition < 8; xPosition++)
        {
            for (int yPosition = 0; yPosition < 8; yPosition++)
            {
                tiles.Add(CreateTile(xPosition, yPosition));
            }
        }

        // Start Timer
        StartCoroutine(timer());
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

    public void SwitchPlayer()
    {
        // test check
        if (nextTurn == 1 && !GetComponent<MoveGenerator>().testCheck(whiteKingIndex))
        {
            GameOver(0, 0);
        }
        if (nextTurn == 0 && !GetComponent<MoveGenerator>().testCheck(blackKingIndex)) 
        {
            GameOver(1, 0);
        }

        // flip nextTurn
        nextTurn = Convert.ToInt32(!Convert.ToBoolean(nextTurn));

        if (nextTurn != Convert.ToInt32(isPlayerWhite))
        {
            // Run an AI move when switched
            GetComponent<AIMovement>().MoveAI();
            Debug.Log("ai");
        }
    }

    private void GameOver(int playerWon, int reason)
    {
        if (reason == 0)
        {
            // Checkmate
            Debug.Log(playerWon + " won through checkmate.");
        }

        ResetGame();
    }

    private void ResetGame() 
    {
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }

        StopCoroutine(timer());

        whiteTakenPieces.Clear();
        blackTakenPieces.Clear();

        whiteTime = 600;
        blackTime = 600;

        SwitchPlayer();

        blackKingIndex = 39;
        whiteKingIndex = 32;

        StartGame();
    }

    public void SetKingIndex(int newIndex, int whichKing)
    {
        if (whichKing == 0) blackKingIndex = newIndex;
        if (whichKing == 1) whiteKingIndex = newIndex;
    }

    IEnumerator timer()
    {
        while (true)
        {
            if (nextTurn == 1)
            {
                whiteTime -= 1;
            }
            else
            {
                blackTime -= 1;
            }

            whiteTimerText.GetComponent<TextMeshProUGUI>().text = "White Time: " + whiteTime;
            blackTimerText.GetComponent<TextMeshProUGUI>().text = "Black Time: " + blackTime;

            yield return new WaitForSeconds(1);
        }
    }
}
