using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CommandManager
{
    private List<ICommand> _commands = new List<ICommand>();
    private int _currentCommand = -1;

    public bool IsAtStart => _currentCommand < 0;
    public bool IsAtEnd => _currentCommand >= _commands.Count - 1;

    public void Execute(ICommand command)
    {
        // Clear redo stack when executing a new command after undoing
        if (_currentCommand + 1 < _commands.Count)
        {
            _commands.RemoveRange(_currentCommand + 1, _commands.Count - (_currentCommand + 1));
        }

        _commands.Add(command);
        Next(); // Execute the new command
    }

    public void Previous()
    {
        if (IsAtStart) return;

        _commands[_currentCommand].Undo();
        _currentCommand--;
    }

    public void Next()
    {
        if (IsAtEnd) return;

        _currentCommand++;
        _commands[_currentCommand].Execute();
    }

    public void Reset()
    {
        _commands.Clear();
        _currentCommand = -1;
    }
}
