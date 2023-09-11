using Chessfifi.Contracts.Dto;

namespace Chessfifi.Services.Service;
public interface IPlayerService
{
    public PlayerDto FindPlayerByUserId(string userId);
    public PlayerDto GetOrCreatePlayerByUserId(string userId, string name);
    public PlayerDto GetPlayer(int id);
}
