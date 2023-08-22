using UnityEngine;

public class PieceView : MonoBehaviour
{
    public Position GridPosition { get; set; }

    void Start()
    {
        var gridPosition = PositionHelper.GridPosition(transform.position);
        transform.position = PositionHelper.WorldPosition(gridPosition);
    }

    public Vector3 Position => transform.position;


    [SerializeField]
    private bool _isPlayer;

    public bool IsPlayer => _isPlayer;

    internal void Take()
        => gameObject.SetActive(false);
    internal void MoveTo(Position toPosition)
    {
        GridPosition = toPosition; // Update the grid position when moving
        transform.position = PositionHelper.WorldPosition(toPosition);
    }

    internal void Place(Position toPosition)
    {
        GridPosition = toPosition; // Update the grid position when placing
        gameObject.SetActive(true);
        MoveTo(toPosition);
    }
}

