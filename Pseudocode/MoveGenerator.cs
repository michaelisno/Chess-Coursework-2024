class MoveGenerator
    public function GenerateMoves(isPlayerWhite, position, pieceType, hasPieceMoved)
        legalMoves[]
        tileIndex = 0

        if pieceType == PieceType.pawn then
            if isPlayerWhite then
                tileIndex = int(8 * position.x + position.y + 1)
            else
                tileIndex = int(8 * position.x + position.y - 1)
            endif
            
            if tileIndex >= 0 and tileIndex <= 63 and GameManager.tiles[tileIndex].GetChild(0).Piece.GetType() == PieceType.none then
                legalMoves.Append(GameManager.tiles[tileIndex])
            endif

            if !hasPieceMoved then
                if isPlayerWhite then
                    tileIndex = int(8 * position.x + position.y + 2)
                else
                    tileIndex = int(8 * position.x + position.y - 2)
                endif

                if tileIndex >= 0 and tileIndex <= 63 and GameManager.tiles[tileIndex].GetChild(0).Piece.GetType() == PieceType.none then
                    legalMoves.Append(GameManager.tiles[tileIndex])
                endif
            endif
        
            if isPlayerWhite then
                tileIndex = int(8 * position.x + position.y - 7)
            else
                tileIndex = int(8 * position.x + position.y - 9)
            endif
            
            if CheckLegality(tileIndex, PieceType.pawn, isPlayerWhite) then
                legalMoves.Append(GameManager.tiles[tileIndex])
            endif

            if isPlayerWhite then
                tileIndex = int(8 * position.x + position.y + 9)
            else
                tileIndex = int(8 * position.x + position.y + 7)
            endif
            
            if CheckLegality(tileIndex, PieceType.pawn, isPlayerWhite) then
                legalMoves.Append(GameManager.tiles[tileIndex])
            endif
        endif

        # knight
        if pieceType == PieceType.knight then
            if position.y != 7 and position.y != 6 or (position.y > 1 and position.y < 6) then
                tileIndex = int(8 * position.x + position.y - 6)
                if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                    legalMoves.Append(GameManager.tiles[tileIndex])
                endif
                
                tileIndex = int(8 * position.x + position.y + 10)
                if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                    legalMoves.Append(GameManager.tiles[tileIndex])
                endif

                tileIndex = int(8 * position.x + position.y - 15)
                if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                    legalMoves.Append(GameManager.tiles[tileIndex])
                endif

                tileIndex = int(8 * position.x + position.y + 17)
                if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                    legalMoves.Append(GameManager.tiles[tileIndex])
                endif

                if position.y == 1 then
                    tileIndex = int(8 * position.x + position.y + 15)
                    if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                        legalMoves.Append(GameManager.tiles[tileIndex])
                    endif

                    tileIndex = int(8 * position.x + position.y - 17)
                    if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                        legalMoves.Append(GameManager.tiles[tileIndex])
                    endif
                endif
            endif

            if position.y != 0 and position.y != 1 or (position.y > 1 and position.y < 6) then
                tileIndex = int(8 * position.x + position.y + 6)
                if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                    legalMoves.Append(GameManager.tiles[tileIndex])
                endif
            endif
    endfunction
endclass