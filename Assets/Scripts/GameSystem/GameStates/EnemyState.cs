using System;
using System.Collections.Generic;
using UnityEngine;

internal class EnemyState: State
{
    private readonly Board _board;

    public EnemyState(Board board)
    {
        _board = board;
    }

    public override void OnEnter()
    {
        Debug.Log("EnemyState Active");

        // Clear the old piece positions from the board
        _board.ClearBoard();

        var pieceViews = GameObject.FindObjectsOfType<PieceView>();

        foreach (var pieceView in pieceViews)
        {
            if (!pieceView.IsPlayer)
            {
                var gridPosition = PositionHelper.GetRandomGridPosition(_board.Radius);
                _board.Place(gridPosition, pieceView);
                pieceView.MoveTo(gridPosition);
                StateMachine.MoveTo(States.Player);
            }
        }
    }

    public override void OnExit()
    {
        Debug.Log("MovingStates");
    }

}

    


    

    //if (pieceView.IsPlayer)
    //{
    //    _playerPieceView = pieceView;
    //    _board.Playerpiece = pieceView;
    //}
                

