namespace Chessfifi.EndPoint.Models.Common;
/// <summary>
/// Пользователь.
/// </summary>
public class UserViewModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Почта подтверждена.
    /// </summary>
    public bool IsEmailConfirmed { get; set; }
}
