class MoveGenerator
    private gameManager
    private tiles[8, 8]
    private isPlayerWhite

    public function GetMoves(selectedTile, ignoreKing = False, _isPlayerWhite = True)
        tile = selectedTile.TileManager
        piece = selectedTile.GetChild().PieceManager
        gameManager = GameManager

        pieceType = piece.GetPieceType()
        position = tile.GetTilePosition()
        hasPieceMoved = piece.GetHasPieceMoved()
        tiles = gameManager.tiles

        isPlayerWhite = GameManager.isPlayerWhite

        if ignoreKing == True then
            isPlayerWhite != GameManager.isPlayerWhite;
        endif

        if _isPlayerWhite != GameManager.isPlayerWhite then
            isPlayerWhite = False
        endif

        return GenerateLegalMoves(pieceType, position, tiles, hasPieceMoved, piece, ignoreKing);
    endprocedure

    private function GenerateLegalMoves(pieceType, position, tiles, hasPieceMoved, piece, ignoreKing)
        if pieceType == PieceManager.PieceType.pawn then
            if isPlayerWhite == True then
                if ignoreKing == False then
                    newY = (int)position.y + 1
                    if CheckMove(position.x, newY) then
                        legalMoves.Add(tiles[(int)position.x, newY])
                    endif

                    newY = (int)position.y + 2
                    if CheckMove(position.x, newY) AND hasPieceMoved == False then
                        legalMoves.Add(tiles[(int)position.x, newY])
                    endif

                    if CheckMove((position.x - 1, position.y + 1), True) then
                        legalMoves.Add(tiles[(int)position.x - 1, (int)position.y + 1])
                    endif

                    if CheckMove((position.x + 1, position.y + 1), True) then
                        legalMoves.Add(tiles[(int)position.x + 1, (int)position.y + 1])
                    endif
                else
                    if CheckMove((position.x - 1, position.y + 1), True) OR CheckMove(position.x - 1, position.y + 1) then
                        legalMoves.Add(tiles[(int)position.x - 1, (int)position.y + 1])
                    endif

                    if CheckMove((position.x + 1, position.y + 1), True) OR CheckMove(position.x + 1, position.y + 1) then
                        legalMoves.Add(tiles[(int)position.x + 1, (int)position.y + 1])
                    endif
                endif
            else
                if ignoreKing == False then
                    newY = (int)position.y - 1
                    if CheckMove(position.x, newY) then
                        legalMoves.Add(tiles[(int)position.x, newY])
                    endif

                    newY = (int)position.y - 2;
                    if CheckMove(position.x, newY) AND hasPieceMoved == False then
                        legalMoves.Add(tiles[(int)position.x, newY])
                    endif

                    if CheckMove((position.x - 1, position.y - 1), True) then
                        legalMoves.Add(tiles[(int)position.x - 1, (int)position.y - 1])
                    endif

                    if CheckMove((position.x + 1, position.y - 1), True)
                        legalMoves.Add(tiles[(int)position.x + 1, (int)position.y - 1])
                    endif
                else
                    if CheckMove((position.x - 1, position.y - 1), True) OR CheckMove(position.x - 1, position.y - 1) then
                        legalMoves.Add(tiles[(int)position.x - 1, (int)position.y - 1])
                    endif

                    if CheckMove((position.x + 1, position.y - 1), True) OR CheckMove(position.x + 1, position.y - 1) then
                        legalMoves.Add(tiles[(int)position.x + 1, (int)position.y - 1])
                    endif
                endif
            endif
        endif

        if pieceType == PieceType.rook OR pieceType == PieceType.queen then
            for newY = (int)position.y + 1 to 8
                if !CheckMove((int)position.x, newY) then
                    if CheckMove((int)position.x, newY), True) then
                        legalMoves.Add(tiles[(int)position.x, newY])
                    endif
                    break
                endif
                legalMoves.Add(tiles[(int)position.x, newY])
            next newY

            for newY = (int)position.y - 1 to 0
                if !CheckMove((int)position.x, newY) then
                    if CheckMove((int)position.x, newY), True) then
                        legalMoves.Add(tiles[(int)position.x, newY])
                    endif
                    break
                endif
                legalMoves.Add(tiles[(int)position.x, newY])
            last newY

            for newX = (int)position.x + 1 to 8
                if !CheckMove(newX, (int)position.y) then
                    if CheckMove((newX, (int)position.y), True) then
                        legalMoves.Add(tiles[newX, (int)position.y])
                    endif
                    break
                endif
                legalMoves.Add(tiles[newX, (int)position.y])
            next newX

            for newX = (int)position.x - 1 to 0
                if !CheckMove(newX, (int)position.y) then
                    if CheckMove((newX, (int)position.y), True) then
                        legalMoves.Add(tiles[newX, (int)position.y])
                    endif
                    break
                endif
                legalMoves.Add(tiles[newX, (int)position.y])
            last newX
        endif

        if pieceType == PieceType.bishop OR pieceType == PieceType.queen then
            for extra = 1 to 8
                if !CheckMove(position.x + extra, position.y + extra) then
                    if CheckMove((position.x + extra, position.y + extra), True) then
                        legalMoves.Add(tiles[(int)position.x + extra, (int)position.y + extra])
                    endif
                    break
                endif
                legalMoves.Add(tiles[(int)position.x + extra, (int)position.y + extra])
            next extra

            for extra = 1 to 8
                if !CheckMove(position.x - extra, position.y + extra) then
                    if CheckMove((position.x - extra, position.y + extra), True) then
                        legalMoves.Add(tiles[(int)position.x - extra, (int)position.y + extra])
                    endif
                    break
                endif
                legalMoves.Add(tiles[(int)position.x - extra, (int)position.y + extra])
            next extra

            for extra = 1 to 8
                if !CheckMove(position.x + extra, position.y - extra) then
                    if CheckMove((position.x + extra, position.y - extra), True) then
                        legalMoves.Add(tiles[(int)position.x + extra, (int)position.y - extra])
                    endif
                    break
                endif
                legalMoves.Add(tiles[(int)position.x + extra, (int)position.y - extra])
            next extra

            for extra = 1 to 8
                if !CheckMove(position.x - extra, position.y - extra) then
                    if CheckMove((position.x - extra, position.y - extra), True) then
                        legalMoves.Add(tiles[(int)position.x - extra, (int)position.y - extra])
                    endif
                    break
                endif
                legalMoves.Add(tiles[(int)position.x - extra, (int)position.y - extra])
            next extra
        endif        

        if pieceType == PieceType.knight then
            if CheckMove(position.x + 2, position.y + 1) OR CheckMove((position.x + 2, position.y + 1), True) then
                legalMoves.Add(tiles[(int)position.x + 2, (int)position.y + 1])
            endif

            if CheckMove(position.x - 2, position.y - 1) OR CheckMove((position.x - 2, position.y - 1), True) then
                legalMoves.Add(tiles[(int)position.x - 2, (int)position.y - 1])
            endif

            if CheckMove(position.x + 2, position.y - 1) OR CheckMove((position.x + 2, position.y - 1), True) then
                legalMoves.Add(tiles[(int)position.x + 2, (int)position.y - 1])
            endif

            if CheckMove(position.x - 2, position.y + 1) OR CheckMove((position.x - 2, position.y + 1), True) then
                legalMoves.Add(tiles[(int)position.x - 2, (int)position.y + 1])
            endif

            if CheckMove(position.x - 1, position.y + 2) OR CheckMove((position.x - 1, position.y + 2), True) then
                legalMoves.Add(tiles[(int)position.x - 1, (int)position.y + 2])
            endif

            if CheckMove(position.x + 1, position.y + 2) OR CheckMove((position.x + 1, position.y + 2), True) then
                legalMoves.Add(tiles[(int)position.x + 1, (int)position.y + 2])
            endif

            if CheckMove(position.x - 1, position.y - 2) OR CheckMove((position.x - 1, position.y - 2), True) then
                legalMoves.Add(tiles[(int)position.x - 1, (int)position.y - 2])
            endif

            if CheckMove(position.x + 1, position.y - 2) OR CheckMove((position.x + 1, position.y - 2), True) then
                legalMoves.Add(tiles[(int)position.x + 1, (int)position.y - 2])
            endif
        endif

        if pieceType == PieceType.king AND ignoreKing == False then
            potentialLegalMoves = GameObject[]

            if CheckMove(position.x, position.y + 1) OR CheckMove((position.x, position.y + 1), True) then
                potentialLegalMoves.Add(tiles[(int)position.x, (int)position.y + 1])
            endif
            if CheckMove(position.x + 1, position.y + 1) OR CheckMove((position.x + 1, position.y + 1), True) then
                potentialLegalMoves.Add(tiles[(int)position.x + 1, (int)position.y + 1])
            endif
            if CheckMove(position.x - 1, position.y + 1) OR CheckMove((position.x - 1, position.y + 1), True) then
                potentialLegalMoves.Add(tiles[(int)position.x - 1, (int)position.y + 1])
            endif
            if CheckMove(position.x - 1, position.y) OR CheckMove((position.x - 1, position.y), True) then
                potentialLegalMoves.Add(tiles[(int)position.x - 1, (int)position.y])
            endif
            if CheckMove(position.x + 1, position.y) OR CheckMove((position.x + 1, position.y), True) then
                potentialLegalMoves.Add(tiles[(int)position.x + 1, (int)position.y])
            endif
            if CheckMove(position.x, position.y - 1) OR CheckMove((position.x, position.y - 1), True) then
                potentialLegalMoves.Add(tiles[(int)position.x, (int)position.y - 1])
            endif
            if CheckMove(position.x + 1, position.y - 1) OR CheckMove((position.x + 1, position.y - 1), True) then
                potentialLegalMoves.Add(tiles[(int)position.x + 1, (int)position.y - 1])
            endif
            if CheckMove(position.x - 1, position.y - 1) OR CheckMove((position.x - 1, position.y - 1), True) then
                potentialLegalMoves.Add(tiles[(int)position.x - 1, (int)position.y - 1])
            endif

            for tile = 0 to tiles.length
                if tile.GetChild().active AND bool(tile.GetChild().PieceManager.GetPieceColour()) != GameManager.isPlayerWhite then
                    tempLegalMoves = GetMoves(tile, True)
                    for legalMove = 0 to tempLegalMoves.length
                        if potentialLegalMoves.Contains(legalMove) then
                            potentialLegalMoves.Remove(legalMove)
                        endif
                    next legalMove
                endif
            next tile

            legalMoves = potentialLegalMoves
        endif

        return legalMoves
    endfunction

    private function CheckMove(newPos, isTakingEnemy = False)
        if isTakingEnemy == False then
            if newPos.x >= 0 AND newPos.x <= 7 AND newPos.y >= 0 AND newPos.y <= 7 then
                return tiles[(int)newPos.x, (int)newPos.y].GetChild().PieceManager.GetPieceType() == PieceType.none
            else
                return False
            endif
        else
            if newPos.x >= 0 AND newPos.x <= 7 AND newPos.y >= 0 AND newPos.y <= 7 then
                return tiles[(int)newPos.x, (int)newPos.y].GetChild().PieceManager.GetPieceType() != PieceType.none 
                    AND bool(tiles[(int)newPos.x, (int)newPos.y].GetChild().PieceManager.GetPieceColour()) != isPlayerWhite
            else
                return False
            endif
        endif
    endfunction
endclass



