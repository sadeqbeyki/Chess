namespace Chessfifi.Domain.ChessAgg;

public interface IPlayerRepository
{
    Player FindPlayerByUserId(string userId);
    Player CreatePlayer(string userId, string name);
    Player GetPlayer(int id);
}
