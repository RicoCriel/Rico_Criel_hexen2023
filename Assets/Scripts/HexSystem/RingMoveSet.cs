using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class RingMoveSet : MoveSet
{
    public RingMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        int ringRadius = fromPosition.Distance(hoverPosition);

        List<Position> positionsInRing = CalculateRingPositions(fromPosition, ringRadius);

        return positionsInRing;
    }

    internal override bool Execute(List<Position> positions)
    {
        bool piecesRemoved = false;

        foreach (var position in positions)
        {
            if (Board.TryGetPieceAt(position, out var piece) && piece != null && !piece.IsPlayer)
            {
                Board.Take(position);
                piecesRemoved = true;
            }
        }

        return piecesRemoved;
    }

    private List<Position> CalculateRingPositions(Position center, int radius)
    {
        List<Position> positionsInRing = new List<Position>();

        if (radius == 0)
        {
            positionsInRing.Add(center);
            return positionsInRing;
        }

        Position currentPosition = center.Add(Position.Direction(4).Scale(radius));

        for (int d = 0; d < 6; d++)
        {
            for (int i = 0; i < radius; i++)
            {
                if (Board.IsValid(currentPosition))
                {
                    positionsInRing.Add(currentPosition);
                }
                currentPosition = currentPosition.Neighbor(d);
            }
        }

        return positionsInRing;
    }
}








