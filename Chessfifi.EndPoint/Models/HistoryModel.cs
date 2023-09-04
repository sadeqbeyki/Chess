namespace Chessfifi.EndPoint.Models;

public class HistoryModel
{
    public List<Game> Games { get; set; }
    public int MyPlayerId { get; set; }

    public class Game
    {
        public string Id { get; set; }

        public Player WhitePlayer { get; set; }

        public Player BlackPlayer { get; set; }

        public Chessfifi.Common.Enums.FinishReason? FinishReason { get; set; }

        public Chessfifi.Common.Enums.GameSide? WinSide { get; set; }
    }

    public class Player
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
