using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class RainMoveSet : MoveSet
{
    public RainMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        var validPositions = new List<Position>();

        if (Board.IsValid(hoverPosition))
        {
            if (!Board.TryGetPieceAt(hoverPosition, out var piece))
            {
                validPositions.Add(hoverPosition);
            }
            else
            {
                validPositions.Add(hoverPosition); // You might want to decide if this is valid or not
            }
        }

        return validPositions;
    }

    internal override bool Execute(List<Position> positions)
    {
        if (positions.Count == 0)
            return false;

        var targetPosition = positions[0];

        Board.DeactivateTile(targetPosition);

        return true; // Return true if the execution was successful
    }
}

