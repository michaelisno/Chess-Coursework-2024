class GameManager:
    public tiles = GameObject[8, 8]
    public takenWhitePieces, takenBlackPieces
    public tilePrefab;
    public chessBoard
    public isPlayerWhite = True;
    public isWhiteMoving = True;
    public isPlayerChecked = False;
    public whiteTimerText;
    public blackTimerText;
    public winScreenPanel;
    public lossScreenPanel;
    public checkerTile
    public whiteKingPosition, blackKingPosition;

    private procedure Start()
        isWhiteMoving = True
        whiteKingPosition = (4, 0)
        blackKingPosition = (4, 7)

        // Initiate tiles and pieces
        CreateBoard()
        StartCoroutine(Timer())

        if isPlayerWhite == false then
            AIMovement.Move()
        endif
    endprocedure

    IEnumerator Timer()
        while true
            if isWhiteMoving == True then
                whiteTimerText.SetText("White Time: " + str(int(whiteTimerText.text.
                    Replace("White Time: ", "").Replace("s", "")) - 1) + "s")
            endif

            if isWhiteMoving == False then
                blackTimerText.SetText("Black Time: " + str(int(blackTimerText.text.
                    Replace("Black Time: ", "").Replace("s", "")) - 1) + "s")
            endif

            if Convert.ToInt32(whiteTimerText.text.Replace("White Time: ", "").Replace("s", "")) <= 0 then
                EndGame(1)
            endif
            if Convert.ToInt32(blackTimerText.text.Replace("Black Time: ", "").Replace("s", "")) <= 0 then
                EndGame(0)
            endif

            yield return new WaitForSeconds(1)
        endwhile
    endienumerator

    public procedure EndGame(winner)
        StopAllCoroutines()

        if winner == 1 then
            if isPlayerWhite == True then
                lossScreenPanel.gameObject.SetActive(True)
            else
                winScreenPanel.gameObject.SetActive(True)
            endif
        else
            if isPlayerWhite == True then
                winScreenPanel.gameObject.SetActive(True)
            else
                lossScreenPanel.gameObject.SetActive(True)
            endif
        endif
    endprocedure

    public procedure PieceMoved()
        isWhiteMoving = !isWhiteMoving

        checker = DetectCheck(int(isWhiteMoving))

        if checker == null then
            if isWhiteMoving != isPlayerWhite then
                AIMovement().Move()
            endif
        else
            if isWhiteMoving == False then
                if isPlayerWhite == True then
                    AIMovement().RunDecheckMove(tiles[blackKingPosition.x, blackKingPosition.y], false, checker)
                endif
            else
                isPlayerChecked = true
                checkerTile = checker
            endif
        endif
    endprocedure

    private function DetectCheck(whichPlayer)
        if whichPlayer == 0 then
            for tile = 0 to tiles.length
                if tile.GetChild().activeInHierarchy AND bool(tile.GetChild().PieceManager().GetPieceColour()) == True then
                    List<GameObject> tempLegalMoves = MoveGenerator().GetMoves(tile, false, true)
                    if tempLegalMoves.Contains(tiles[(int)blackKingPosition.x, (int)blackKingPosition.y]) then
                        return tile
                    endif
                endif
            next tile
        endif

        if whichPlayer == 1 then
            for tile = 0 to tiles
                if (tile.GetChild().activeInHierarchy AND bool(tile.GetChild().PieceManager().GetPieceColour()) == False then
                    tempLegalMoves = MoveGenerator().GetMoves(tile, false, false)
                    if tempLegalMoves.Contains(tiles[(int)whiteKingPosition.x, (int)whiteKingPosition.y]) then
                        return tile
                    endif
                endif
            next tile
        endif

        return null
    endfunction
endclass
