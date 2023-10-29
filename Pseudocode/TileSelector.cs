class TileSelector
    public selectedTile = None

    private ray
    private hit

    private highlightedTiles[]

    private procedure Update()
        if Input.GetMouseButtonDown(0) then
            ray = Camera.ScreenPointToRay(Input.mousePosition)
            
            if Physics.Raycast(ray, out hit) then
                if hit.gameObject.tag == "tile" then
                    SelectTile()
                endif
            endif
        endif
    endprocedure

    private procedure SelectTile()
        tile = hit.gameObject
        
        if highlightedTiles.Contains(tile) then
            if tile.GetChild(0).Piece.GetType() != PieceType.none then
                takenPiece = new Piece()

                if tile.GetChild(0).Piece.GetColour() == 1 then
                    takenPiece = tile.GetChild(0).Piece;
                    GameManager.whiteTakenPieces.Add(takenPiece)
                endif
                if tile.GetChild(0).Piece.GetColour() == 0 then
                    takenPiece = tile.GetChild(0).Piece;
                    GameManager.blackTakenPieces.Add(takenPiece)
                endif
            endif
            
            tile.GetChild(0).Piece.SetType(selectedTile.GetChild(0).Piece.GetType())
            tile.GetChild(0).Piece.SetColour(int(GameManager.isPlayerWhite))
            tile.GetChild(0).Piece.hasPieceMoved = true
            selectedTile.GetChild(0).Piece.SetType(PieceType.none)
            selectedTile.GetChild(0).Piece.SetColour(int(selectedTile.Tile.GetPosition().x + 
                selectedTile.Tile.GetPosition().y) % 2)
            selectedTile = null

            HighlightLegalTiles([])
        else
            if selectedTile == tile then
                selectedTile.Tile.SetColour(int(selectedTile.Tile.GetPosition().x + 
                    selectedTile.Tile.GetPosition().y) % 2)
                selectedTile = null
                HighlightLegalTiles([])
            else
                if tile.GetChild(0).Piece.GetType() != PieceType.none && tile.GetChild(0).Piece.GetColour() == int(GameManager.isPlayerWhite) then
                    selectedTile.Tile.SetColour(int(selectedTile.Tile.GetPosition().x + selectedTile.Tile.GetPosition().y) % 2)
                    selectedTile = tile
                    tile.Tile.SetColour(3)

                    HighlightLegalTiles(MoveGenerator.GenerateMoves(GameManager.isPlayerWhite, selectedTile.Tile.GetPosition(), selectedTile.GetChild(0).Piece.GetType(),
                        selectedTile.GetChild(0).Piece.hasPieceMoved))
                endif
            endif
        endif
    endprocedure
    
    private procedure HighlightLegalTiles(legalTiles[])
        for t = 0 to len(highlightedTiles)
            tile.Tile.SetColour(int(tile.Tile.GetPosition().x + tile.Tile.GetPosition.y) % 2)
        next t

        highlightedTiles.Clear()

        for t = 0 to len(legalTiles)
            tile.Tile.SetColour(2)
            highlightedTiles.Add(tile)
        next t
    endprocedure
endclass