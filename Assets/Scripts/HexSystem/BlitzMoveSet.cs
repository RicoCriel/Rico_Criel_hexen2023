using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class BlitzMoveSet : MoveSet
{
    public BlitzMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        throw new NotImplementedException();
    }

    internal override bool Execute(List<Position> positions)
    {
        throw new NotImplementedException();
    }
}
