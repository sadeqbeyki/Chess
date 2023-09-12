using ChessDotCore.Engine;
using Chessfifi.Common.Enums;
using Chessfifi.Domain;
using Chessfifi.Domain.Helpers;
using Chessfifi.Services.Addon;
using Chessfifi.Services.Dto;

namespace Chessfifi.Services;
public interface IGameInfo
{
    string Id { get; }
    PlayerDto WhitePlayer { get; }
    PlayerDto BlackPlayer { get; }
    GameSide StepSide { get; }
    GameMode GameMode { get; }
    bool IsMyGame(int playerId);
    int FieldWidth { get; }
    int FieldHeight { get; }

    bool IsFinish { get; }
    void Move(int playerId, int fromX, int fromY, int toX, int toY, string pawnTransformPiece = null);

    string GetForsythEdwardsNotation(bool onlyPositions = false);

    List<Dto.AvailableMove> AvailableMoves();

    List<Dto.MoveDto> GetMoves();
    void Surrender(int playerId);
    Common.Enums.FinishReason? FinishReason { get; }
    GameSide? WinSide { get; }
}

public class GameInfo : IGameInfo
{
    public string Id { get; private set; }
    public PlayerDto WhitePlayer { get; private set; }
    public PlayerDto BlackPlayer { get; private set; }
    public GameMode GameMode { get; private set; }
    public int FieldWidth => _game.Width;
    public int FieldHeight => _game.Height;

    public bool whiteConfirm;
    public bool blackConfirm;

    private Game _game;

    public GameInfo(PieceTypesDto pieceTypes, string id, GameMode gameMode, PlayerDto whitePlayer, PlayerDto blackPlayer)
    {
        Id = id;
        WhitePlayer = whitePlayer;
        BlackPlayer = blackPlayer;
        GameMode = gameMode;
        _game = new Game();
        if (gameMode == GameMode.Dragon)
        {
            var rules = new DragonRules(pieceTypes);
            var field = new Field(rules);
            _game.Init(field);
        }
        else
        {
            _game.Init();
        }

        // оставлю для отладки особых случаев
        if (false)
        {
            var rules2 = new ClassicRules();
            rules2.Positions = new List<Domain.Position>
            {
                new Domain.Position(4, 6, PieceBuilder.Pawn(Side.White)),
                new Domain.Position(7, 7, PieceBuilder.King(Side.Black)),
                new Domain.Position(0, 0, PieceBuilder.King(Side.White)),
            };
            var field2 = new Field(rules2);
            _game.Init(field2);
        }
    }

    public bool IsMyGame(int playerId)
    {
        return BlackPlayer.Id == playerId || WhitePlayer.Id == playerId;
    }

    /// <summary>
    /// Make a move
    /// </summary>
    /// <param name="playerId">Who is walking</param>
    /// <param name="fromX">from X.</param>
    /// <param name="fromY">from Y.</param>
    /// <param name="toX">to X.</param>
    /// <param name="toY">to Y.</param>
    /// <param name="pawnTransformPiece">The name of a piece to move a pawn to another piece at the end of the field.</param>
    public void Move(int playerId, int fromX, int fromY, int toX, int toY, string pawnTransformPiece = null)
    {
        Side side = GetSide(playerId);

        _game.Move(side, fromX, fromY, toX, toY, pawnTransformPiece);

    }

    private Side GetSide(int playerId)
    {
        Side side;
        if (WhitePlayer.Id == playerId)
        {
            side = Side.White;
        }
        else if (BlackPlayer.Id == playerId)
        {
            side = Side.Black;
        }
        else
        {
            throw new BusinessException("This is not your game");
        }

        return side;
    }

    /// <summary>
    /// Forsyth–Edwards Notation.
    /// </summary>
    /// <param name="onlyPositions">Only the arrangement of figures.</param>
    /// <returns>Arrangement of pieces on the board.</returns>
    public string GetForsythEdwardsNotation(bool onlyPositions = false)
    {
        return _game.GetForsythEdwardsNotation(onlyPositions);
    }

    public List<Dto.AvailableMove> AvailableMoves()
    {
        return _game.AvailableMoves().Select(move =>
        {
            var dto = new Dto.AvailableMove();
            dto.From = new Dto.Position { X = move.From.X, Y = move.From.Y };
            dto.To = move.To.Select(to => new Dto.Position { X = to.X, Y = to.Y }).ToList();
            return dto;
        }).ToList();
    }

    public List<Dto.MoveDto> GetMoves()
    {
        return _game.GetMoves().Select(move =>
        {
            var dto = Init(move);
            return dto;
        }).ToList();
    }

    private Dto.MoveDto Init(Domain.Move move)
    {
        if (move == null)
        {
            return null;
        }

        var dto = new Dto.MoveDto
        {
            From = move.From == null ? null : new Dto.Position { X = move.From.X, Y = move.From.Y },
            To = new Dto.Position { X = move.To.X, Y = move.To.Y },

            AdditionalMove = Init(move.AdditionalMove),
            KillEnemy = FillDtoPiece(move.KillEnemy),
            Runner = FillDtoPiece(move.Runner),
        };

        return dto;
    }

    private Dto.Piece FillDtoPiece(Domain.Piece piece)
    {
        if (piece == null)
        {
            return null;
        }

        return new Dto.Piece
        {
            Side = piece.Side == Side.White ? GameSide.White : GameSide.Black,
            TypeName = piece.Type.Name,
            TypeShortName = piece.Type.ShortName.ToString(),
        };
    }

    public void Surrender(int playerId)
    {
        Side side = GetSide(playerId);
        _game.Surrender(side);
    }

    public GameSide StepSide => _game.StepSide == Side.White ? GameSide.White : GameSide.Black;

    public bool IsFinish => _game.State == GameState.Finish;

    public GameSide? WinSide
    {
        get
        {
            if (_game.WinSide == null)
            {
                return null;
            }

            return _game.WinSide == Side.White ? GameSide.White : GameSide.Black;
        }
    }

    public Common.Enums.FinishReason? FinishReason
    {
        get
        {
            if (_game.FinishReason == null)
            {
                return null;
            }

            switch (_game.FinishReason)
            {
                case Domain.FinishReason.Draw:
                    return Common.Enums.FinishReason.Draw;
                case Domain.FinishReason.Mate:
                    return Common.Enums.FinishReason.Mate;
                case Domain.FinishReason.Surrender:
                    return Common.Enums.FinishReason.Surrender;
                case Domain.FinishReason.TimeOver:
                    return Common.Enums.FinishReason.TimeOver;
                default:
                    throw new Exception("finish reason unrecognized " + _game.FinishReason);
            }
        }
    }
}
