class SelectionManager
{
    private hit
    private selectedTile = None
    private highlightedTiles[]

    private procedure Update()
        if Input.GetMouseButtonDown(0) AND GameManager.isWhiteMoving == GameManager.isPlayerWhite then
            if Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) then
                if selectedTile == null then
                    SelectNewPiece()
                    return
                endif            

                tilePosition = selectedTile.TileManager.GetTilePosition()
                oldPiece = selectedTile.GetChild().PieceManager
                newPiece = hit.collider.GetChild().PieceManager

                if (newPiece.GetPieceType() != PieceType.none AND bool(newPiece.GetPieceColour() then == GameManager.isPlayerWhite) OR 
                        highlightedTiles.Contains(hit.collider.gameObject))
                    selectedTile.TileManager.SetTileColour(int(tilePosition.x + tilePosition.y) MOD 2)
                    selectedTile = null
                endif

                if hit.collider.gameObject == oldPiece.transform.parent.gameObject then
                    HighlightLegalMoves(GameObject[])
                    return
                endif

                if highlightedTiles.Contains(hit.collider.gameObject) then AND hit.collider.GetChild().PieceManager.GetPieceType() 
                        == PieceType.none) then
                    newPiece.SetHasPieceMoved(True)
                    newPiece.SetPieceColour(oldPiece.GetPieceColour())
                    newPiece.SetPieceType(oldPiece.GetPieceType())

                    oldPiece.SetPieceType(PieceType.none)
                    oldPiece.SetHasPieceMoved(False)

                    HighlightLegalMoves(GameObject[])
                    GameManager.PieceMoved()
                    return
                else if bool(newPiece.GetPieceColour()) == GameManager.isPlayerWhite then
                    HighlightLegalMoves(GameObject[])
                    SelectNewPiece()
                    return
                endif

                if highlightedTiles.Contains(hit.collider.gameObject) AND hit.collider.GetChild().PieceManager.GetPieceType() != PieceType.none
                        AND bool(hit.collider.GetChild().PieceManager.GetPieceColour()) != GetComponent<GameManager>().isPlayerWhite then
                    pieceToTake = hit.collider.GetChild().gameObject.Clone();
                    pieceToTake.name = str(pieceToTake.PieceManager.GetPieceType())

                    if GameManager.isPlayerWhite then
                        GameManager.takenBlackPieces.Add(pieceToTake)
                    else
                        GameManager.takenWhitePieces.Add(pieceToTake)
                    endif
 
                    newPiece.SetHasPieceMoved(True)
                    newPiece.SetPieceColour(oldPiece.GetPieceColour())
                    newPiece.SetPieceType(oldPiece.GetPieceType())

                    oldPiece.SetPieceType(PieceType.none)
                    oldPiece.SetHasPieceMoved(False)

                    HighlightLegalMoves(GameObject[])
                    GameManager.PieceMoved()

                    return
                endif
            endif
        endif
    endprocedure

    private procedure SelectNewPiece()
        if hit.GetChild().PieceManager.GetPieceType() == PieceType.none OR hit.GetChild().PieceManager.GetPieceColour() 
                != int(GameManager.isPlayerWhite) then
            return
        endif

        selectedTile = hit.collider.gameObject
        selectedTile.TileManager.SetTileColour(2)

        legalMoves[] = MoveGenerator.GetMoves(selectedTile)

        if legalMoves.length > 0 then
            HighlightLegalMoves(legalMoves)
        endif
    endprocedure

    private procedure HighlightLegalMoves(legalMoves)
        for tile = 0 to highlightedTiles.length
            tilePosition = tile.TileManager.GetTilePosition()
            tile.TileManager.SetTileColour(int(tilePosition.x + tilePosition.y) MOD 2)
        next tile

        highlightedTiles.Clear()

        for tile = 0 to highlightedTiles.length
            tile.TileManager.SetTileColour(3)
            highlightedTiles.Add(tile)
        next tile
    endprocedure
endclass


