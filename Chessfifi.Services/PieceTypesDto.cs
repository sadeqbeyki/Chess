using System.Reflection;
using Chessfifi.Domain;
using Chessfifi.Domain.Helpers;
using Chessfifi.Domain.PieceTypes;

namespace Chessfifi.Services;
public class PieceTypesDto
{
    public PieceTypesDto()
    {
        Value = PieceBuilder.PieceTypes;

        var _pieceTypes = new Dictionary<string, PieceType>();
        var ourtype = typeof(PieceType);

        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type itm in a.GetTypes().Where(type => type.IsSubclassOf(ourtype)))
            {
                var type = (PieceType)Activator.CreateInstance(itm);
                _pieceTypes.Add(type.Name, type);
            }
        }

        Value = _pieceTypes;
    }

    public Piece King(Side side)
    {
        return new Piece(side, Value["king"]);
    }
    public Piece Bishop(Side side)
    {
        return new Piece(side, Value["bishop"]);
    }
    public Piece Knight(Side side)
    {
        return new Piece(side, Value["knight"]);
    }
    public Piece Pawn(Side side)
    {
        return new Piece(side, Value["pawn"]);
    }
    public Piece Queen(Side side)
    {
        return new Piece(side, Value["queen"]);
    }
    public Piece Rook(Side side)
    {
        return new Piece(side, Value["rook"]);
    }
    public Piece Dragon(Side side)
    {
        return new Piece(side, Value["dragon"]);
    }
    public Piece Soldier(Side side)
    {
        return new Piece(side, Value["soldier"]);
    }
    public Piece Hydra(Side side)
    {
        return new Piece(side, Value["hydra"]);
    }

    public Dictionary<string, PieceType> Value { get; }
}
