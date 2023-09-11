using Chessfifi.Common.Enums;
using Chessfifi.Contracts;
using Chessfifi.Contracts.Dto;

namespace Chessfifi.Services;
public interface IGameManager
{
    bool IsInit { get; }
    void Init(List<IGameInfo> games);
    void StartSearch(PlayerDto player, GameMode gameMode);
    void StopSearch(int playerId);
    SearchStatus Check(int playerId);
    SearchStatus Confirm(int playerId);
    IGameInfo FindMyPlayingGame(int playerId);
}
