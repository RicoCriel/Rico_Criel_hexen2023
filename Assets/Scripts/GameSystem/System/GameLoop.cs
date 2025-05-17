using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class GameLoop : MonoBehaviour
{
    private Board _board;
    private CommandManager _commandManager;
    private StateMachine _stateMachine;
    private Engine _engine;
    private ButtonView _buttonView;
    private PieceView _playerPieceView;
    private BoardView _boardView;

    [SerializeField] private int _boardRadius;

    private Coroutine _delayGameOverTransition;
    private AudioManager _audioManager;
    private VfxManager _vfxManager;

    public void OnEnable()
    {
        _board = new Board(_boardRadius);
        _audioManager = FindObjectOfType<AudioManager>();
        _vfxManager = FindObjectOfType<VfxManager>();
        _audioManager?.InitializeWithBoard(_board);

        _board.PieceTaken += (s, e) => CheckGameOverCondition();
        _board.PieceUndoTake += (s, e) => CheckGameOverCondition();
        _board.PieceMoved += (s, e) => e.Piece.gameObject.transform.position = PositionHelper.WorldPosition(e.ToPosition);
        _board.PieceUndoMove += (s, e) => e.Piece.gameObject.transform.position = PositionHelper.WorldPosition(e.ToPosition);
        _board.PieceTaken += (s, e) => e.Piece?.Take();
        _board.PieceUndoTake += (s, e) => e.Piece?.Undo();

        _commandManager = new CommandManager();
        _engine = new Engine(_board, _commandManager);

        _buttonView = FindObjectOfType<ButtonView>();
        _stateMachine = new StateMachine(_buttonView);

        _boardView = FindObjectOfType<BoardView>();
        _boardView.CardHovered += CardHovered;
        _boardView.CardDropped += CardDropped;

        _board.OnTileReactivated += HandleTileEnable;
        _board.OnTileDeactivated += HandleTileDisable;

        ResetInternalState();
        InitializePiecePlacement();

        _buttonView.OnUndo += UndoLastMove;
        _buttonView.OnPlay += StartGame;
        _buttonView.OnPause += PauseGame;
        _buttonView.OnResume += ResumeGame;
        _buttonView.OnQuit += ExitGame;
        _buttonView.OnRestart += RestartGame;
    }

    private void InitializePiecePlacement()
    {
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

    private void OnDisable()
    {
        _boardView.CardHovered -= CardHovered;
        _boardView.CardDropped -= CardDropped;

        _board.OnTileReactivated -= HandleTileEnable;
        _board.OnTileDeactivated -= HandleTileDisable;

        _audioManager?.UnregisterBoardEvents();

        _buttonView.OnUndo -= UndoLastMove;
        _buttonView.OnPlay -= StartGame;
        _buttonView.OnPause -= PauseGame;
        _buttonView.OnResume -= ResumeGame;
        _buttonView.OnQuit -= ExitGame;
        _buttonView.OnRestart -= RestartGame;
    }

    private void Start()
    {
        _stateMachine.ChangeState(new MenuState(_buttonView));
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

    private void UndoLastMove()
    {
        _commandManager?.Previous();
        _boardView.ActivatedPositions = null;
    }

    private void StartGame()
    {
        _stateMachine.ChangeState(_stateMachine.PlayState);
    }

    private void PauseGame()
    {
        _stateMachine.ChangeState(_stateMachine.PauseState);
    }

    private void ResumeGame()
    {
        _stateMachine.ChangeState(_stateMachine.PlayState);
    }

    private void CheckGameOverCondition()
    {
        var remainingPieces = _board.GetAllPieces().Count(p => !p.IsPlayer);

        if (remainingPieces == 0 && _board.Playerpiece != null)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if(_delayGameOverTransition != null)
        {
            StopCoroutine(_delayGameOverTransition);
        }
        
        _delayGameOverTransition = StartCoroutine(DelayGameEnd());
    }

    private void ResetInternalState()
    {
        _board.ClearAll();  // Clear the board by removing all pieces
        _boardView.ActivatedPositions = null;  // Clear any activated positions
    }

    private void RestartGame()
    {
        _board.Reset();       // Undo previous actions, re-emit events, then clear internal state
        _commandManager.Reset();

        InitializePiecePlacement(); // Re-initialize placement of all PieceViews

        _boardView.ActivatedPositions = null;
        _vfxManager?.Clear(); // Clean up the particles still alive
        _stateMachine.ChangeState(_stateMachine.PlayState);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator DelayGameEnd()
    {
        yield return new WaitForSeconds(0.5f); // Allow UI to reset
        //Cards get frozen otherwise
        _stateMachine.ChangeState(_stateMachine.GameOverState);
    }
}
