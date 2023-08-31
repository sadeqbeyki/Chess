namespace Chessfifi.Common.Enums;

/// <summary>
/// why game finished ?
/// </summary>
public enum FinishReason
{
    /// <summary>
    /// Winning (MAT).
    /// </summary>
    Mate = 0,

    /// <summary>
    /// Withdrawal of one of the players
    /// </summary>
    Surrender = 1,

    /// <summary>
    /// Time Out
    /// </summary>
    TimeOver = 2,

    /// <summary>
    /// PAT
    /// </summary>
    Draw = 3,

    /// <summary>
    /// Lottery
    /// </summary>
    DrawByAgreement = 4,

    /// <summary>
    /// Timely Lottery
    /// </summary>
    DrawByTime = 5,
}
