namespace Chessfifi.Domain
{
    public class ChessSquare
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Piece { get; set; } // نوع مهره (شاه، وزیر، ...)
    }

    public class ChessBoard
    {
        public List<ChessSquare> Squares { get; set; } = new List<ChessSquare>();
    }

    public class ChessMove
    {
        public ChessSquare FromSquare { get; set; }
        public ChessSquare ToSquare { get; set; }
        public string MovingPiece { get; set; } // نوع مهره (شاه، وزیر، ...)
    }

    public class Player
    {
        public string Name { get; set; }
        public string ContactInfo { get; set; } // ایمیل یا شماره تماس بازیکن
        public bool IsTurn { get; set; } // نوبت بازی بازیکن
    }

}