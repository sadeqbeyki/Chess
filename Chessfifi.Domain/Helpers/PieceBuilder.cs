using Chessfifi.Domain.PieceTypes;
using System.Reflection;

namespace Chessfifi.Domain.Helpers
{
    // need to made addOn
    public static class PieceBuilder
    {
        public static Piece King(Side side)
        {
            return new Piece(side, PieceTypes["king"]);
        }
        public static Piece Bishop(Side side)
        {
            return new Piece(side, PieceTypes["bishop"]);
        }
        public static Piece Knight(Side side)
        {
            return new Piece(side, PieceTypes["knight"]);
        }
        public static Piece Pawn(Side side)
        {
            return new Piece(side, PieceTypes["pawn"]);
        }
        public static Piece Queen(Side side)
        {
            return new Piece(side, PieceTypes["queen"]);
        }
        public static Piece Rook(Side side)
        {
            return new Piece(side, PieceTypes["rook"]);
        }

        private static Dictionary<string, PieceType> _pieceTypes;

        /// <summary>
        /// Type of piece Dic
        /// </summary>
        public static Dictionary<string, PieceType> PieceTypes
        {
            get
            {
                if (_pieceTypes == null)
                {
                    _pieceTypes = new Dictionary<string, PieceType>();
                    var ourtype = typeof(PieceType);
                    var list = Assembly.GetAssembly(ourtype).GetTypes()
                        .Where(type => type.IsSubclassOf(ourtype));

                    foreach (var itm in list)
                    {
                        var type = (PieceType)Activator.CreateInstance(itm);
                        PieceTypes.Add(type.Name, type);
                    }
                }

                return _pieceTypes;
            }
        }
    }
}
