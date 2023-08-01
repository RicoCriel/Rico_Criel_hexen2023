using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Position
{
    private readonly int _q;
    private readonly int _r;
    private readonly int _s;

    public int Q => _q;
    public int R => _r;
    public int S => _s; 

    public Position(int q, int r, int s)
    {
        _q = q;
        _r = r;
        _s = s;
    }

    public Position Add(Position position)
    {
        return new Position(_q + position.Q, _r + position.R, _s + position.S);
    }

    public Position Add(Position position, int radius)
    {
        return new Position(_q + position.Q, _r + position.R, _s + position.S);
    }

    public Position Subtract(Position position)
    {
        return new Position(_q - position.Q, _r - position.R, _s - position.S);
    }

    public Position Scale(int scaleFactor)
    {
        return new Position(_q * scaleFactor, _r * scaleFactor, _s * scaleFactor);
    }

    public Position Scale(Position position,int scaleFactor)
    {
        return new Position(position._q * scaleFactor, position._r * scaleFactor, position._s * scaleFactor);
    }

    public Position RotateLeft()
    {
        return new Position(-_s, -_q, -_r);
    }

    public Position RotateRight()
    {
        return new Position(-_r, -_s, -_q);
    }

    static public List<Position> directions = new List<Position> {
                                        new Position(1, 0, -1),
                                        new Position(1, -1, 0),
                                        new Position(0, -1, 1),
                                        new Position(-1, 0, 1),
                                        new Position(-1, 1, 0),
                                        new Position(0, 1, -1) };

    static public Position Direction(int direction)
    {
        return Position.directions[direction];
    }

    public Position Neighbor(int direction)
    {
        return Add(Position.Direction(direction));
    }

    public Position Neighbor(Position position,int direction)
    {
        return Add(Position.Direction(direction));
    }

    static public List<Position> Diagonals = new List<Position> {
                                            new Position(2, -1, -1),
                                            new Position(1, -2, 1),
                                            new Position(-1, -1, 2),
                                            new Position(-2, 1, 1),
                                            new Position(-1, 2, -1),
                                            new Position(1, 1, -2) };

    public Position DiagonalNeighbor(int direction)
    {
        return Add(Position.Diagonals[direction]);
    }

    public int Length()
    {
        return (int)((Mathf.Abs(_q) + Mathf.Abs(_r) + Mathf.Abs(_s)) / 2);
    }

    public int Distance(Position position)
    {
        return Subtract(position).Length();
    }

    public List<Position> Ring(Position centerPosition, int radius)
    {
        var results = new List<Position>();
        var hex = Add(centerPosition.Scale(Direction(4), radius));

        for (int i = 0; i <= 5; i++)
        {
            for (int j = 0; j <= radius; j++)
            {
                results.Append(hex);
                hex = Neighbor(hex, i);
            }
        }
        return results;
    }

    public override string ToString()
    {
        return $"Position({Q}, {R}, {S})";
    }
}
