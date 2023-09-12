using Chessfifi.Domain;
using Chessfifi.Domain.PieceTypes;

namespace Bg.Chess.Game.Addon;

/// <summary>
/// Type "Hydra"
/// </summary>
public class Hydra : PieceType
{
    /// </inheritdoc>
    public override bool IsPawnTransformAvailable => true;

    /// </inheritdoc>
    public override string Name => "hydra";

    /// </inheritdoc>
    public override char ShortName => 'h';

    /// </inheritdoc>
    /// 
    protected override List<Chessfifi.Domain.Position> GetBaseMoves(Chessfifi.Domain.Piece piece, MoveMode moveMode)
    {
        var availablePositions = new List<Chessfifi.Domain.Position>();

        AddAvailableDiagonalMoves(piece, availablePositions, moveMode, 2);
        AddAvailableLineMoves(piece, availablePositions, moveMode, 3);

        return availablePositions;
    }
}
