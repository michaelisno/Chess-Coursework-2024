class PieceManager
    public enum PieceType
        none,
        pawn,
        rook,
        knight,
        bishop,
        queen,
        king
    endenum

    public pieceMeshes[]
    public pieceMaterials[]
    private pieceType
    private colour
    private hasPieceMoved = False

    public procedure SetPieceColour(_colour)
        colour = _colour
        materials[] = MeshRenderer.materials
        materials[1] = pieceMaterials[colour]
        MeshRenderer.materials = materials
    endprocedure

    public procedure SetPieceType(newType)
        pieceType = newType

        if pieceType == PieceType.none then
            gameObject.SetActive(False)
            return
        endif

        MeshFilter.mesh = pieceMeshes[int(pieceType) - 1]
        gameObject.SetActive(true)
    endprocedure

    public procedure SetHasPieceMoved(_hasPieceMoved) 
        hasPieceMoved = _hasPieceMoved
    endprocedure

    public function GetPieceColour() 
        return colour
    endfunction
    public function GetPieceType() 
        return pieceType
    endfunction
    public function GetHasPieceMoved()
        return hasPieceMoved;
    endfunction
endclass
