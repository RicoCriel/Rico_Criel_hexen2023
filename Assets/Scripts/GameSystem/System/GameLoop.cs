using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private Board _board;
    private CommandManager _commandManager;
    private Engine _engine;

    private PieceView _playerPieceView;
    private BoardView _boardView;
    [SerializeField] private int _boardRadius;

    public void OnEnable()
    {
        _board = new Board(_boardRadius);
        _board.PieceMoved += (s, e) => e.Piece.gameObject.transform.position = PositionHelper.WorldPosition(e.ToPosition);
        _board.PieceUndoMove += (s,e) => e.Piece.gameObject.transform.position = PositionHelper.WorldPosition(e.ToPosition);
        _board.PieceTaken += (s, e) => e.Piece?.Take();
        _board.PieceUndoTake += (s, e) => e.Piece?.Undo();

        //TO DO: Create statemachine 
        _commandManager = new CommandManager();
        _engine = new Engine(_board, _commandManager);

        _boardView = FindObjectOfType<BoardView>();
        _boardView.CardHovered += CardHovered;
        _boardView.CardDropped += CardDropped;

        _board.OnTileReactivated += HandleTileEnable;
        _board.OnTileDeactivated += HandleTileDisable;

        ResetGameState();

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

    private void HandleTileDisable(Position position)
    {
        if (_boardView != null && _boardView.TryGetPositionView(position, out var positionView))
        {
            positionView.Deactivate();
            positionView.Disable();
        }
    }

    private void HandleTileEnable(Position position)
    {
        if (_boardView.TryGetPositionView(position, out var positionView))
        {
            positionView.Enable();
        }
    }

    private void CardDropped(object sender, InteractionEventArgs e)
    {
        if (e != null &&
            _boardView.ActivatedPositions.Contains(e.Position.GridPosition))
        {
            _engine.Drop(
                    e.Card.CardType,
                    _boardView.ActivatedPositions);
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
    }

    public void UndoLastMove()
    {
        _commandManager?.Previous();
        _boardView.ActivatedPositions = null;
    }

}
