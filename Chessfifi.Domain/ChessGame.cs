namespace Chessfifi.Domain
{
    public class ChessGame
    {
        public int Id { get; set; }
        public List<ChessSquare> ChessBoard { get; set; }
        public List<ChessMove> ChessMoves { get; set; }
        public List<Player> Players { get; set; }
    }

}