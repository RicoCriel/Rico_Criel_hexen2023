using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TeleportCommand : ICommand
{
    private readonly Board _board;
    private readonly Position _toPosition;
    private readonly Position _fromPosition;

    public TeleportCommand(Board board, Position toPosition)
    {
        _board = board;
        _toPosition = toPosition;
        _fromPosition = PositionHelper.GridPosition(board.Playerpiece.Position); 
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

internal class TeleportMoveSet : MoveSet
{
    public TeleportMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        var validPositions = new List<Position>();

        if (!Board.TryGetPieceAt(hoverPosition, out var piece) && Board.IsValid(hoverPosition))
                validPositions.Add(hoverPosition);

        return validPositions;
    }

    internal override bool Execute(List<Position> positions)
    {
        if (positions.Count == 0)
            return false;

        var command = CreateCommand(positions);
        if (command == null)
            return false;

        command.Execute();

        return true;
    }

    internal override ICommand CreateCommand(List<Position> positions)
    {
        if (positions.Count == 0)
            return null;

        var movePosition = positions[0];
        return new TeleportCommand(Board, movePosition);
    }
}

