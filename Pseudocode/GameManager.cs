class GameManager
    public array tiles[8, 8]
    public takenWhitePieces[], takenBlackPieces[]

    public tilePrefab
    public chessBoard

    public isPlayerWhite = True
    public isWhiteMoving = True

    private procedure Start()
        CreateBoard()
    endprocedure

    public procedure PieceMoved()
        isWhiteMoving = !isWhiteMoving

        if  isWhiteMoving != isPlayerWhite then
            GetComponent<AIMovement>().Move()
        endif
    endprocedure

    private procedure CreateBoard()
        for (x = 0 to 7)
            for (y = 0 to 7)
                tiles[x, y] = CreateTile(x, y)
            next y
        next x
    endprocedure

    private function CreateTile(int x, int y)
        tile = Instantiate(tilePrefab, new Vector3(x, 0, y), chessBoard)

        tile.name = str(x) + ":" + str(y)

        tile.GetComponent<TileManager>().SetTilePosition(x, y)
        tile.GetComponent<TileManager>().SetTileColour((x + y) MOD 2)
        tile.GetComponent<TileManager>().InitializePiece()

        return tile;
    endfunction
endclass


