using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionEventArgs : EventArgs
{

    public CardView Card { get; }
    public PositionView Position { get; }


    public InteractionEventArgs(PositionView position, CardView card)
    {
        Position = position;
        Card = card;    
    }
}

public class BoardView : MonoBehaviour
{
    public event EventHandler<InteractionEventArgs> CardHovered;
    public event EventHandler<InteractionEventArgs> CardDropped;

    private List<Position> _activatedPositions = new List<Position>(0);

    private Dictionary<Position, PositionView> _positionViewsCached = new Dictionary<Position, PositionView>();

    public List<Position> ActivatedPositions
    {
        get { return _activatedPositions; }

        set
        {
            foreach (var position in _activatedPositions)
                _positionViewsCached[position].Deactivate();

            if (value == null)
                _activatedPositions = new List<Position>(0);
            else
                _activatedPositions = value;

            foreach (var position in _activatedPositions)
                _positionViewsCached[position].Activate();
        }
    }

    private void Start()
    {
        foreach (var positionView in GetComponentsInChildren<PositionView>())
            _positionViewsCached.Add(positionView.GridPosition, positionView);
    }


    internal void OnCardViewHoveredOnPositionView(PositionView positionView, CardView cardView)
    {
        OnCardHovered(new InteractionEventArgs(positionView, cardView));
    }
    internal void OnCardViewDroppedOnPositionView(PositionView positionView, CardView cardView)
    {
        OnCardDropped(new InteractionEventArgs(positionView, cardView));
    }

    protected virtual void OnCardHovered(InteractionEventArgs eventArgs)
    {
        var handler = CardHovered;
        handler?.Invoke(this, eventArgs);
    }

    protected virtual void OnCardDropped(InteractionEventArgs eventArgs)
    {
        var handler = CardDropped;
        handler?.Invoke(this, eventArgs);
    }
}

