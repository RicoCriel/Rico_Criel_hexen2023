using System;
using System.Collections.Generic;

internal class RingCommand : ICommand
{
    private readonly Board _board;
    private readonly List<Position> _positions;
    private readonly Dictionary<Position, bool> _tileStates = new Dictionary<Position, bool>();
    private readonly List<Position> _deactivatedPiecePositions = new List<Position>();

    public RingCommand(Board board, List<Position> positions)
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
        return new RingCommand(Board, positions);
    }
}








