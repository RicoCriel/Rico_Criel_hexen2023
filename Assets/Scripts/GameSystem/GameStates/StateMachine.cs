using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class StateMachine
{
    private IState _currentState;
    public MenuState MenuState { get; }
    public PlayState PlayState { get; }
    public PauseState PauseState { get; }
    public GameOverState GameOverState { get; }

    public StateMachine(ButtonView buttonView)
    {
        MenuState = new MenuState(buttonView);
        PlayState = new PlayState(buttonView);
        PauseState = new PauseState(buttonView);
        GameOverState = new GameOverState(buttonView);
    }

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
}
