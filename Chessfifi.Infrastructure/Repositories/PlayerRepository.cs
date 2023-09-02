using Chessfifi.Infrastructure.UnitOfWork;

namespace Chessfifi.Domain.ChessAgg;

public class PlayerRepository : IPlayerRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public PlayerRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Player GetPlayer(int id)
    {
        var player = _unitOfWork.Context.Players
            .First(x => x.Id == id);
        return player;
    }

    public Player FindPlayerByUserId(string userId)
    {
        var player = _unitOfWork.Context.Players
            .FirstOrDefault(x => x.UserId == userId);
        return player;
    }

    public Player CreatePlayer(string userId, string name)
    {
        var player = _unitOfWork.Context.Players
            .FirstOrDefault(x => x.UserId == userId);
        if (player != null)
        {
            throw new Exception("user has player");
        }

        player = new Player();
        player.UserId = userId;
        player.Name = name;
        _unitOfWork.Context.Players.Add(player);
        _unitOfWork.Context.SaveChanges();

        return player;
    }
}
