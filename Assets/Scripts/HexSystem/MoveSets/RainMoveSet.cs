using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class RainCommand: ICommand
{
    private readonly Board _board;
    private readonly Position _targetPosition;
    private bool _isTileActive;
    private PieceView _deactivatedPiece;

    public RainCommand(Board board, Position targetPosition)
    {
        _board = board;
        _targetPosition = targetPosition;
    }

    public void Execute()
    {
        _isTileActive = _board.IsTileActive(_targetPosition);
        _deactivatedPiece = null;

        if (_board.TryGetPieceAt(_targetPosition, out var piece) && !piece.IsPlayer)
        {
            _deactivatedPiece = piece; 
        }

        _board.DeactivateTile(_targetPosition);
    }

    public void Undo()
    {
        if (_isTileActive)
        {
            if (_deactivatedPiece != null)
            {
                _board.UndoTake(_targetPosition);
            }

            _board.UndoDeactivateTile(_targetPosition);
        }
    }
}

internal class RainMoveSet : MoveSet
{
    public RainMoveSet(Board board) : base(board)
    {
    }

    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
    {
        var validPositions = new List<Position>();

        if (Board.IsValid(hoverPosition))
        {
            validPositions.Add(hoverPosition); 
        }

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

        // Takes the first (and only) position from the list
        var positionsList = positions[0];
        return new RainCommand(Board, positionsList);
    }
}



