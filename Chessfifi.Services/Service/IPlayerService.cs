using Chessfifi.Domain.ChessAgg;
using Chessfifi.Services.Dto;

namespace Chessfifi.Services.Service;
public interface IPlayerService
{
    public PlayerDto FindPlayerByUserId(string userId);
    public PlayerDto GetOrCreatePlayerByUserId(string userId, string name);
    public PlayerDto GetPlayer(int id);
}

public class PlayerService : IPlayerService
{
    private IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepo)
    {
        _playerRepository = playerRepo;
    }

    public PlayerDto FindPlayerByUserId(string userId)
    {
        var dbPlayer = _playerRepository.FindPlayerByUserId(userId);
        if (dbPlayer == null)
        {
            return null;
        }
        return FillPlayerDto(dbPlayer);
    }

    public PlayerDto GetPlayer(int id)
    {
        var dbPlayer = _playerRepository.GetPlayer(id);
        return FillPlayerDto(dbPlayer);
    }

    public PlayerDto GetOrCreatePlayerByUserId(string userId, string name)
    {
        var dbPlayer = _playerRepository.FindPlayerByUserId(userId);
        if (dbPlayer == null)
        {
            dbPlayer = _playerRepository.CreatePlayer(userId, name);
        }

        return FillPlayerDto(dbPlayer);
    }

    private PlayerDto FillPlayerDto(Player dbPlayer)
    {
        PlayerDto playerDto = new()
        {
            Id = dbPlayer.Id,
            UserId = dbPlayer.UserId,
            Name = dbPlayer.Name
        };
        return playerDto;
    }
}
