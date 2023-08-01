using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Engine
{
    private readonly Board _board;
    private readonly MoveSetCollection _moveSetCollection;
    private readonly StateMachine _stateMachine;

    public MoveSetCollection MoveSet => _moveSetCollection;

    public Engine(Board board, StateMachine stateMachine)
    {
        _board = board;
        _moveSetCollection = new MoveSetCollection(_board);
        _stateMachine = stateMachine;
    }

    internal bool Drop(CardType cardType, List<Position> positions)
    {
        if (!_board.TryGetPieceAt(PositionHelper.GridPosition(_board.Playerpiece.Position), out var piece))
            return false;

        if(!_moveSetCollection.TryGetMoveSet(cardType, out var moveSet))
            return false;

        if(!moveSet.Execute(positions))
            return false;

        if (!_stateMachine.CurrentState.Equals(States.Playing))
            return false;

        return true;
    }
}
