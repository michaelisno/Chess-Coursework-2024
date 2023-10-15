class Piece
    public PieceType { none, pawn, rook, knight, bishop, queen, king }
    
    private pieceModels[6]
    private pieceMaterials[4]

    private hasPieceMoved

    private type
    private colour

    public function GetType()
        return type
    endfunction

    public function GetColour()
        return colour
    endfunction

    public procedure SetType(_type)
        this.SetActive(True)
        type = _type
        
        switch type:
            case none:
                this.SetActive(False)
            case pawn:
                MeshFilter.mesh = pieceModels[0]
            case rook:
                MeshFilter.mesh = pieceModels[1]
            case knight:
                MeshFilter.mesh = pieceModels[2]
            case bishop:
                MeshFilter.mesh = pieceModels[3]
            case queen:
                MeshFilter.mesh = pieceModels[4]
            case king:
                MeshFilter.mesh = pieceModels[5]
        endswitch
    endprocedure

    public procedure SetColour(_colour)

    endprocedure
endclass