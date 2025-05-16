using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class GameOverState : IState
{
    private ButtonView _buttonView;

    public GameOverState(ButtonView buttonView)
    {
        _buttonView = buttonView;
    }

    public void Enter()
    {
        _buttonView.ShowGameOverMenu();
        _buttonView.HideGameUI();
        _buttonView.HidePauseMenu();
        _buttonView.HideStartMenu();
    }

    public void Exit()
    {
        _buttonView.HideGameOverMenu();
    }
}
