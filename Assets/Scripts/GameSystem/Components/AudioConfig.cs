using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SFXType
{
    None,
    PlayerMove,
    EnemyMove,
    PieceCaptured,
    PieceRespawn,
    UndoMove,
    GameOver,
    UIButtonClick
}

public enum MusicType
{
    MainMenu,
    Gameplay,
    GameOver
}

[CreateAssetMenu(menuName = "Audio/Configuration", order = 0)]
internal class AudioConfig : ScriptableObject
{
    [System.Serializable]
    public struct SFXEntry
    {
        public SFXType type;
        public AudioClip clip;
    }

    [System.Serializable]
    public struct MusicEntry
    {
        public MusicType type;
        public AudioClip clip;
    }

    [SerializeField] private List<SFXEntry> _sfxClips = new List<SFXEntry>();
    [SerializeField] private List<MusicEntry> _musicClips = new List<MusicEntry>();

    private Dictionary<SFXType, AudioClip> _sfxDict;
    private Dictionary<MusicType, AudioClip> _musicDict;

    private void OnEnable()
    {
        _sfxDict = _sfxClips
            .Where(e => e.clip != null)
            .GroupBy(e => e.type)
            .ToDictionary(g => g.Key, g => g.First().clip);

        _musicDict = _musicClips
            .Where(e => e.clip != null)
            .GroupBy(e => e.type)
            .ToDictionary(g => g.Key, g => g.First().clip);
    }

    public bool TryGetSFX(SFXType type, out AudioClip clip)
    {
        return _sfxDict.TryGetValue(type, out clip);
    }

    public bool TryGetMusic(MusicType type, out AudioClip clip)
    {
        return _musicDict.TryGetValue(type, out clip);
    }
}

