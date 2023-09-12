namespace Chessfifi.Services.Dto;

/// <summary>
/// Move
/// </summary>
public class MoveDto
{
    /// <summary>
    /// From
    /// </summary>
    public Position From { get; set; }

    /// <summary>
    /// To
    /// </summary>
    public Position To { get; set; }

    /// <summary>
    /// Runner
    /// </summary>
    public Piece Runner { get; set; }

    /// <summary>
    /// If after a move an enemy piece dies.
    /// </summary>
    public Piece KillEnemy { get; set; }

    /// <summary>
    /// When the king moved, the rook also moved, but this is one.
    /// </summary>
    public MoveDto AdditionalMove { get; set; }
}
