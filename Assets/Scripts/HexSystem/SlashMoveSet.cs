using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class SlashMoveSet : MoveSet
{
    public SlashMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        var allValidPositions = new List<Position>();
        var validPositions = new List<Position>();


        for (int d = 0; d < 6; d++)
        {
            var direction = Position.Direction(d);

            Position currentPosition = fromPosition.Add(direction);
            if(Board.IsValid(currentPosition))
            {
                validPositions.Add(currentPosition);
            }
        }
        if (validPositions.Contains(hoverPosition))
        {
            for (int d = 0; d < 6; d++)
            {
                var direction = Position.Direction(d);

                Position currentPosition = hoverPosition.Add(direction);

                if(validPositions.Contains(currentPosition))
                allValidPositions.Add(currentPosition);
                allValidPositions.Add(hoverPosition);
            }
        }
        else
        {
            allValidPositions.AddRange(validPositions);
        }

        return allValidPositions;
    }

    internal override bool Execute(List<Position> positions)
    {
        foreach (var position in positions)
        {
            Board.Take(position);
        }
        return true;
    }
}
