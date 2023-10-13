class Tile
    private tileMaterials[]
    private position, colour

    public procedure SetColour(_colour)
        MeshRenderer.material = tileMaterials[_colour]
        if _colour == 0 || _colour == 1 then
            colour = _colour
        endif
    endprocedure
   public procedure SetPosition(_position)
        position = _position
    endprocedure

    public function GetPosition()
        return position
    endfunction
    public function GetColour()
        return colour
    endfunction

    public procedure InitiatePieces()
        if position.y == 1 || position.y == 6 then
            Piece.SetType(PieceType.pawn)
        endif
        if position.y == 0 || position.y == 7 then
            if position.x == 0 || position.x == 7 then
                Piece.SetType(PieceType.rook)
            elseif position.x == 1 || position.x == 6 then
                Piece.SetType(PieceType.knight)
            elseif position.x == 2 || position.x == 5 then
                Piece.SetType(PieceType.bishop)
            elseif position.x == 3 then
                Piece.SetType(PieceType.queen)
            elseif position.x == 4 then
                Piece.SetType(PieceType.king)
            endif
        endif
        Piece.SetColour(position.y <= 3)
    endprocedure

endclass

