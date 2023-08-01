using System;
using System.Collections.Generic;


internal abstract class MoveSet : IMoveSet
{
    private Board _board;

    protected Board Board => _board;

    protected MoveSet(Board board)
    {
        _board = board;
    }

    internal abstract bool Execute(List<Position> positions);

    public abstract List<Position> Positions(Position fromPosition, Position hoverPosition);

    public class MoveCommand : ICommand
    {
        Board _board;
        Position _toPosition;
        Position _fromPosition;
        PieceView _takenPiece;
        private bool _pieceTaken;

        public MoveCommand(Board board, Position toPosition, Position fromPosition)
        {
            _board = board;
            _toPosition = toPosition;
            _fromPosition = fromPosition;
        }

        public virtual void Execute()
        {
            _pieceTaken = _board.TryGetPieceAt(_toPosition, out _takenPiece);

            if (_pieceTaken)
            {
                var taken = _board.Take(_toPosition);
                _pieceTaken = _pieceTaken & taken;
            }

            _board.Move(_fromPosition, _toPosition);
        }

        public void Undo()
        {
            _board.Move(_toPosition, _fromPosition);
            if (_pieceTaken)
                _board.Place(_toPosition, _takenPiece);
        }

    }

}
