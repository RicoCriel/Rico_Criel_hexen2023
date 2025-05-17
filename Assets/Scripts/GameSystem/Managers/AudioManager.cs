using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class AudioManager: MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioConfig _audioConfig;

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private int _poolSize = 5;
    private List<AudioSource> _sfxPool = new List<AudioSource>();

    private Board _board;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        ConfigureAudioSources();
    }

    private void ConfigureAudioSources()
    {
        // Create pool
        for (int i = 0; i < _poolSize; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.spatialBlend = 0;
            source.playOnAwake = false;
            _sfxPool.Add(source);
        }
    }

    public void InitializeWithBoard(Board board)
    {
        _board = board;
        RegisterBoardEvents();
    }

    private void RegisterBoardEvents()
    {
        if (_board == null) return;

        _board.PieceMoved += HandlePieceMoved;
        _board.PieceTaken += HandlePieceTaken;
        //_board.PieceUndoTake += HandlePieceUndo;
        //_board.PieceUndoMove += HandleUndoMove;
    }

    public void UnregisterBoardEvents()
    {
        if (_board == null) return;

        _board.PieceMoved -= HandlePieceMoved;
        _board.PieceTaken -= HandlePieceTaken;
        //_board.PieceUndoTake -= HandlePieceUndo;
        //_board.PieceUndoMove -= HandleUndoMove;
    }

    public void PlaySFX(SFXType type)
    {
        if (!_audioConfig.TryGetSFX(type, out var clip)) return;

        var source = _sfxPool.FirstOrDefault(s => !s.isPlaying);
        if (source == null) source = _sfxPool[0]; // Fallback

        if(!source.isPlaying)
        {
            source.PlayOneShot(clip);
        }
    }

    public void PlayMusic(MusicType type, bool loop = true)
    {
        if (_audioConfig.TryGetMusic(type, out var clip))
        {
            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }
    }

    public void StopMusic(float fadeDuration = 0.5f)
    {
        StartCoroutine(FadeMusic(0f, fadeDuration));
    }

    public void SetMusicVolume(float volume, float fadeDuration = 0.5f)
    {
        StartCoroutine(FadeMusic(volume, fadeDuration));
    }

    private IEnumerator FadeMusic(float targetVolume, float duration)
    {
        float startVolume = _musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _musicSource.volume = targetVolume;
        if (targetVolume <= 0f) _musicSource.Stop();
    }

    public void SetSFXVolume(float volume)
    {
        _sfxSource.volume = Mathf.Clamp01(volume);
    }

    private void HandlePieceMoved(object sender, PieceMovedEventArgs e)
    {
        PlaySFX(e.Piece.IsPlayer ? SFXType.PlayerMove : SFXType.EnemyMove);
    }

    private void HandlePieceTaken(object sender, PieceTakenEventArgs e)
    {
        PlaySFX(SFXType.PieceCaptured);
        if (e.Piece.IsPlayer) PlaySFX(SFXType.GameOver);
    }

    private void HandleUndoMove(object sender, PieceUndoMoveEventArgs e)
    {
        PlaySFX(SFXType.UndoMove);
    }

    private void HandlePieceUndo(object sender, PieceUndoTakeEventArgs e)
    {
        PlaySFX(SFXType.PieceRespawn);
    }

    private void OnDestroy()
    {
        UnregisterBoardEvents();
        StopAllCoroutines();
    }
}
