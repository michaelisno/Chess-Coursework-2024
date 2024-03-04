class SelectionManager
    private hit
    private selectedTile = None
    private highlightedTiles[]

    private procedure Update()
        if Input.GetMouseButtonDown(0) AND (GameManager().isWhiteMoving == GameManager().isPlayerWhite) then
            if Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) then
                if !GameManager().isPlayerChecked then
                    if selectedTile == None then
                        SelectNewPiece()
                        return
                    endif

                    tilePosition = selectedTile.TileManager().GetTilePosition()
                    oldPiece = selectedTile.GetChild(0).PieceManager()
                    newPiece = hit.GetChild(0).PieceManager()

                    if (newPiece.GetPieceType() != PieceType.none AND bool(newPiece.GetPieceColour()) == GameManager().isPlayerWhite) 
                            OR highlightedTiles.Contains(hit.gameObject) then
                        selectedTile.TileManager().SetTileColour(int(tilePosition.x + tilePosition.y) MOD 2)
                        selectedTile = null
                    endif

                    if hit.gameObject == oldPiece.parent.gameObject then
                        HighlightLegalMoves(GameObject[])
                        return
                    endif

                    if highlightedTiles.Contains(hit.gameObject) AND hit.transform.GetChild().PieceManager().GetPieceType() 
                            == PieceManager.PieceType.none then
                        if oldPiece.GetPieceType() == PieceType.king then
                            if hit.transform.GetChild().PieceManager().GetPieceColour() == 0 then
                                GameManager().blackKingPosition = newPiece.TileManager().GetTilePosition()
                            else
                                GameManager().whiteKingPosition = newPiece.TileManager().GetTilePosition()
                            endif
                        endif

                        newPiece.SetHasPieceMoved(true)
                        newPiece.SetPieceColour(oldPiece.GetPieceColour())
                        newPiece.SetPieceType(oldPiece.GetPieceType())

                        oldPiece.SetPieceType(PieceType.none)
                        oldPiece.SetHasPieceMoved(false)

                        HighlightLegalMoves(GameObject[])
                        GameManager().PieceMoved()
                        return
                    else if bool(newPiece.GetPieceColour()) == GameManager().isPlayerWhite then
                        HighlightLegalMoves(GameObject[])
                        SelectNewPiece()
                        return
                    endif

                    if highlightedTiles.Contains(hit.gameObject) AND hit.transform.GetChild().PieceManager().GetPieceType() != PieceType.none
                            AND bool(hit.transform.GetChild().PieceManager().GetPieceColour()) != GameManager().isPlayerWhite then
                        if oldPiece.GetPieceType() == PieceType.king then
                            if hit.transform.GetChild().PieceManager().GetPieceColour() == 0 then
                                GameManager().blackKingPosition = newPiece.TileManager().GetTilePosition()
                            else
                                GameManager().whiteKingPosition = newPiece.TileManager().GetTilePosition()
                            endif
                        endif

                        pieceToTake = hit.GetChild().gameObject.Clone()
                        pieceToTake.name = str(pieceToTake.PieceManager().GetPieceType())

                        if GameManager().isPlayerWhite then 
                            GameManager().takenBlackPieces.Add(pieceToTake)
                        else
                            GameManager().takenWhitePieces.Add(pieceToTake)
                        endif
   
                        newPiece.SetHasPieceMoved(true)
                        newPiece.SetPieceColour(oldPiece.GetPieceColour())
                        newPiece.SetPieceType(oldPiece.GetPieceType())

                        oldPiece.SetPieceType(PieceType.none)
                        oldPiece.SetHasPieceMoved(false)

                        HighlightLegalMoves(GameObject[])
                        GameManager().PieceMoved()

                        return
                    endif
                else
                    if hit.transform.GetChild().PieceManager().GetPieceType() == PieceType.king
                            AND bool(hit.transform.GetChild().PieceManager().GetPieceColour()) == GameManager().isPlayerWhite then
                        kingLegalMoves = MoveGenerator().GetMoves(hit.gameObject, true, GameManager().isPlayerWhite)

                        if kingLegalMoves.Count > 0 then
                            HighlightLegalMoves(kingLegalMoves)
                        endif
                    else
                        enemyChecker = GameManager.checkerTile
                        enemyCheckerLegalMoves

                        kingPos
                        enemyPos = enemyChecker.TileManager().GetTilePosition()

                        bool isKingWhite = GameManager().isPlayerWhite

                        if GameManager().isPlayerWhite then
                            kingPos = GameManager().whiteKingPosition
                        else
                            kingPos = GameManager().blackKingPosition
                        endif

                        checkedKingTile = GameManager().tiles[(int)kingPos.x, (int)kingPos.y]

                        if enemyChecker.GetChild().PieceManager().GetPieceType() == PieceType.bishop
                            OR enemyChecker.GetChild().PieceManager().GetPieceType() == PieceType.queen then
                            int xDiff = (int)checkedKingTile.TileManager.GetTilePosition().x - (int)enemyChecker.TileManager().GetTilePosition().x
                            int yDiff = (int)checkedKingTile.TileManager().GetTilePosition().y - (int)enemyChecker.TileManager().GetTilePosition().y

                            if xDiff < 0 AND yDiff < 0 then
                                enemyCheckerLegalMoves = MoveGenerator().GetMoves(enemyChecker, false, !isKingWhite, 0)
                            else if xDiff < 0 && yDiff > 0 then
                                enemyCheckerLegalMoves = MoveGenerator().GetMoves(enemyChecker, false, !isKingWhite, 1)
                            else if xDiff > 0 && yDiff < 0 then
                                enemyCheckerLegalMoves = MoveGenerator().GetMoves(enemyChecker, false, !isKingWhite, 2)
                            else
                                enemyCheckerLegalMoves = MoveGenerator().GetMoves(enemyChecker, false, !isKingWhite, 3)
                            endif
                        endif

                        enemyCheckerLegalMoves.Add(enemyChecker)

                        viableFriendlyTiles = GameObject[]
                        viableFriendlyMoves = GameObject[]

                        for tile = 0 to GameManager.tiles.count
                            if tile.GetChild().gameObject.activeInHierarchy AND bool(tile.GetChild().PieceManager().GetPieceColour())
                                    == GameManager().isPlayerWhite then
                                tempFriendlyLegalMoves = MoveGenerator().GetMoves(tile, false, GameManager().isPlayerWhite)

                                finalLegalMoves = GameObject[]

                                for tempMove = 0 to tempFriendlyLegalMoves.count
                                    if enemyCheckerLegalMoves.Contains(tempMove) then
                                        finalLegalMoves.Add(tempMove)
                                    endif
                                next tempMove

                                if finalLegalMoves.Count > 0 then
                                    viableFriendlyTiles.Add(tile)
                                    viableFriendlyMoves.Add(finalLegalMoves)
                                endif
                            endif
                        nextTile

                        if viableFriendlyTiles.Contains(hit.collider.gameObject) then
                            index = viableFriendlyTiles.IndexOf(hit.gameObject)
                            HighlightLegalMoves(viableFriendlyMoves[index])
                            GameManager().isPlayerChecked = false
                        endif

                        if viableFriendlyTiles.Count == 0 then
                            kingLegalMoves = MoveGenerator().GetMoves(hit.gameObject, true, GameManager().isPlayerWhite)
                            if kingLegalMoves.Count == 0 then
                                GameManager().EndGame(int(GameManager().isPlayerWhite))
                            endif
                        endif
                    endif
                endif
            endif
        endif
    endprocedure

    private procedure SelectNewPiece()
        if (hit.GetChild().PieceManage().GetPieceType() == PieceType.none OR hit.GetChild().PieceManager().GetPieceColour() 
                != int(GameManager().isPlayerWhite))
            return

        selectedTile = hit.gameObject
        selectedTile.TileManager().SetTileColour(2)

        legalMoves = MoveGenerator().GetMoves(selectedTile, false, GameManager.isPlayerWhite)

        if legalMoves.Count > 0 then
            HighlightLegalMoves(legalMoves)
        endif
    endprocedure

    private procedure HighlightLegalMoves(legalMoves)
        for tile = 0 to highlightedTiles.count
            tilePosition = tile.TileManager().GetTilePosition()
            tile.TileManager().SetTileColour(int(tilePosition.x + tilePosition.y) MOD 2)
        next tile

        highlightedTiles.Clear()

        for tile = 0 to legalMoves.count
            tile.TileManager().SetTileColour(3)
            highlightedTiles.Add(tile)
        next tile
    endprocedure
endclass
