using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//internal delegate List<Position> Collector(Board board, Position fromPosition, Position hoverPosition);
//
//internal class ConfigurableMoveSet:MoveSet
//{
//    private readonly Collector _collector;
//
//
//    public ConfigurableMoveSet(Board board,CommandQueue commandQueue, Collector collector) : base(board)
//    {
//        _collector = collector;
//    }
//
//    public override List<Position> Positions(Position fromPosition, Position hoverPosition)
//        => _collector(Board, fromPosition, hoverPosition);
//
//    internal override bool Execute(Position fromPosition, Position hoverPosition, Position toPosition)
//    {
//        return true;
//        return false;
//    }
//    
//}
