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
        /// <summary>
        /// ������.
        /// </summary>
        public PositionViewModel From { get; set; }

        /// <summary>
        /// ����.
        /// </summary>
        public PositionViewModel To { get; set; }

        /// <summary>
        /// ��� �����.
        /// </summary>
        public string Runner { get; set; }

        /// <summary>
        /// ���� ����� ���� ������ ��������� ������.
        /// </summary>
        public string KillEnemy { get; set; }

        /// <summary>
        /// ��� ���� ������, ��������� � �����, �� ��� ����.
        /// </summary>
        public MoveViewModel AdditionalMove { get; set; }
    }

    public class PositionViewModel
    {
        /// <summary>
        /// �� �����������.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// �� ���������.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// ������.
        /// </summary>
        public string Piece { get; set; }
    }

    public class PlayerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
