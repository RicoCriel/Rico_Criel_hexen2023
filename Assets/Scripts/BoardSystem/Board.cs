using System;
using System.Collections.Generic;


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
        public event EventHandler<PieceTakenEventArgs> PieceTaken;
        public event EventHandler<PiecePlacedEventArgs> PiecePlaced;

        private readonly Dictionary<Position, PieceView> _pieces = new Dictionary<Position, PieceView>();

        public int Radius;
        public PieceView Playerpiece;

    private readonly List<Position> _occupiedPositions = new List<Position>();


    public Board(int radius)
        {
            this.Radius = radius;
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

            piece.MoveTo(toPosition); // Update the piece's position

            OnPieceMoved(new PieceMovedEventArgs(piece, fromPosition, toPosition));

            return true;
        }

        public bool Place(Position toPosition, PieceView piece)
        {
            if (!IsValid(toPosition))
                return false;

            if (_pieces.ContainsKey(toPosition))
                return false;  // Avoid placing on an occupied position

            if (_pieces.ContainsValue(piece))
                return false;  // Avoid placing the same piece twice

            _pieces[toPosition] = piece;

            OnPiecePlaced(new PiecePlacedEventArgs(piece, toPosition));

            return true;
        }


        //new method for rainmoveset
        public void DeactivateTile(Position position)
        {
            if (TryGetPieceAt(position, out var piece) && !piece.IsPlayer)
            {
                // Deactivate the piece
                _pieces.Remove(position);
                OnPieceTaken(new PieceTakenEventArgs(piece, position));
            }
            else
            {
                // Deactivate the tile game object
                if (_pieces.ContainsKey(position))
                {
                    _pieces.Remove(position);
                    OnPieceTaken(new PieceTakenEventArgs(null, position)); // Send null to indicate no piece
                }
            }
        }


        public bool Take(Position fromPosition)
        {
            if (!IsValid(fromPosition))
                return false;

            if (!_pieces.TryGetValue(fromPosition, out var piece))
                return false;

            _pieces.Remove(fromPosition);

            OnPieceTaken(new PieceTakenEventArgs(piece, fromPosition));

            return true;
        }

    public bool IsPositionAvailable(Position position)
    {
        return !_pieces.ContainsKey(position);
    }


    public bool TryGetPieceAt(Position position, out PieceView piece)
                => _pieces.TryGetValue(position, out piece);

        public bool IsValid(Position position)
            => (-Radius < position.Q && position.Q < Radius)
            && (-Radius < position.R && position.R < Radius)
            && (-Radius < position.S && position.S < Radius)
            && position.Q + position.R + position.S == 0;

        public void ClearBoard()
        {
            _pieces.Clear();
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
        #endregion


    }

