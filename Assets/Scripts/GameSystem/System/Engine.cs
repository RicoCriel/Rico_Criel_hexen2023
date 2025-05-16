using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine
{
    private readonly Board _board;
    private readonly CommandManager _commandManager;
    private readonly MoveSetCollection _moveSetCollection;

    public MoveSetCollection MoveSet => _moveSetCollection;

    public Engine(Board board, CommandManager commandManager)
    {
        _board = board;
        _commandManager = commandManager;
        _moveSetCollection = new MoveSetCollection(_board);
    }
    
    internal bool Drop(CardType cardType, List<Position> positions)
    {
        if (!_moveSetCollection.TryGetMoveSet(cardType, out var moveSet))
            return false;

        var command = moveSet.CreateCommand(positions);
        if (command == null)
            return false;

        _commandManager.Execute(command);
        return true;
    }
}
