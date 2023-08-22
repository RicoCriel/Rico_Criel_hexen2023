using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private Board _board;
    private Engine _engine;

    private PieceView _playerPieceView;
    private BoardView _boardView;
    private StateMachine _stateMachine;

    public void OnEnable()
    {
        _board = new Board(4);
        _board.PieceMoved += (s, e) => e.Piece.gameObject.transform.position = PositionHelper.WorldPosition(e.ToPosition);
        _board.PieceTaken += (s, e) => e.Piece?.Take(); //checks if the piece is not null and then executes take

        //comment this out and uncomment the commented code to return to logic without statemachine design
        _stateMachine = new StateMachine();
        _stateMachine.Register(States.Player, new PlayerState(_board));
        _stateMachine.Register(States.Enemy, new EnemyState(_board));
        _stateMachine.InitialState = States.Player;

        _engine = new Engine(_board);

        _boardView = FindObjectOfType<BoardView>();
        _boardView.CardHovered += CardHovered;
        _boardView.CardDropped += CardDropped;

        ResetGameState();

        //uncomment if randomisation is not needed
        var pieceViews = FindObjectsOfType<PieceView>();
        foreach (var pieceView in pieceViews)
        {
            var gridPosition = PositionHelper.GridPosition(pieceView.Position);
            _board.Place(gridPosition, pieceView); // Use gridPosition when placing
            pieceView.GridPosition = gridPosition; // Set the grid position in the PieceView
            if (pieceView.IsPlayer)
            {
                _playerPieceView = pieceView;
                _board.Playerpiece = pieceView;
            }
        }
    }

    private void CardDropped(object sender, InteractionEventArgs e)
    {
        if (e != null &&
            _boardView.ActivatedPositions.Contains(e.Position.GridPosition))
        {
            //_engine.Drop(
            //        e.Card.CardType,
            //        _boardView.ActivatedPositions);

            if (_stateMachine.CurrentStateName == States.Player)
            {
                //remove condition if not needed
                _engine.Drop(
                    e.Card.CardType,
                    _boardView.ActivatedPositions);

                _stateMachine.MoveTo(States.Enemy);
            }
        }
    }

    private void CardHovered(object sender, InteractionEventArgs e)
    {
        var positions = _engine.MoveSet.For(
            e.Card.CardType).Positions(
            PositionHelper.GridPosition(_playerPieceView.Position),
            e.Position.GridPosition);

        _boardView.ActivatedPositions = positions;
    }


    private void ResetGameState()
    {
        _board.ClearBoard();  // Clear the board by removing all pieces
        _boardView.ActivatedPositions = null;  // Clear any activated positions
        _stateMachine.InitialState = States.Player;  // Reset the state machine to the starting state
    }

}
