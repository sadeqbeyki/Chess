namespace Chessfifi.Contracts.Dto;

/// <summary>
/// Move
/// </summary>
public class MoveDto
{
    /// <summary>
    /// From
    /// </summary>
    public PositionDto From { get; set; }

    /// <summary>
    /// To
    /// </summary>
    public PositionDto To { get; set; }

    /// <summary>
    /// Runner
    /// </summary>
    public PieceDto Runner { get; set; }

    /// <summary>
    /// If after a move an enemy piece dies.
    /// </summary>
    public PieceDto KillEnemy { get; set; }

    /// <summary>
    /// When the king moved, the rook also moved, but this is one.
    /// </summary>
    public MoveDto AdditionalMove { get; set; }
}
