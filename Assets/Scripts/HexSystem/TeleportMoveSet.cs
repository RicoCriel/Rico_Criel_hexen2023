using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class TeleportMoveSet : MoveSet
{
    public TeleportMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        var validPositions = new List<Position>();

        if (!Board.TryGetPieceAt(hoverPosition, out var piece) && Board.IsValid(hoverPosition))
                validPositions.Add(hoverPosition);

        return validPositions;
    }

    internal override bool Execute(List<Position> positions)
    {
        return Board.Move(PositionHelper.GridPosition(Board.Playerpiece.Position),positions[0]);
    }
}

