// todo заменить Bg.Chess.Domain на Bg.Chess
namespace Chessfifi.Domain.PieceTypes
{
    /// <summary>
    /// Фигура "Ладья"
    /// </summary>
    public class Rook : PieceType
    {
        /// </inheritdoc>
        public override bool IsPawnTransformAvailable => true;

        /// </inheritdoc>
        public override string Name => "rook";

        /// </inheritdoc>
        public override char ShortName => 'r';

        /// </inheritdoc>
        protected override List<Position> GetBaseMoves(Piece piece, MoveMode moveMode)
        {
            var availablePositions = new List<Position>();

            AddAvailableLineMoves(piece, availablePositions, moveMode);

            return availablePositions;
        }
    }
}
