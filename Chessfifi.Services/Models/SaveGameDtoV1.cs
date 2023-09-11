namespace Chessfifi.Services.Models;
public class SaveGameDtoV1
{
    public List<Move> Moves { get; set; }

    public string Positions { get; set; }

    public class Move
    {
        public Position From { get; set; }
        public Position To { get; set; }

        public string Runner { get; set; }

        public string KillEnemy { get; set; }

        public Move AdditionalMove { get; set; }
    }

    public class Position
    {
        public int X { get; set; }

        public int Y { get; set; }
    }
}
