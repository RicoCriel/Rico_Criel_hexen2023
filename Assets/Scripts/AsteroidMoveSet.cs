using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

internal class AsteroidMoveSet : MoveSet
{
    public AsteroidMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        var validPositions = new List<Position>();
        var allValidPositions = new List<Position>();

        if (Board.IsValid(hoverPosition))
            validPositions.Add(hoverPosition);

        for (int d = 0; d < 6; d++)
        {
            var direction = Position.Direction(d);

            Position currentPosition = hoverPosition.Add(direction);
            if (Board.IsValid(currentPosition))
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

                if (validPositions.Contains(currentPosition))
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
            if(Board.TryGetPieceAt(position, out var piece))
            {
                if (!piece.IsPlayer && Board.IsValid(position))
                {
                    return Board.Take(position);
                }
            }
        }
        return true;
    }
}
