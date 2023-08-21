using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

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

    public static Position GetRandomGridPosition(int maxRadius)
    {
        int randomQ = Random.Range(-maxRadius, maxRadius);
        int randomR = Random.Range(Mathf.Max(-maxRadius, -randomQ - maxRadius), Mathf.Min(maxRadius, -randomQ + maxRadius));
        int randomS = -randomQ - randomR;

        Position randomPosition = new Position(randomQ, randomR, randomS);

        if (!IsValid(randomPosition, maxRadius))
        {
            return GetRandomGridPosition(maxRadius);
        }

        return randomPosition;
    }

    public static bool IsValid(Position position, int maxRadius)
    {
        return (-maxRadius < position.Q && position.Q < maxRadius)
            && (-maxRadius < position.R && position.R < maxRadius)
            && (-maxRadius < position.S && position.S < maxRadius)
            && position.Q + position.R + position.S == 0;
    }
}
