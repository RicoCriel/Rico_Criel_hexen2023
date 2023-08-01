using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PieceView : MonoBehaviour
{

    void Start()
    {
        var gridPosition = PositionHelper.GridPosition(transform.position);
        transform.position = PositionHelper.WorldPosition(gridPosition);
    }

    public Vector3 Position => transform.position;


    [SerializeField]
    private bool _isPlayer;

    public bool IsPlayer => _isPlayer;

    internal void MoveTo(Position toPosition)
            => transform.position = PositionHelper.WorldPosition(toPosition);


    internal void Take()
        => gameObject.SetActive(false);


    internal void Place(Position toPosition)
    {
        gameObject.SetActive(true);
        MoveTo(toPosition);
    }
}

