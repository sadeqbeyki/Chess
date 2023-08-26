namespace Chessfifi.Domain
{
    public class ChessMove
    {
        public int Id { get; set; }
        public int FromRow { get; set; }
        public int FromColumn { get; set; }
        public int ToRow { get; set; }
        public int ToColumn { get; set; }
        public string MovingPiece { get; set; } // نوع مهره (شاه، وزیر، ...)
    }

}