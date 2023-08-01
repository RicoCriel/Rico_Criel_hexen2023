using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayState : State
{
    private BoardView _boardView;

    public PlayState()
    {

    }

    public override void OnEnter()
    {
        var asyncOperation = SceneManager.LoadSceneAsync("HexGame", LoadSceneMode.Additive);
        asyncOperation.completed += InitializeScene;
    }

    public override void OnExit()
    {
        SceneManager.UnloadSceneAsync("HexGame");
    }

    private void InitializeScene(AsyncOperation obj)
    {
        _boardView = GameObject.FindObjectOfType<BoardView>();
    }
}
