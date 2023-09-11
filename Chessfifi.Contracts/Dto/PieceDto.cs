using Chessfifi.Common.Enums;

namespace Chessfifi.Contracts.Dto;

public class PieceDto
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
