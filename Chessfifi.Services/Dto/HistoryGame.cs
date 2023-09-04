using Chessfifi.Common.Enums;

namespace Chessfifi.Services.Dto;
public class HistoryGame
{
    public string Id { get; set; }
    public FinishReason? FinishReason { get; set; }
    public GameSide? WinSide { get; set; }
    public GameMode? GameMode { get; set; }
    public List<MoveDto> Moves { get; set; }
    public string Positions { get; set; }
    public PlayerDto WhitePlayer { get; set; }
    public PlayerDto BlackPlayer { get; set; }
}