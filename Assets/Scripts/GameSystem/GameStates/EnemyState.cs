using System;
using System.Collections.Generic;
using System.Linq;
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

        var enemyPieceViews = GameObject.FindObjectsOfType<PieceView>()
            .Where(pieceView => !pieceView.IsPlayer)
            .ToList();

        var availablePositions = _board.GetAllPositions()
            .Where(position => _board.IsValid(position) && _board.IsPositionAvailable(position))
            .ToList();

        foreach (var enemyPieceView in enemyPieceViews)
        {
            if (availablePositions.Count == 0)
            {
                Debug.LogWarning("No available positions left for enemy pieces.");
                break;
            }

            // Randomly select a position from the available positions
            int randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
            Position gridPosition = availablePositions[randomIndex];

            // Move the enemy piece to the new position
            _board.Move(enemyPieceView.GridPosition, gridPosition);
            enemyPieceView.MoveTo(gridPosition);

            availablePositions.RemoveAt(randomIndex); // Remove the selected position from available positions
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
                

