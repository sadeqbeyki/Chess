using Chessfifi.Domain.ChessAgg;
using Chessfifi.Services.Dto;
using Chessfifi.Services.Service.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chessfifi.Services.Service;
public interface IGameService
{
    public void SaveGame(IGameInfo game);
    public HistoryGame GetGame(string gameId);
    public IEnumerable<HistoryGame> GetGames(int playerId);
    List<IGameInfo> GetNotFinishGames();
}

public class GameService : IGameService
{
    private IGameRepository _gameRepository;
    private IPlayerService _playerService;
    private IGameManager _gameManager;
    private PieceTypesDto _pieceTypes;
    private ILogger _logger;

    public GameService(
        IGameManager gameManager,
        IPlayerService playerService,
        IGameRepository gameRepository,
        PieceTypesDto pieceTypes,
        ILoggerFactory loggerFactory)
    {
        _gameRepository = gameRepository;
        _playerService = playerService;
        _gameManager = gameManager;
        _pieceTypes = pieceTypes;
        // todo make LogSource a set of constants
        _logger = loggerFactory.CreateLogger("chess");
    }

    public void SaveGame(IGameInfo game)
    {
        var gameDto = new SaveGameDtoV1();
        FillDtoV1(gameDto, game);

        string data = JsonConvert.SerializeObject(gameDto);
        _gameRepository.SaveGame(game.Id,
            game.WhitePlayer.Id,
            game.BlackPlayer.Id,
            game.FinishReason,
            game.WinSide,
            game.GameMode,
            data);
    }

    public HistoryGame GetGame(string gameId)
    {
        var game = _gameRepository.GetGame(gameId);
        var gameDto = JsonConvert.DeserializeObject<SaveGameDtoV1>(game.Data);
        var gameInfo = new HistoryGame();
        gameInfo.Id = gameId;
        gameInfo.WhitePlayer = _playerService.GetPlayer(game.WhitePlayerId);
        gameInfo.BlackPlayer = _playerService.GetPlayer(game.BlackPlayerId);

        gameInfo.FinishReason = (Common.Enums.FinishReason?)game.FinishReason;
        gameInfo.WinSide = (Common.Enums.GameSide?)game.WinSide;
        gameInfo.GameMode = (Common.Enums.GameMode?)game.GameMode;

        FillGameFromDtoV1(gameInfo, gameDto);
        return gameInfo;
    }

    public IEnumerable<HistoryGame> GetGames(int playerId)
    {
        var games = _gameRepository.GetGames(playerId);

        return games.Select(x => new HistoryGame
        {
            Id = x.LogicalName,
            BlackPlayer = new Dto.PlayerDto { Id = x.BlackPlayerId },
            WhitePlayer = new Dto.PlayerDto { Id = x.WhitePlayerId },
            FinishReason = (Common.Enums.FinishReason?)x.FinishReason,
            GameMode = (Common.Enums.GameMode?)x.GameMode,
            WinSide = (Common.Enums.GameSide?)x.WinSide,
        }).ToList();
    }

    public void FillDtoV1(SaveGameDtoV1 dto, IGameInfo game)
    {
        dto.Moves = game.GetMoves().Select(x =>
        {
            var dto = FillDtoMove(x);
            dto.AdditionalMove = FillDtoMove(x.AdditionalMove);
            if (x.KillEnemy != null)
            {
                dto.KillEnemy = x.KillEnemy.GetNotation();

            }

            return dto;
        }).ToList();

        var positionsStr = game.GetForsythEdwardsNotation(true);
        dto.Positions = positionsStr;
    }

    private SaveGameDtoV1.Move FillDtoMove(MoveDto move)
    {
        if (move == null)
        {
            return null;
        }

        return new SaveGameDtoV1.Move()
        {
            Runner = move.Runner.GetNotation(),
            From = move.From == null ? null : new SaveGameDtoV1.Position
            {
                X = move.From.X,
                Y = move.From.Y,
            },
            To = new SaveGameDtoV1.Position
            {
                X = move.To.X,
                Y = move.To.Y,
            },

        };
    }

    private void FillGameFromDtoV1(HistoryGame game, SaveGameDtoV1 dto)
    {
        game.Moves = dto.Moves.Select(x =>
        {
            var dto = FillMove(x);
            dto.AdditionalMove = FillMove(x.AdditionalMove);
            if (x.KillEnemy != null)
            {
                dto.KillEnemy = GetPieceByNotation(x.KillEnemy);

            }
            return dto;
        }).ToList();

        game.Positions = dto.Positions;
    }

    private Piece GetPieceByNotation(string piece)
    {
        var toUpper = piece.ToUpper();

        var type = GetTypeByChar(piece.ToLower().ToCharArray()[0]);
        var side = piece == toUpper ? Common.Enums.GameSide.White : Common.Enums.GameSide.Black;
        return new Piece
        {
            TypeName = type,
            TypeShortName = piece,
            Side = side,
        };
    }

    private string GetTypeByChar(char piece)
    {
        var type = _pieceTypes.Value.Select(x => x.Value).FirstOrDefault(x => x.ShortName == piece);
        if (type == null)
        {
            throw new Exception("type not recognized: " + piece);
        }
        return type.Name;
    }


    private MoveDto FillMove(SaveGameDtoV1.Move move)
    {
        if (move == null)
        {
            return null;
        }

        return new MoveDto()
        {
            Runner = GetPieceByNotation(move.Runner),
            From = move.From == null ? null : new Position
            {
                X = move.From.X,
                Y = move.From.Y,
            },
            To = new Position
            {
                X = move.To.X,
                Y = move.To.Y,
            },

        };
    }

    public List<IGameInfo> GetNotFinishGames()
    {
        var games = new List<IGameInfo>();
        var dbGames = _gameRepository.GetNotFinishGames();

        var playerIds = dbGames.Select(x => x.WhitePlayerId).Distinct().ToList();
        playerIds.AddRange(dbGames.Select(x => x.BlackPlayerId).Distinct().ToList());
        var players = playerIds.ToDictionary(x => x, x => _playerService.GetPlayer(x));
        foreach (var dbGame in dbGames)
        {
            var game = new GameInfo(_pieceTypes, dbGame.LogicalName, (Common.Enums.GameMode)dbGame.GameMode, players[dbGame.WhitePlayerId], players[dbGame.BlackPlayerId]);
            var gameDto = JsonConvert.DeserializeObject<SaveGameDtoV1>(dbGame.Data);
            var gameInfo = new HistoryGame();
            FillGameFromDtoV1(gameInfo, gameDto);
            for (int i = 0; i < gameInfo.Moves.Count; i++)
            {
                var move = gameInfo.Moves[i];

                var pawnTransformPiece =
                    move.Runner.TypeName == "pawn" || move.Runner.TypeName == "soldier" ?
                    move.AdditionalMove?.Runner.TypeName.ToLower()
                    : null;
                game.Move(i % 2 == 0 ? dbGame.WhitePlayerId : dbGame.BlackPlayerId, move.From.X, move.From.Y, move.To.X, move.To.Y, pawnTransformPiece);
            }
            games.Add(game);
        }

        return games;
    }
}
