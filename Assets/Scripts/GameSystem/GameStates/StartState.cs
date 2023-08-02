using System;
using UnityEngine;

public class StartState : State
{
    private StartView _startView;

    public override void OnEnter()
    {
        _startView = GameObject.FindObjectOfType<StartView>();
           if (_startView != null)
            _startView.PlayClicked += OnPlayClicked;
    }

    public override void OnExit()
    {
        if (_startView != null)
            _startView.PlayClicked -= OnPlayClicked;
    }

    private void OnPlayClicked(object sender, EventArgs e)
    {
        StateMachine.MoveTo(States.Playing);
        _startView.Button.SetActive(false);
    }
}
