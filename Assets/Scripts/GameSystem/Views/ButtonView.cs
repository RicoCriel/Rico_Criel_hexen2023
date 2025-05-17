using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class ButtonView: MonoBehaviour
{
    [SerializeField] private Button _undoButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _gamePlayUI;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;

    [SerializeField] private GameObject _cardContainer;

    public Action OnUndo;
    public Action OnPlay;
    public Action OnPause;
    public Action OnResume;
    public Action OnQuit;
    public Action OnRestart;

    private void OnEnable()
    {
        _undoButton.onClick.AddListener(Undo);
        _playButton.onClick.AddListener(Play);
        _pauseButton.onClick.AddListener(Pause);
        _resumeButton.onClick.AddListener(Resume);
        _quitButton.onClick.AddListener(Quit);
        _restartButton.onClick.AddListener(Restart);
    }

    private void OnDisable()
    {
        _undoButton.onClick.RemoveListener(Undo);
        _playButton.onClick.RemoveListener(Play);
        _pauseButton.onClick.RemoveListener(Pause);
        _resumeButton.onClick.RemoveListener(Resume);
        _quitButton.onClick.RemoveListener(Quit);
        _restartButton.onClick.RemoveListener(Restart);
    }

    private void Undo()
    {
        OnUndo?.Invoke();
    }

    private void Play()
    {
        OnPlay?.Invoke();
    }

    private void Pause()
    {
        OnPause?.Invoke();
    }

    private void Resume()
    {
        OnResume?.Invoke();
    }

    private void Quit()
    {
        OnQuit?.Invoke();
    }

    private void Restart()
    {
        AudioManager.Instance.PlaySFX(SFXType.UIButtonClick);
        OnRestart?.Invoke();
    }

    public void ShowPauseMenu()
    {
        _pauseMenu.SetActive(true);
        _cardContainer.SetActive(false);
    }

    public void HidePauseMenu()
    {
        _pauseMenu.SetActive(false);
        _cardContainer.SetActive(true);
    }

    public void ShowGameOverMenu()
    {
        _gameOverMenu.SetActive(true);
        _cardContainer.SetActive(false);
    }

    public void HideGameOverMenu()
    {
        _gameOverMenu.SetActive(false);
        _cardContainer.SetActive(true);
    }

    public void ShowStartMenu()
    {
        _startMenu.SetActive(true);
        _cardContainer.SetActive(false);
    }

    public void HideStartMenu()
    {
        _startMenu.SetActive(false);
        _cardContainer.SetActive(true);
    }

    public void ShowGameUI()
    {
        _gamePlayUI.SetActive(true);
    }

    public void HideGameUI()
    {
        _gamePlayUI.SetActive(false);
    }

    public void PlaySFX()
    {
        AudioManager.Instance.PlaySFX(SFXType.UIButtonClick);
    }

    public void PlayUndoSFX()
    {
        AudioManager.Instance.PlaySFX(SFXType.UndoMove);
    }
}
