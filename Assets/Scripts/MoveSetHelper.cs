using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class MoveSetHelper
{
    private PieceView _currentPiece;
    private readonly Position _currentPosition;
    private readonly Position _hoveredPosition;
    private Board _board;
    private List<Position> _positions = new List<Position>();

    public MoveSetHelper(Board board, Position position, Position hoveredPosition)
    {
        _board = board;
        _currentPosition = position;
        _hoveredPosition = hoveredPosition;

        if (!_board.TryGetPieceAt(_currentPosition, out _currentPiece))
            Debug.Log($"Passed in a position {_currentPosition} which contains no piece to {nameof(MoveSetHelper)}.");
        _hoveredPosition = hoveredPosition;
    }


    //public MoveSetHelper UpQ(int maxSteps = int.MaxValue, params Validator[] condition)
    //=> Collect(new Vector3Int(0,-1,1), maxSteps, condition);

    //public MoveSetHelper UpR(int maxSteps = int.MaxValue, params Validator[] condition)
    //=> Collect(new Vector3Int(1,0,-1), maxSteps, condition);

    //public MoveSetHelper UpS(int maxSteps = int.MaxValue, params Validator[] condition)
    //=> Collect(new Vector3Int(1,-1,0), maxSteps, condition);

    //public MoveSetHelper DownQ(int maxSteps = int.MaxValue, params Validator[] condition)
    //=> Collect(new Vector3Int(0,1,-1), maxSteps, condition);

    //public MoveSetHelper DownR(int maxSteps = int.MaxValue, params Validator[] condition)
    //=> Collect(new Vector3Int(-1,0,1), maxSteps, condition);

    //public MoveSetHelper DownS(int maxSteps = int.MaxValue, params Validator[] condition)
    //=> Collect(new Vector3Int(-1,1,0), maxSteps, condition);

    //public MoveSetHelper Left(int maxSteps = int.MaxValue, params Validator[] condition)
    //=> Collect(new Vector3Int(-1,0,1), maxSteps, condition);

    //public MoveSetHelper Right(int maxSteps = int.MaxValue, params Validator[] condition)
    //=> Collect(new Vector3Int(1,0,-1), maxSteps, condition);



    public delegate bool Validator(Position currentPosition, Board board, Position targetTile);

    public MoveSetHelper Collect(Vector3Int direction,int maxSteps = int.MaxValue, params Validator[] condition)
    {
        if (_currentPiece == null)
            return this;

        var currentStep = 0;

        var position = new Position(_currentPosition.Q + direction.x, _currentPosition.R - direction.y, _currentPosition.S + direction.z);

        while (_board.IsValid(position)
            && (condition == null || condition.All((v) => v(_currentPosition, _board, position)))
            )
        {
            if (_board.TryGetPieceAt(position, out var piece))
            {
                if (piece.IsPlayer != _currentPiece.IsPlayer)
                    _positions.Add(position);

                break;
            }

            _positions.Add(position);

            position = new Position(position.Q + direction.x, position.R - direction.y, position.S + direction.z);
            currentStep++;
        }

        return this;
    }


    public List<Position> ValidPositions()
    {
        return _positions;
    }
}
