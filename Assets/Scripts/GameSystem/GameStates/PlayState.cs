using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class PlayState : IState
{
    private readonly ButtonView _buttonView;

    public PlayState(ButtonView buttonView)
    {
        _buttonView = buttonView;
    }

    public void Enter()
    {
        Time.timeScale = 1f;
        _buttonView.ShowGameUI();
        _buttonView.HideGameOverMenu();
        _buttonView.HidePauseMenu();
        _buttonView.HideStartMenu();
    }

    public void Exit()
    {
        _buttonView.HideGameUI();
    }
}
