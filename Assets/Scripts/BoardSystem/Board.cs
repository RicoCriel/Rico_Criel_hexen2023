using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceMovedEventArgs : EventArgs
{
    public PieceView Piece { get; }

    public Position FromPosition { get; }

    public Position ToPosition { get; }

    public PieceMovedEventArgs(PieceView piece, Position fromPosition, Position toPosition)
    {
        Piece = piece;
        FromPosition = fromPosition;
        ToPosition = toPosition;
    }
}

public class PieceUndoMoveEventArgs : EventArgs
{
    public PieceView Piece { get; }

    public Position FromPosition { get; }

    public Position ToPosition { get; }

    public PieceUndoMoveEventArgs(PieceView piece, Position fromPosition, Position toPosition)
    {
        Piece = piece;
        FromPosition = fromPosition;
        ToPosition = toPosition;
    }
}

public class PieceTakenEventArgs : EventArgs
{
    public PieceView Piece { get; }

    public Position FromPosition { get; }

    public PieceTakenEventArgs(PieceView piece, Position fromPosition)
    {
        Piece = piece;
        FromPosition = fromPosition;
    }
}

public class PieceUndoTakeEventArgs : EventArgs
{
    public PieceView Piece { get; }

    public Position FromPosition { get; }

    public PieceUndoTakeEventArgs(PieceView piece, Position fromPosition)
    {
        Piece = piece;
        FromPosition = fromPosition;
    }
}

public class PiecePlacedEventArgs : EventArgs
{
    public PieceView Piece { get; }

    public Position ToPosition { get; }

    public PiecePlacedEventArgs(PieceView piece, Position toPosition)
    {
        Piece = piece;
        ToPosition = toPosition;
    }
}

public class PositionsActivatedEventArgs : EventArgs
{
    public List<Position> OldPositions { get; }
    public List<Position> NewPositions { get; }

    public PositionsActivatedEventArgs(List<Position> oldPositions, List<Position> newPositions)
    {
        OldPositions = oldPositions;
        NewPositions = newPositions;
    }
}
public class Board
{
    public event EventHandler<PieceMovedEventArgs> PieceMoved;
    public event EventHandler<PieceUndoMoveEventArgs> PieceUndoMove;
    public event EventHandler<PieceTakenEventArgs> PieceTaken;
    public event EventHandler<PiecePlacedEventArgs> PiecePlaced;
    public event EventHandler<PieceUndoTakeEventArgs> PieceUndoTake;

    public Action <Position> OnTileReactivated;
    public Action<Position> OnTileDeactivated;

    private readonly Dictionary<Position, PieceView> _pieces = new Dictionary<Position, PieceView>();

    public int Radius;
    public PieceView Playerpiece;
    public PositionView TileView;

    private readonly List<Position> _updatedPositions = new List<Position>();
    private readonly List<Position> _deactivatedTiles = new List<Position>();
    private readonly List<(Position From, Position To)> _moveHistory = new List<(Position,Position)>();

    private readonly Dictionary<Position, PieceView> _deactivatedPieces = new Dictionary<Position, PieceView>();

    public Board(int radius)
    {
        this.Radius = radius;
    }

    public IEnumerable<PieceView> GetAllPieces()
    {
        return _pieces.Values;
    }

    public bool Move(Position fromPosition, Position toPosition)
    {
        if (!IsValid(fromPosition))
            return false;

        if (!IsValid(toPosition))
            return false;

        if (_pieces.ContainsKey(toPosition))
            return false;

        if (!_pieces.TryGetValue(fromPosition, out var piece))
            return false;

        _pieces[toPosition] = piece;
        _pieces.Remove(fromPosition);

        piece.MoveTo(toPosition); 

        OnPieceMoved(new PieceMovedEventArgs(piece, fromPosition, toPosition));
        _updatedPositions.Add(toPosition);
        _moveHistory.Add((fromPosition, toPosition));

        return true;
    }

    public bool UndoMove(Position fromPosition, Position toPosition)
    {
        if (!_pieces.TryGetValue(fromPosition, out var piece))
        {
            //Debug.Log("UNDO MOVE FAILED");
            return false;
        }

        _pieces.Remove(fromPosition);
        _pieces[toPosition] = piece;

        piece.MoveTo(toPosition);

        OnPieceUndoMove(new PieceUndoMoveEventArgs(piece, fromPosition, toPosition));
        return true;
    }

    public bool TryUndoMove(Position fromPosition, Position toPosition)
    {
        return _pieces.ContainsKey(fromPosition);
    }

    public bool Place(Position toPosition, PieceView piece)
    {
        if (!IsValid(toPosition))
            return false;

        if (_pieces.ContainsKey(toPosition))
            return false;  

        if (_pieces.ContainsValue(piece))
            return false; 

        _pieces[toPosition] = piece;

        OnPiecePlaced(new PiecePlacedEventArgs(piece, toPosition));
        return true;
    }

    //Only used for Rain MoveSet atm
    public void DeactivateTile(Position position) 
    {
        if (_deactivatedTiles.Contains(position))
        {
            return;
        }

        _deactivatedTiles.Add(position);


        if (TryGetPieceAt(position, out var piece) && !piece.IsPlayer)
        {
            _pieces.Remove(position);
            OnPieceTaken(new PieceTakenEventArgs(piece, position));
            _deactivatedPieces.Add(position, piece);
        }
        OnTileDeactivated?.Invoke(position);
    }

    public void UndoDeactivateTile(Position position)
    {
        if (_deactivatedTiles.Remove(position))
        {
            OnTileReactivated?.Invoke(position);
        }
    }

    public bool Take(Position fromPosition)
    {
        if (!IsValid(fromPosition))
            return false;

        if (!_pieces.TryGetValue(fromPosition, out var piece))
            return false;

        _pieces.Remove(fromPosition);
        _deactivatedPieces.Add(fromPosition, piece);

        OnPieceTaken(new PieceTakenEventArgs(piece, fromPosition));

        return true;
    }

    public bool UndoTake(Position fromPosition)
    {
        if (!_deactivatedPieces.TryGetValue(fromPosition, out var piece))
        {
            return false;
        }

        // Move the piece back to active
        _pieces[fromPosition] = piece;
        _deactivatedPieces.Remove(fromPosition);
        OnPieceUndoTake(new PieceUndoTakeEventArgs(piece, fromPosition));
        return true;
    }

    public bool IsPositionAvailable(Position position)
    {
        return !_pieces.ContainsKey(position);
    }

    public List<Position> GetAllPositions()
    {
        List<Position> positions = new List<Position>();
        for (int q = -Radius; q <= Radius; q++)
        {
            int r1 = Math.Max(-Radius, -q - Radius);
            int r2 = Math.Min(Radius, -q + Radius);
            for (int r = r1; r <= r2; r++)
            {
                positions.Add(new Position(q, r, -q - r));
            }
        }
        return positions;
    }

    public bool TryGetPieceAt(Position position, out PieceView piece)
        => _pieces.TryGetValue(position, out piece);

    public bool IsValid(Position position)
        => (-Radius < position.Q && position.Q < Radius)
        && (-Radius < position.R && position.R < Radius)
        && (-Radius < position.S && position.S < Radius)
        && position.Q + position.R + position.S == 0;

    public bool IsTileActive(Position position)
    {
        return !_deactivatedTiles.Contains(position);
    }

    public void ClearAll()
   {
        _pieces.Clear();
        _deactivatedPieces.Clear();
        _deactivatedTiles.Clear();
        _moveHistory.Clear();
   }

    public void Reset()
    {
        foreach (var (from, to) in _moveHistory.AsEnumerable().Reverse())
        {
            UndoMove(to,from);
        }

        foreach (var kvp in _deactivatedPieces.ToList())
        {
            UndoTake(kvp.Key);
        }

        foreach (var position in GetAllPositions())
        {
            OnTileReactivated?.Invoke(position);
        }

        ClearAll();
    }

    #region EventTriggers
    protected virtual void OnPieceMoved(PieceMovedEventArgs eventArgs)
   {
        var handler = PieceMoved;
        handler?.Invoke(this, eventArgs);
   }

    protected virtual void OnPiecePlaced(PiecePlacedEventArgs eventArgs)
   {
        var handler = PiecePlaced;
        handler?.Invoke(this, eventArgs);
   }

    protected virtual void OnPieceTaken(PieceTakenEventArgs eventArgs)
   {
        var handler = PieceTaken;
        handler?.Invoke(this, eventArgs);
   }

    protected virtual void OnPieceUndoTake(PieceUndoTakeEventArgs eventArgs)
   {
       var handler = PieceUndoTake;
       handler?.Invoke(this, eventArgs);
   }

    protected virtual void OnPieceUndoMove(PieceUndoMoveEventArgs eventArgs)
    {
        var handler = PieceUndoMove;
        handler?.Invoke(this, eventArgs);
    }
    #endregion

}

