using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

internal class PushbackCommand : ICommand
{
    private readonly Board _board;
    private readonly Position _fromPosition;
    private readonly Position _toPosition;

    public PushbackCommand(Board board, Position fromPosition, Position toPosition)
    {
        _board = board;
        _fromPosition = fromPosition;
        _toPosition = toPosition;
    }

    public void Execute()
    {
        _board.Move(_fromPosition, _toPosition);
    }

    public void Undo()
    {
        _board.UndoMove(_toPosition, _fromPosition);
    }
}

internal class PushbackMoveSet : MoveSet
{
    public PushbackMoveSet(Board board) : base(board)
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
                {
                    allValidPositions.Add(currentPosition);
                    allValidPositions.Add(hoverPosition);
                }
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
        var command = CreateCommand(positions);
        if (command == null) return false;

        command.Execute();
        return true;
    }

    internal override ICommand CreateCommand(List<Position> positions)
    {
        foreach (var position in positions)
        {

            if (Board.TryGetPieceAt(position, out PieceView piece))
            {
                Position difference = position.Subtract(PositionHelper.GridPosition(Board.Playerpiece.Position));
                Position toPosition = position.Add(difference);
                if (!Board.TryGetPieceAt(toPosition, out PieceView piec) && Board.IsValid(toPosition))
                {
                    Board.Move(position, toPosition);
                    piece.MoveTo(toPosition);
                }
                return new PushbackCommand(Board, position, toPosition);
            }
        }

        return null;
    }


}

