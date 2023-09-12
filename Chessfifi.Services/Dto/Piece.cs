using Chessfifi.Common.Enums;

namespace Chessfifi.Services.Dto;

public class Piece
{
    public GameSide Side { get; set; }

    public string TypeShortName { get; set; }

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
