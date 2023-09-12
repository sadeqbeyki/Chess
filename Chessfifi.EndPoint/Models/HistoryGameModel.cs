namespace Chessfifi.EndPoint.Models;

public class HistoryGameModel
{
    public string Id { get; set; }

    public PlayerViewModel WhitePlayer { get; set; }

    public PlayerViewModel BlackPlayer { get; set; }

    public List<MoveViewModel> Moves { get; set; }

    public string Positions { get; set; }

    public Chessfifi.Common.Enums.FinishReason? FinishReason { get; set; }

    public Chessfifi.Common.Enums.GameSide? WinSide { get; set; }

    public class MoveViewModel
    {

        public PositionViewModel From { get; set; }

        public PositionViewModel To { get; set; }


        public string Runner { get; set; }

        public string KillEnemy { get; set; }

        public MoveViewModel AdditionalMove { get; set; }
    }

    public class PositionViewModel
    {

        public int X { get; set; }

        public int Y { get; set; }

        public string Piece { get; set; }
    }

    public class PlayerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
