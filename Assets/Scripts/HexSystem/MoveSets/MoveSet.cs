using System;
using System.Collections.Generic;


internal abstract class MoveSet : IMoveSet
{
    private Board _board;

    protected Board Board => _board;

    protected MoveSet(Board board)
    {
        _board = board;
    }

    internal abstract bool Execute(List<Position> positions);

    public abstract List<Position> Positions(Position fromPosition, Position hoverPosition);
    internal abstract ICommand CreateCommand(List<Position> positions);
}
