class TileManager
    public tileMaterials[];
    private position;
    private colour;
    private pieceManager;

    public procedure SetTilePosition(x, y) 
        position = new Vector2(x, y);
    endprocedure
    public procedure SetTileColour(_colour)
        colour = _colour;
        Renderer.material = tileMaterials[colour];
    endprocedure

    public function GetTilePosition() 
        return position;
    endfunction
    public function GetTileColour() 
        return colour;
    endfunction

    public procedure InitializePiece()
        colour = int(position.y <= 4);
        pieceManager.SetPieceColour(colour);

        if position.y == 1 OR position.y == 6 then
            pieceManager.SetPieceType(PieceType.pawn);
        elseif position.y == 0 AND (position.x == 0 OR position.x == 7) OR position.y == 7 AND (position.x == 0 OR position.x == 7) then
            pieceManager.SetPieceType(PieceType.rook);
        elseif position.y == 0 AND (position.x == 1 OR position.x == 6) OR position.y == 7 AND (position.x == 1 OR position.x == 6) then
            pieceManager.SetPieceType(PieceType.knight);
        elseif position.y == 0 AND (position.x == 2 OR position.x == 5) OR position.y == 7 AND (position.x == 2 OR position.x == 5) then
            pieceManager.SetPieceType(PieceType.bishop); ;
        elseif position.x == 3 AND (position.y == 0 OR position.y == 7) then
            pieceManager.SetPieceType(PieceType.queen);
        elseif position.x == 4 AND (position.y == 0 OR position.y == 7) then
            pieceManager.SetPieceType(PieceType.king);
        else
            pieceManager.SetPieceType(PieceType.none);
        endif
    endprocedure
endclass



