using Chessfifi.Common.Enums;

namespace Chessfifi.Domain.ChessAgg;
public interface IGameRepository
{
    Game GetGame(string gameId);
    void SaveGame(string id, int whitePlayerId, int blackPlayerId,
        Common.Enums.FinishReason? finishReason, GameSide? winSide, GameMode gameMode, string data);
    List<Game> GetGames(int playerId);
    List<Game> GetNotFinishGames();
}
