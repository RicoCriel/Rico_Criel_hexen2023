using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuState : State
{
    private MenuView _menuView;

    public override void OnEnter()
    {
        var asyncOperation = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
            asyncOperation.completed += InitializeScene;
    }

    public override void OnExit()
    {
        if (_menuView != null)
            _menuView.PlayClicked -= OnPlayClicked;

        SceneManager.UnloadSceneAsync("Menu");
    }


    private void InitializeScene(AsyncOperation obj)
    {
        _menuView = GameObject.FindObjectOfType<MenuView>();
            if (_menuView != null)
                _menuView.PlayClicked += OnPlayClicked;
    }

    private void OnPlayClicked(object sender, EventArgs e)
    {
            StateMachine.MoveTo(States.Playing);
    }
    
}
