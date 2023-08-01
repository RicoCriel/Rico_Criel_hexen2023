using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private Board _board;
    private Engine _engine;

    private PieceView _playerPieceView;
    private BoardView _boardView;
    private StateMachine _stateMachine;

    //new
    private InteractionEventArgs _currentHoverEvent;

    public void OnEnable()
    {
        _stateMachine = new StateMachine();
        _stateMachine.Register(States.Start, new MenuState());
        _stateMachine.Register(States.Playing, new PlayState());
        _stateMachine.InitialState = States.Start;

        _board = new Board(4);
        _board.PieceMoved += (s, e) => e.Piece.gameObject.transform.position = PositionHelper.WorldPosition(e.ToPosition);
        _board.PieceTaken += (s, e) => e.Piece.Take();

        _engine = new Engine(_board, _stateMachine);

        _boardView = FindObjectOfType<BoardView>();
        _boardView.CardHovered += CardHovered;
        _boardView.CardDropped += CardDropped;

        var pieceViews = FindObjectsOfType<PieceView>();
        foreach(var pieceView in pieceViews)
        {
            _board.Place(PositionHelper.GridPosition(pieceView.Position), pieceView);
            if (pieceView.IsPlayer)
            {
                _playerPieceView = pieceView;
                _board.Playerpiece = pieceView;
            }
        }
    }

    private void CardDropped(object sender, InteractionEventArgs e)
    {
        if (_board.TryGetPieceAt(e.Position.GridPosition, out var piece))
            Debug.Log($"Found Piece: {piece}");

        Debug.Log($"{e.Card.CardType} dropped on {e.Position.GridPosition} ");

        if (_currentHoverEvent != null &&
            _boardView.ActivatedPositions.Contains(_currentHoverEvent.Position.GridPosition))
        {
            _engine.Drop(
                _currentHoverEvent.Card.CardType,
                _boardView.ActivatedPositions);
        }
    }

    private void CardHovered(object sender, InteractionEventArgs e)
    {
        Debug.Log($"{e.Card.CardType} hovered on {e.Position.GridPosition}");

        _currentHoverEvent = e;

        var positions = _engine.MoveSet.For(
            e.Card.CardType).Positions(
            PositionHelper.GridPosition(_playerPieceView.Position),
            e.Position.GridPosition);

        _boardView.ActivatedPositions = positions;
    }
}
