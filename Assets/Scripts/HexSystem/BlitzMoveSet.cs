using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class BlitzMoveSet : MoveSet
{
    private const int MaxHighlightedEnemies = 4;

    public BlitzMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        var validPositions = new List<Position>();
        var enemyPositions = new List<Position>();

        // Find enemy positions within a radius of 2 around the hoverPosition
        for (int q = -2; q <= 2; q++)
        {
            for (int r = Math.Max(-2, -q - 2); r <= Math.Min(2, -q + 2); r++)
            {
                int s = -q - r;
                var position = new Position(q, r, s);

                if (Board.IsValid(position) && Board.TryGetPieceAt(position, out var piece) && !piece.IsPlayer)
                {
                    enemyPositions.Add(position);
                }
            }
        }

        // Randomly select one enemy position to highlight
        if (enemyPositions.Count > 0)
        {
            var randomIndex = UnityEngine.Random.Range(0, enemyPositions.Count);
            validPositions.Add(enemyPositions[randomIndex]);

            // Highlight up to 3 additional random enemy positions
            int maxAdditionalHighlight = Math.Min(MaxHighlightedEnemies - 1, enemyPositions.Count - 1);
            var additionalHighlighted = new HashSet<int> { randomIndex };

            while (additionalHighlighted.Count < maxAdditionalHighlight)
            {
                var randomAdditionalIndex = UnityEngine.Random.Range(0, enemyPositions.Count);
                if (!additionalHighlighted.Contains(randomAdditionalIndex))
                {
                    additionalHighlighted.Add(randomAdditionalIndex);
                    validPositions.Add(enemyPositions[randomAdditionalIndex]);
                }
            }
        }

        return validPositions;
    }

    internal override bool Execute(List<Position> positions)
    {
        bool pieceRemoved = false;

        // Filter the positions to only consider enemy positions
        var enemyPositions = positions
            .Where(position => Board.TryGetPieceAt(position, out var piece) && !piece.IsPlayer)
            .ToList();

        if (enemyPositions.Count > 0)
        {
            // Randomly select one enemy position to remove
            int randomIndex = UnityEngine.Random.Range(0, enemyPositions.Count);
            Position positionToRemove = enemyPositions[randomIndex];

            if (Board.IsValid(positionToRemove))
            {
                Board.Take(positionToRemove);
                pieceRemoved = true;
            }
        }

        return pieceRemoved;
    }



    //internal override bool Execute(List<Position> positions)
    //{
    //    bool piecesRemoved = false;

    //    foreach (var position in positions)
    //    {
    //        if (Board.TryGetPieceAt(position, out var piece))
    //        {
    //            if (!piece.IsPlayer && Board.IsValid(position))
    //            {
    //                Board.Take(position);
    //                piecesRemoved = true;
    //            }
    //        }
    //    }

    //    return piecesRemoved;
    //}
}



