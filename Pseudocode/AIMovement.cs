private IEnumerator MovePiece(moveTo, moveFrom, informGameManager = True)
    randomTime = Random.Range(1, 3)

    yield return new WaitForSeconds(randomTime)

    newPiece = moveTo.GetChild().PieceManager()
    oldPiece = moveFrom.GetChild().PieceManager()

    if oldPiece.GetPieceType() == PieceType.king then
        if newPiece.GetPieceColour() == 0 then
            GameManager().blackKingPosition = newPiece.GetParent().TileManager().GetTilePosition()
        else
            GameManager().whiteKingPosition = newPiece.GetParent().TileManager().GetTilePosition()
        endif
    endif

    if moveTo.GetChild().PieceManager().GetPieceType() != PieceType.none then
        pieceToTake = moveTo.GetChild().Clone()
        pieceToTake.name = str(pieceToTake.PieceManager().GetPieceType())

        if GameManager().isPlayerWhite == False then
            GameManager().takenBlackPieces.Append(pieceToTake)
        else
            GameManager().takenWhitePieces.Add(pieceToTake)
        endif
    endif
 
    newPiece.SetHasPieceMoved(True)
    newPiece.SetPieceColour(oldPiece.GetPieceColour())
    newPiece.SetPieceType(oldPiece.GetPieceType())

    oldPiece.SetPieceType(PieceType.none)
    oldPiece.SetHasPieceMoved(False)

    if informGameManager == true then
        GameManager().PieceMoved()
    endif
endienumerator