using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


internal class AsteroidCommand : ICommand
{
    private readonly Board _board;
    private readonly List<Position> _positions;
    private readonly Dictionary<Position, bool> _tileStates = new Dictionary<Position, bool>();
    private readonly List<Position> _deactivatedPiecePositions = new List<Position>();

    public AsteroidCommand(Board board, List<Position> positions)
    {
        _board = board;
        _positions = positions;
    }

    public void Execute()
    {
        foreach (var position in _positions)
        {
            // Store tile state
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


internal class AsteroidMoveSet : MoveSet
{
    public AsteroidMoveSet(Board board) : base(board) { }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        var validPositions = new List<Position>();

        if (Board.IsValid(hoverPosition))
        {
            validPositions.Add(hoverPosition);

            // Add adjacent positions
            for (int d = 0; d < 6; d++)
            {
                var direction = Position.Direction(d);
                var currentPosition = hoverPosition.Add(direction);
                if (Board.IsValid(currentPosition))
                {
                    validPositions.Add(currentPosition);
                }
            }
        }

        return validPositions.Distinct().ToList(); // Ensure no duplicates
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
        return new AsteroidCommand(Board, positions);
    }
}
