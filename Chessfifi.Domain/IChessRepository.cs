namespace Chessfifi.Domain
{
    public interface IChessRepository
    {
        bool IsCheck(int gameId, string player);
        bool IsMoveValid(int gameId, int fromRow, int fromColumn, int toRow, int toColumn);
        void MakeMove(int gameId, int fromRow, int fromColumn, int toRow, int toColumn);
    }
}