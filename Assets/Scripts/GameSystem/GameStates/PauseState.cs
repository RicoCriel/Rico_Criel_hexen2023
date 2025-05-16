using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class PauseState : IState
{
    private readonly ButtonView _buttonView;

    public PauseState(ButtonView buttonView)
    {
        _buttonView = buttonView;
    }

    public void Enter()
    {
        Time.timeScale = 0f;
        _buttonView.ShowPauseMenu();
        _buttonView.HideGameUI();
        _buttonView.HideGameOverMenu();
        _buttonView.HideStartMenu();
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        _buttonView.HidePauseMenu();
    }

}
