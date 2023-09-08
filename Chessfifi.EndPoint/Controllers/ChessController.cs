using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Chessfifi.Services;
using Chessfifi.Services.Service;
using Chessfifi.EndPoint.Models;
using Chessfifi.Services.Dto;
using static Chessfifi.EndPoint.Models.HistoryGameModel;

namespace Chessfifi.EndPoint.Controllers;
[Authorize]
public class ChessController : BaseController
{
    private readonly ILogger _logger;
    private IGameManager _gameManager;
    private IPlayerService _playerService;
    private IUserService _userService;
    private IGameService _gameService;
    private static object lockObject = new object();

    public ChessController(
        ILoggerFactory loggerFactory,
        IGameManager gameManager,
        IPlayerService playerService,
        IUserService userService,
        IGameService gameService) : base(loggerFactory, userService)
    {
        _logger = loggerFactory.CreateLogger("chess");
        _gameManager = gameManager;
        _playerService = playerService;
        _userService = userService;
        _gameService = gameService;

        // todo asa
        if (!gameManager.IsInit)
        {
            lock (lockObject)
            {
                if (!gameManager.IsInit)
                {
                    var games = _gameService.GetNotFinishGames();
                    gameManager.Init(games);
                }
            }
        }
    }

    [HttpGet]
    public ViewResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _userService.GetUser(userId);

        return View();
    }

    [HttpPost]
    public JsonResult StartSearch(string mode)
    {
        Common.Enums.GameMode gameMode;
        if (mode == "classic")
        {
            gameMode = Common.Enums.GameMode.Classic;
        }
        else if (mode == "dragon")
        {
            gameMode = Common.Enums.GameMode.Dragon;
        }
        else
        {
            throw new Exception("unrecognized mode " + mode);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.FindFirstValue(ClaimTypes.Name);

        var user = _userService.GetUser(userId);
        if (!user.IsEmailConfirmed)
        {
            return Json(new { error = true, message = "Confirm your email before starting the game" });
        }

        var player = _playerService.GetOrCreatePlayerByUserId(userId, userName);
        _gameManager.StartSearch(player, gameMode);
        return Json(new { error = false });
    }

    [HttpPost]
    public JsonResult CheckSearch()
    {
        var playerId = GetPlayerId();
        var status = _gameManager.Check(playerId);
        return Json(new { status = status.ToString() });
    }

    [HttpPost]
    public JsonResult ConfirmSearch()
    {
        var playerId = GetPlayerId();
        var status = _gameManager.Confirm(playerId);
        return Json(new { status = status.ToString() });
    }

    [HttpPost]
    public JsonResult StopSearch()
    {
        var playerId = GetPlayerId();
        _gameManager.StopSearch(playerId);
        return Json(new { error = false });
    }

    [HttpPost]
    public JsonResult GetGame()
    {
        var playerId = GetPlayerId();
        var game = _gameManager.FindMyPlayingGame(playerId);
        return InitFieldResponse(playerId, game);
    }

    [HttpPost]
    public JsonResult Move(int fromX, int fromY, int toX, int toY, string pawnTransformPiece)
    {
        var playerId = GetPlayerId();
        var game = _gameManager.FindMyPlayingGame(playerId);
        game.Move(playerId, fromX, fromY, toX, toY, pawnTransformPiece);
        _gameService.SaveGame(game);

        return InitFieldResponse(playerId, game);
    }

    [HttpPost]
    public JsonResult Surrender()
    {
        var playerId = GetPlayerId();
        var game = _gameManager.FindMyPlayingGame(playerId);
        game.Surrender(playerId);
        _gameService.SaveGame(game);
        return InitFieldResponse(playerId, game);
    }

    private JsonResult InitFieldResponse(int playerId, IGameInfo game)
    {
        if (game == null)
        {
            return Json(new { error = true, message = "Game not found" });
        }

        string notation;
        List<Chessfifi.Services.Dto.AvailableMove> moves = null;
        if (game.IsFinish)
        {
            notation = game.GetForsythEdwardsNotation(true);
        }
        else
        {
            notation = game.GetForsythEdwardsNotation();
            moves = game.AvailableMoves();
        }

        var historyMoves = game.GetMoves();

        var side = game.WhitePlayer.Id == playerId ? "White" : "Black";
        var stepSide = game.StepSide == Chessfifi.Common.Enums.GameSide.White ? "White" : "Black";
        var winSide = game.WinSide == Chessfifi.Common.Enums.GameSide.White ? "White" : "Black";
        var finishReason = "";
        if (game.IsFinish)
        {
            switch (game.FinishReason)
            {
                case Chessfifi.Common.Enums.FinishReason.Mate:
                    finishReason = "Mate";
                    break;
                case Chessfifi.Common.Enums.FinishReason.Surrender:
                    finishReason = "Surrender";
                    break;
                case Chessfifi.Common.Enums.FinishReason.Draw:
                    finishReason = "Draw";
                    break;
                case Chessfifi.Common.Enums.FinishReason.TimeOver:
                    finishReason = "TimeOver";
                    break;
            }
        }

        // todo DTO
        return Json(new
        {
            game.Id,
            EnemyName = game.WhitePlayer.Id == playerId ? game.BlackPlayer.Name : game.WhitePlayer.Name,
            game.FieldWidth,
            game.FieldHeight,
            Notation = notation,
            AvailableMoves = moves,
            HistoryMoves = historyMoves,
            Side = side,
            StepSide = stepSide,
            game.IsFinish,
            FinishReason = finishReason,
            WinSide = winSide,
        });
    }

    private int GetPlayerId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var player = _playerService.FindPlayerByUserId(userId);
        if (player == null)
        {
            throw new BusinessException("احتمالا تا کنون بازی ای نداشته اید");
        }
        return player.Id;
    }

    [HttpGet]
    public ActionResult History()
    {
        var playerId = GetPlayerId();
        var games = _gameService.GetGames(playerId);

        var model = new HistoryModel();
        model.Games = games.Select(x =>
            new HistoryModel.Game
            {
                Id = x.Id,
                BlackPlayer = FillPlayer(x.BlackPlayer),
                WhitePlayer = FillPlayer(x.WhitePlayer),
                FinishReason = x.FinishReason,
                WinSide = x.WinSide,
            }).ToList();
        model.MyPlayerId = playerId;

        HistoryModel.Player FillPlayer(PlayerDto player)
        {
            return new HistoryModel.Player
            {
                Id = player.Id,
                Name = player.Name,
            };
        }

        return View("History", model);
    }

    [HttpGet]
    [Route("/History/{gameId}")]
    public ActionResult HistoryGame(string gameId)
    {
        var game = _gameService.GetGame(gameId);
        var model = new HistoryGameModel
        {
            Id = game.Id,
            BlackPlayer = FillPlayer(game.BlackPlayer),
            WhitePlayer = FillPlayer(game.WhitePlayer),
            FinishReason = game.FinishReason,
            WinSide = game.WinSide,
        };

        PlayerViewModel FillPlayer(PlayerDto player)
        {
            return new PlayerViewModel
            {
                Id = player.Id,
                Name = player.Name,
            };
        }

        model.Moves = game.Moves.Select(x =>
        {
            var dto = FillMove(x);
            dto.AdditionalMove = FillMove(x.AdditionalMove);
            if (x.KillEnemy != null)
            {
                dto.KillEnemy = x.KillEnemy.GetNotation();

            }
            return dto;
        }).ToList();

        model.Positions = game.Positions;
        return View("HistoryGame", model);
    }

    private MoveViewModel FillMove(MoveDto x)
    {
        if (x == null)
        {
            return null;
        }
        return new MoveViewModel()
        {
            Runner = x.Runner.GetNotation(),
            From = x.From == null ? null : new PositionViewModel
            {
                X = x.From.X,
                Y = x.From.Y,
            },
            To = new PositionViewModel
            {
                X = x.To.X,
                Y = x.To.Y,
            },

        };
    }
}
