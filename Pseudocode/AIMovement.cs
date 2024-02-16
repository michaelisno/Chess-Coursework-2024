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

    public procedure RunDecheckMove(checkedKingTile, isKingWhite, checkerTile)
        kingsLegalMoves[] = MoveGenerator.GetMoves(checkedKingTile, false, isKingWhite)
        if kingsLegalMoves.Count > 0 then
            MovePiece(kingsLegalMoves[Random.Range(0, kingsLegalMoves.Count)], checkedKingTile, false)
            GameManager.PieceMoved()
        else
            for tile = 0 to GameManager.tiles.length
                if bool(tile.GetChild().PieceManager.GetPieceColour()) == isKingWhite AND tile.GetChild().gameObject.active then
                    friendlyTiles.Add(tile)
                endif
            next tile

            friendlyLegalMoves[,] = new GameObject[,]
            relaventFriendlyTile[] = new GameObject[]

            for friendly = 0 to friendlyTiles.length
                legalMoves[] = new GameObject[]

                if friendly.GetChild().PieceManager.GetPieceType() != PieceType.king then
                    legalMoves = MoveGenerator.GetMoves(friendly, false, isKingWhite)
                endif

                friendlyLegalMoves.Add(legalMoves)
                relaventFriendlyTile.Add(friendly)
            next friendly

            checkerLegalMoves = new GameObject[]

            if checkerTile.GetChild().PieceManager.GetPieceType() == PieceType.rook
                    OR checkerTile.GetChild().PieceManager.GetPieceType() == PieceType.queen then
                xDiff = (int)checkedKingTile.TileManager.GetTilePosition().x- (int)checkerTile.TileManager.GetTilePosition().x
                yDiff = (int)checkedKingTile.TileManager.GetTilePosition().y- (int)checkerTile.TileManager.GetTilePosition().y

                if xDiff == 0 then
                    if yDiff >= 0 then
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 1)
                    else
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 0)
                    endif
                else
                    if xDiff >= 0 then
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 3)
                    else
                        checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 2)
                    endif
                endif
            endif

            if checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.bishop
                OR checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.queen
                xDiff = (int)checkedKingTile.TileManager.GetTilePosition().x - (int)checkerTile.TileManager.GetTilePosition().x
                yDiff = (int)checkedKingTile.TileManager.GetTilePosition().y - (int)checkerTile.TileManager.GetTilePosition().y

                if xDiff < 0 && yDiff < 0 then
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 0)
                else if xDiff < 0 && yDiff > 0 then
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 1)
                else if xDiff > 0 && yDiff < 0 then
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 2)
                else
                    checkerLegalMoves = GetComponent<MoveGenerator>().GetMoves(checkerTile, false, !isKingWhite, 3)
                endif
            endif

            if checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.knight
                    OR checkerTile.transform.GetChild(0).GetComponent<PieceManager>().GetPieceType() == PieceManager.PieceType.pawn
                checkerLegalMoves.Add(checkerTile)
            endif

            checkerLegalMoves.Add(GameManager.tiles[(int)checkerTile.TileManager.GetTilePosition().x,
            (int)checkerTile.TileManager.GetTilePosition().y])

            GameObject[,] finalLegalMoves = new GameObject[,]
            GameObject[] relaventFinalMoves = new GameObject[]

            for tile = 0 to relaventFriendlyTile.length
                GameObject[] myLegalMoves = friendlyLegalMoves[tile]
                GameObject[] tempFinalMoves = new List<GameObject>()
                for legalMove = 0 to myLegalMoves.length
                    if checkerLegalMoves.Contains(legalMove) then
                        tempFinalMoves.Add(legalMove)
                    endif
                next legalMove
                if tempFinalMoves.Count > 0 then
                    finalLegalMoves.Add(tempFinalMoves)
                    relaventFinalMoves.Add(tile)
                endif
            next tile

            if inalLegalMoves.Count > 0 then
                pieceIndex = Random.Range(0, finalLegalMoves.Count)
                moveIndex = Random.Range(0, finalLegalMoves[pieceIndex].Count)

                MovePiece(finalLegalMoves[pieceIndex][moveIndex], relaventFinalMoves[pieceIndex], false)
                GameManager.PieceMoved();
            else
                GetComponent<GameManager>().EndGame(0)
            endif
        endif
    endprocedure
endclass
