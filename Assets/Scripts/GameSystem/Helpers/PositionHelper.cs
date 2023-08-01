using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class PositionHelper
{
    public static float TileSize = 1.1547f;

    public static Position GridPosition(Vector3 worldPosition)
    {
        
        var q = Mathf.RoundToInt((Mathf.Sqrt(3) / 3f * worldPosition.x - 1f / 3 * worldPosition.z) / TileSize);
        var r = Mathf.RoundToInt((2f / 3 * worldPosition.z) / TileSize);
        var s = -q - r;


        return new Position(q, r, s);
    }

    public static Vector3 WorldPosition(Position gridPosition)
    {
        var x = TileSize * (Mathf.Sqrt(3) * gridPosition.Q + Mathf.Sqrt(3) / 2 * gridPosition.R);
        var z = TileSize * (3f / 2 * gridPosition.R);
        return new Vector3(x, 0, z);
    }


}
