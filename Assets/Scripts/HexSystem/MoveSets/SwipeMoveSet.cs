using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class SwipeCommand : ICommand
{
    private readonly Board _board;
    private readonly List<Position> _positions;
    private readonly Dictionary<Position, bool> _tileStates = new Dictionary<Position, bool>();
    private readonly List<Position> _deactivatedPiecePositions = new List<Position>();

    public SwipeCommand(Board board, List<Position> positions)
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
                    _board.Take(position);
                }
            }
        }
    }

    public void Undo()
    {
        foreach (var piecePosition in _deactivatedPiecePositions)
        {
            _board.UndoTake(piecePosition);
        }
    }
}

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
        var command = CreateCommand(positions);
        if (command == null) return false;

        command.Execute();
        return true; 
    }

    internal override ICommand CreateCommand(List<Position> positions)
    {
        if (positions.Count == 0) return null;
        return new SwipeCommand(Board, positions);
    }
}


