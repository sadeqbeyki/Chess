using Chessfifi.Contracts;
using Chessfifi.Contracts.Dto;

namespace Chessfifi.Services.Service;
public interface IGameService
{
    public void SaveGame(IGameInfo game);
    public HistoryGame GetGame(string gameId);
    public IEnumerable<HistoryGame> GetGames(int playerId);
    List<IGameInfo> GetNotFinishGames();
}
