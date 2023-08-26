namespace Chessfifi.Domain
{
    public class ChessSquare
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Piece { get; set; } // نوع مهره (شاه، وزیر، ...)
    }
}