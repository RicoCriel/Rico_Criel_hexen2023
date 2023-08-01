using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PositionView : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    [SerializeField]
    private UnityEvent OnActivate;

    [SerializeField]
    private UnityEvent OnDeactivate;

    private BoardView _boardView;

    public Position GridPosition
        => PositionHelper.GridPosition(transform.position);


    void Start()
    {
        _boardView = GetComponentInParent<BoardView>();
        var gridPosition = PositionHelper.GridPosition(transform.position);
        transform.position = PositionHelper.WorldPosition(gridPosition);
    }

    internal void Deactivate()
        => OnDeactivate?.Invoke();

    internal void Activate()
        => OnActivate?.Invoke();

    public void OnPointerEnter(PointerEventData eventData)
    {
        var cardView = eventData.pointerDrag?.GetComponent<CardView>();
        if(cardView != null) 
            _boardView.OnCardViewHoveredOnPositionView(this, cardView);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var cardView = eventData.pointerDrag?.GetComponent<CardView>();
        if (cardView != null)
            _boardView.OnCardViewDroppedOnPositionView(this, cardView);
    }
}
