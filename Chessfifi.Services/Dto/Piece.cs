using Chessfifi.Common.Enums;

namespace Chessfifi.Services.Dto;

/// <summary>
/// Фигура.
/// </summary>
public class Piece
{
    /// <summary>
    /// Сторона.
    /// </summary>
    public GameSide Side { get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    public string TypeShortName { get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    public string TypeName { get; set; }

    public string GetNotation()
    {
        if (Side == GameSide.White)
        {
            return TypeShortName.ToUpper();
        }

        return TypeShortName;
    }
}
