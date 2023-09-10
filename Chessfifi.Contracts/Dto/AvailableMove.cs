namespace Chessfifi.Contracts.Dto;
/// <summary>
/// Move From To
/// </summary>
public class AvailableMove
{
    /// <summary>
    /// Where i am
    /// </summary>
    public Position From { get; set; }

    /// <summary>
    /// where to go
    /// </summary>
    public List<Position> To { get; set; }
}