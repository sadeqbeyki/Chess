namespace Chessfifi.Services.Dto;

/// <summary>
/// Ход.
/// </summary>
public class MoveDto
{
    /// <summary>
    /// Откуда.
    /// </summary>
    public Position From { get; set; }

    /// <summary>
    /// Куда.
    /// </summary>
    public Position To { get; set; }

    /// <summary>
    /// Кто ходил.
    /// </summary>
    public Piece Runner { get; set; }

    /// <summary>
    /// Если после хода умерла вражеская фигура.
    /// </summary>
    public Piece KillEnemy { get; set; }

    /// <summary>
    /// При ходе короля, двигалась и ладья, но это один.
    /// </summary>
    public MoveDto AdditionalMove { get; set; }
}
