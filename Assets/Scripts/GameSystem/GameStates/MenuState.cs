using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class MenuState : IState
{
    private ButtonView _buttonView;

    public MenuState(ButtonView buttonView)
    {
        _buttonView = buttonView;
    }

    public void Enter()
    {
        _buttonView.ShowStartMenu();
        _buttonView.HideGameUI();
        _buttonView.HideGameOverMenu();
        _buttonView.HidePauseMenu();
    }

    public void Exit()
    {
        _buttonView.HideStartMenu();
    }
}
