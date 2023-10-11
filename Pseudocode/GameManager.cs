class GameManager
    public tiles
    public whiteTakenPieces, blackTakenPieces
    public isPlayerWhite = True

    private tilePrefab, board

    private procedure Start()
        for xPosition = 0 to 7
            for yPosition = 0 to 7
                tiles.Append(CreateTile(xPosition, yPosition))
            next yPosition
        next xPosition
    endprocedure

    private function CreateTile()
        newTile = Instantiate(tilePrefab, (xPosition, 0, yPosition), Quaternion.identity, board.transform)

        newTile.name = str(8 * xPosition * yPosition)
        newTile.tile.SetColour((xPosition + yPosition)%2)
        newTile.tile.SetPosition(xPosition, yPosition);
        newTile.tile.InitiatePieces()

        return newTile
    endfunction
endclass








