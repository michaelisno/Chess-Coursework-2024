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

                tileIndex = int(8 * position.x + position.y - 10)
                if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                    legalMoves.Append(GameManager.tiles[tileIndex])
                endif

                tileIndex = int(8 * position.x + position.y + 15)
                if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                    legalMoves.Append(GameManager.tiles[tileIndex])
                endif

                tileIndex = int(8 * position.x + position.y - 17)
                if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                    legalMoves.Append(GameManager.tiles[tileIndex])
                endif

                if position.y == 0 then
                    tileIndex = int(8 * position.x + position.y - 15)
                    if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                        legalMoves.Append(GameManager.tiles[tileIndex])
                    endif

                    tileIndex = int(8 * position.x + position.y + 17)
                    if CheckLegality(tileIndex, PieceType.knight, isPlayerWhite) then
                        legalMoves.Append(GameManager.tiles[tileIndex])
                    endif
                endif
            endif
        endif

        # rook and lateral queen
        if pieceType == PieceType.rook or pieceType == PieceType.queen then
            for i = position.y to 7
                tempIndex = int(8 * position.x + tileIndex + 1)
                if CheckLegality(tempIndex, PieceType.rook, isPlayerWhite) then
                    legalMoves.Add(GameManager.tiles[tempIndex])

                    if GameManager.tiles[tempIndex].GetChild().Piece.Type != PieceType.none && GameManager.tiles[tempIndex].GetChild().Piece.GetColour() 
                        != int(isPlayerWhite) then
                        break
                    endif
                else
                    break
                endif
            next i

            for i = position.y to 0
                tempIndex = int(8 * position.x + tileIndex - 1)
                if CheckLegality(tempIndex, PieceType.rook, isPlayerWhite) then
                    legalMoves.Add(GameManager.tiles[tempIndex])

                    if GameManager.tiles[tempIndex].GetChild().Piece.Type != PieceType.none && GameManager.tiles[tempIndex].GetChild().Piece.GetColour() 
                            != int(isPlayerWhite) then
                        break
                    endif
                else
                    break
                endif
            previous i

            for i = position.y to 7
                tempIndex = int(position.y + (tileIndex + 1)*8)
                if CheckLegality(tempIndex, PieceType.rook, isPlayerWhite) then
                    legalMoves.Add(GameManager.tiles[tempIndex])

                    if GameManager.tiles[tempIndex].GetChild().Piece.Type != PieceType.none && GameManager.tiles[tempIndex].GetChild().Piece.GetColour() 
                            != int(isPlayerWhite) then
                        break
                    endif
                else
                    break
                endif
            next i

            for i = position.y to 0
                tempIndex = int(position.y + (tileIndex - 1)*8)
                if CheckLegality(tempIndex, PieceType.rook, isPlayerWhite) then
                    legalMoves.Add(GameManager.tiles[tempIndex])

                    if GameManager.tiles[tempIndex].GetChild().Piece.Type != PieceType.none && GameManager.tiles[tempIndex].GetChild().Piece.GetColour() 
                            != int(isPlayerWhite) then
                        break
                    endif
                else
                    break
                endif
            next i
        endif

        # bishop and diagonal queen
        if pieceType = PieceType.bishop or pieceType == PieceType.queen then
            originPosition = int(8*position.x + position.y)
            
            for i = int(position.x * 8 + position.y + 9) to 63
                if CheckLegality(isPlayerWhite, PieceType.bishop, isPlayerWhite, originPosition) then
                    legalMoves.Add(GameManager.tiles[i])
                
                    if GameManager.tiles[i].GetChild().Piece.GetType() != PieceType.none && GameManager.tiles[testedPosition].GetChild().Piece.GetColour() 
                            != int(isPlayerWhite) then
                        break
                    endif
                else
                    break
                endif
            next9th i

            for i = int(position.x * 8 + position.y - 7) to 0
                if CheckLegality(isPlayerWhite, PieceType.bishop, isPlayerWhite, originPosition) then
                    legalMoves.Add(GameManager.tiles[i])


                    if GameManager.tiles[i].GetChild().Piece.GetType() != PieceType.none && GameManager.tiles[testedPosition].GetChild().Piece.GetColour() 
                            != int(isPlayerWhite) then
                        break
                    endif
                else
                    break
                endif
            previous7th i

            for i = int(position.x * 8 + position.y + 7) to 63
                if CheckLegality(isPlayerWhite, PieceType.bishop, isPlayerWhite, originPosition) then
                    legalMoves.Add(GameManager.tiles[i])


                    if GameManager.tiles[i].GetChild().Piece.GetType() != PieceType.none && GameManager.tiles[testedPosition].GetChild().Piece.GetColour() 
                            != int(isPlayerWhite) then
                        break
                    endif
                else
                    break
                endif
            next7th i

            for i = int(position.x * 8 + position.y - 9) to 0
                if CheckLegality(isPlayerWhite, PieceType.bishop, isPlayerWhite, originPosition) then
                    legalMoves.Add(GameManager.tiles[i])


                    if GameManager.tiles[i].GetChild().Piece.GetType() != PieceType.none && GameManager.tiles[testedPosition].GetChild().Piece.GetColour() 
                            != int(isPlayerWhite) then
                        break
                    endif
                else
                    break
                endif
            previous9th i
        endif

        # king
        if pieceType == PieceType.king then
            currentPosition = int(position.x * 8 + position.y)
            if CheckLegality(currentPosition + 1, PieceType.king, isPlayerWhite White) && originPosition.y < 7 then
                legalMoves.Add(GameManager.tiles[currentPosition + 1])
            if CheckLegality(currentPosition - 1, PieceType.king, isPlayerWhite White) && originPosition.y > 0 then
                legalMoves.Add(GameManager.tiles[currentPosition - 1])
            if CheckLegality(currentPosition - 7, PieceType.king, isPlayerWhite White) && originPosition.y < 7 then
                legalMoves.Add(GameManager.tiles[currentPosition - 7])
            if CheckLegality(currentPosition + 9, PieceType.king, isPlayerWhite White) && originPosition.y < 7 then
                legalMoves.Add(GameManager.tiles[currentPosition + 9])
            if CheckLegality(currentPosition + 8, PieceType.king, isPlayerWhite White) then
                legalMoves.Add(GameManager.tiles[currentPosition + 8])
            if CheckLegality(currentPosition - 8, PieceType.king, isPlayerWhite White) then
                legalMoves.Add(GameManager.tiles[currentPosition - 8])
            if CheckLegality(currentPosition - 9, PieceType.king, isPlayerWhite White) && originPosition > 0 then
                legalMoves.Add(GameManager.tiles[currentPosition - 9])
            if CheckLegality(currentPosition + 7, PieceType.king, isPlayerWhite White) && originPosition > 0 then
                legalMoves.Add(GameManager.tiles[currentPosition + 7])
        endif
        
        return legalMoves
    endfunction

    # Seperate function to check if moves are legal
    private function CheckLegality(tileIndex, PieceType type, isPlayerWhite, originPosition = 0)
        if tileIndex >= 0 && tileIndex <= 63 then
            piece = GameManager.tiles[tileIndex].GetChild().Piece
            tile = GameManager.tiles[tileIndex].Tile

            switich type:
                case PieceType.knight:
                    return piece.GetType() == PieceType.none or (piece.GetType() != PieceType.none and piece.GetColour() != int (isPlayerWhite)
                case PieceType.pawn:
                    return piece.GetType() != PieceType.none and piece.GetColour() != int(isPlayerWhite)
                case PieceType.rook:
                    return piece.GetType() == PieceType.None or (piece.GetType() != PieceType.none and piece.GetColour() != int(isPlayerWhite))
                case PieceType.bishop:
                    return ((piece.GetType() == PieceType.none) or piece.GetType() != PieceType.none and piece.GetColour() != int(isPlayerWhite))   
                            and tile.GetColour() == GameManager.tiles[originPosition].Tile.GetColour()
                case PieceType.king:
                    return piece.GetType() == PieceType.none or (piece.GetType() != PieceType.none and piece.GetColour() != int(isPlayerWhite))
            endswitch
        else
            return false
        endif
endclass