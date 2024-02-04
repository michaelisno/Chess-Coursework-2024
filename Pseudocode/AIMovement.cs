class AIMovement
    public procedure Move()
        myColour = !GameManager.isPlayerWhite
        myTiles = []

        for tile = 0 to GameManager.tiles.length
            if tile.GetChild().PieceManager.GetPieceType() != PieceType.none 
                AND bool(tile.GetChild().PieceManager.GetPieceColour()) == myColour) then
                myTiles.Add(tile)
            endif
        next tile

        legalMoves = []
        do
            randomTileIndex = Random.Range(0, myTiles.length)
            selectedPiece = myTiles[randomTileIndex]
            legalMoves = MoveGenerator.GetMoves(selectedPiece, False, False)
        until legalMoves.length != 0

        selectedLegalMove = Random.Range(0, legalMoves.length)

        MovePiece(legalMoves[selectedLegalMove], selectedPiece)
    endprocedure

    private procedure MovePiece(moveTo, moveFrom)
        newPiece = moveTo.GetChild().PieceManager
        oldPiece = moveFrom.GetChild().PieceManager

        if moveTo.GetChild().PieceManager.GetPieceType() != PieceType.none then
            pieceToTake = moveTo.GetChild().Clone()
            pieceToTake.name = str(pieceToTake.PieceManager.GetPieceType())

            if !GameManager.isPlayerWhite then
                GameManager.takenBlackPieces.Add(pieceToTake)
            else
                GameManager.takenWhitePieces.Add(pieceToTake)
            endif
        endif

        newPiece.SetHasPieceMoved(True)
        newPiece.SetPieceColour(oldPiece.GetPieceColour())
        newPiece.SetPieceType(oldPiece.GetPieceType())

        oldPiece.SetPieceType(PieceManager.PieceType.none)
        oldPiece.SetHasPieceMoved(False)

        GameManager.PieceMoved()
    endprocedure
endclass
