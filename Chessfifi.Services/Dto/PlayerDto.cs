namespace Chessfifi.Services.Dto;

/// <summary>
/// Игрок.
/// </summary>
public class PlayerDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Ник.
    /// </summary>
    public string Name { get; set; }
}
