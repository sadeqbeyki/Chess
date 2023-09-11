using ChessDotCore.Engine;
using Chessfifi.Common.Enums;
using Chessfifi.Contracts.Dto;


namespace Chessfifi.Contracts;
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
