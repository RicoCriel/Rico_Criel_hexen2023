using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum States
{
    //add states here
    Start, Playing, Player, Enemy
}

public class StateMachine
{
    private Dictionary<States, State> _states = new Dictionary<States, State>();

    private Stack<States> _currentStateNames = new Stack<States>();

    //this returns a State and cannot be directly compared 
    public State CurrentState => _states[_currentStateNames.Peek()];

    //this returns the current state name as an enum value.
    public States CurrentStateName => _currentStateNames.Peek();

    public void Register(States stateName, State state)
    {
        state.StateMachine = this;

        _states.Add(stateName, state);
    }

    public States InitialState
    {
        set
        {
            _currentStateNames.Push(value);
            CurrentState.OnEnter();
            CurrentState.OnResume();
        }
    }

    public void MoveTo(States stateName)
    {
        CurrentState.OnSuspend();
        CurrentState.OnExit();

        _currentStateNames.Pop();
        _currentStateNames.Push(stateName);

        CurrentState.OnEnter();
        CurrentState.OnResume();
    }

    public void Push(States stateName)
    {
        CurrentState.OnSuspend();

        _currentStateNames.Push(stateName);

        CurrentState.OnEnter();
        CurrentState.OnResume();
    }

    public void Pop()
    {
        CurrentState.OnSuspend();
        CurrentState.OnExit();

        _currentStateNames.Pop();

        CurrentState.OnResume();
    }
}
