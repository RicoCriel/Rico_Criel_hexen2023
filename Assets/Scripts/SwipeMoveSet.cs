using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class SwipeMoveSet : MoveSet
{
    public SwipeMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        var allValidPositions = new List<Position>();

        for (int d = 0; d < 6; d++)
        {
            var validPositions = new List<Position>();

            var direction = Position.Direction(d);

            Position currentPosition = fromPosition.Add(direction);

            while(Board.IsValid(currentPosition) && Board.IsValid(hoverPosition))
            {
                validPositions.Add(currentPosition);
                currentPosition = currentPosition.Add(direction);
            }

            if (validPositions.Contains(hoverPosition))
                return validPositions;

            allValidPositions.AddRange(validPositions);
        }
        return allValidPositions;

    }

    internal override bool Execute(List<Position> positions)
    {
        foreach (var position in positions)
        {
            if(Board.TryGetPieceAt(position, out var piece))
            {
                Board.Take(position);
            }
        }
        return true;
    }


}


