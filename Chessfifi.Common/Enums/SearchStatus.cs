namespace Chessfifi.Common.Enums;

/// <summary>
/// Game situation
/// </summary>
public enum SearchStatus
{
    /// <summary>
    /// Not Exist
    /// </summary>
    NotFound = 0,

    /// <summary>
    /// is coming
    /// </summary>
    InProcess = 1,

    /// <summary>
    /// Waiting to apporve
    /// </summary>
    NeedConfirm = 2,

    /// <summary>
    /// Need Confirm Opponent
    /// </summary>
    NeedConfirmOpponent = 3,

    /// <summary>
    /// Ended.
    /// </summary>
    Finish = 4,
}