class GameManager
    public tiles = [8,8]
    public tilePrefab
    public chessBoard

    private procedure Start()
        CreateBoard()
    endprocedure

    private procedure CreateBoard()
        for x = 0 to 7
            for y = 0 to 7
                tiles[x,y] = CreateTile(x, y)
            next y
        next x
    endprocedure

    private function CreateTile(x, y)
        tile = Instantiate(tilePrefab, (x,0,y), chessBoard)
        tile.name = str(x) + ":" + str(y)

        return tile
    endfunction
endclass

