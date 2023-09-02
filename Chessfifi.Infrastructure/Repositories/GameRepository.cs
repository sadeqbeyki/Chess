using Chessfifi.Common.Enums;
using Chessfifi.Domain.ChessAgg;
using Chessfifi.Infrastructure.UnitOfWork;

namespace Chessfifi.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public GameRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Game GetGame(string gameId)
    {
        var dbGame = _unitOfWork.Context.Games.FirstOrDefault(x => x.LogicalName == gameId);
        return dbGame;
    }

    public List<Game> GetGames(int playerId)
    {
        var dbGames = _unitOfWork.Context.Games
            .Where(x => x.BlackPlayerId == playerId || x.WhitePlayerId == playerId).OrderByDescending(x => x.Id).ToList();
        return dbGames;
    }

    public List<Game> GetNotFinishGames()
    {
        return _unitOfWork.Context.Games.Where(x => x.FinishReason == null).ToList();
    }

    public void SaveGame(string id, int whitePlayerId, int blackPlayerId, FinishReason? finishReason, GameSide? winSide, GameMode gameMode, string data)
    {
        var dbGame = _unitOfWork.Context.Games.FirstOrDefault(x => x.LogicalName == id);
        if (dbGame == null)
        {
            dbGame = new Game();
            _unitOfWork.Context.Games.Add(dbGame);
        }

        dbGame.GameMode = (int)gameMode;
        dbGame.LogicalName = id;
        dbGame.WhitePlayerId = whitePlayerId;
        dbGame.BlackPlayerId = blackPlayerId;
        dbGame.FinishReason = (int?)finishReason;
        dbGame.WinSide = (int?)winSide;
        dbGame.Data = data;
        _unitOfWork.Context.SaveChanges();
    }


}
