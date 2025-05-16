using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

internal class BlitzCommand : ICommand
{
    private readonly Board _board;
    private readonly List<Position> _positions;
    private readonly Dictionary<Position, bool> _tileStates = new Dictionary<Position, bool>();
    private readonly List<Position> _deactivatedPiecePositions = new List<Position>();

    public BlitzCommand(Board board, List<Position> positions)
    {
        _board = board;
        _positions = positions;
    }

    public void Execute()
    {
        foreach (var position in _positions)
        {
            _tileStates[position] = _board.IsTileActive(position);

            if (_tileStates[position])
            {
                if (_board.TryGetPieceAt(position, out var piece) && !piece.IsPlayer)
                {
                    _deactivatedPiecePositions.Add(position);
                }

                //Must happen after adding the pieces that are deactivated or the list of positions is empty
                _board.DeactivateTile(position);
            }
        }
    }

    public void Undo()
    {
        foreach (var position in _positions)
        {
            if (_tileStates.TryGetValue(position, out bool wasActive) && wasActive)
            {
                _board.UndoDeactivateTile(position);
            }
        }

        foreach (var piecePosition in _deactivatedPiecePositions)
        {
            _board.UndoTake(piecePosition);
        }
    }
}

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
        //bool pieceRemoved = false;

        //// Filter the positions to only consider enemy positions
        //var enemyPositions = positions
        //    .Where(position => Board.TryGetPieceAt(position, out var piece) && !piece.IsPlayer)
        //    .ToList();

        //if (enemyPositions.Count > 0)
        //{
        //    // Randomly select one enemy position to remove
        //    int randomIndex = UnityEngine.Random.Range(0, enemyPositions.Count);
        //    Position positionToRemove = enemyPositions[randomIndex];

        //    if (Board.IsValid(positionToRemove))
        //    {
        //        Board.Take(positionToRemove);
        //        pieceRemoved = true;
        //    }
        //}

        //return pieceRemoved;
        var command = CreateCommand(positions);
        if (command == null) return false;

        command.Execute();
        return true; 
    }

    internal override ICommand CreateCommand(List<Position> positions)
    {
        if (positions.Count == 0) return null;
        return new BlitzCommand(Board, positions);
    }
}



