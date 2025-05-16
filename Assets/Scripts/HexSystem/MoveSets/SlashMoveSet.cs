using System;
using System.Collections.Generic;
using System.Linq;

internal class SlashCommand : ICommand
{
    private readonly Board _board;
    private readonly List<Position> _positions;
    private readonly Dictionary<Position, bool> _tileStates = new Dictionary<Position, bool>();
    private readonly List<Position> _deactivatedPiecePositions = new List<Position>();

    public SlashCommand(Board board, List<Position> positions)
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
                    _board.Take(position);
                    _deactivatedPiecePositions.Add(position);
                }
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

        return allValidPositions.Distinct().ToList(); 
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
        return new SlashCommand(Board, positions);
    }
}
