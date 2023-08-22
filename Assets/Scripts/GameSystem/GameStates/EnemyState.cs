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

        var pieceViews = GameObject.FindObjectsOfType<PieceView>();

        foreach (var pieceView in pieceViews)
        {
            if (!pieceView.IsPlayer)
            {
                // Generate a random grid position
                Position gridPosition;
                do
                {
                    gridPosition = PositionHelper.GetRandomGridPosition(_board.Radius);
                }
                while (_board.IsPositionOccupied(gridPosition)); // Check if position is occupied

                // Check if the current piece is on the board and is not the player's piece
                if (_board.TryGetPieceAt(pieceView.GridPosition, out var existingPiece) && !existingPiece.IsPlayer)
                {
                    // Move the existing piece to the new position
                    _board.Move(pieceView.GridPosition, gridPosition);
                    pieceView.MoveTo(gridPosition);
                }
            }
        }

        StateMachine.MoveTo(States.Player); // Move to Player state after moving existing pieces
    }






    public override void OnExit()
    {
        // Clear the old piece positions from the board
        //_board.ClearBoard();
        Debug.Log("MovingStates");
    }

}

    


    

    //if (pieceView.IsPlayer)
    //{
    //    _playerPieceView = pieceView;
    //    _board.Playerpiece = pieceView;
    //}
                

