using UnityEngine;
using CameraShake;
using System;
using System.Collections.Generic;
using UnityEngine.VFX;

public class PieceView : MonoBehaviour, IShake, IParticle
{
    [SerializeField] private GameObject _explodeParticlePrefab;
    [SerializeField] private GameObject _moveParticlePrefab;

    public Vector3 Position => transform.position;

    [SerializeField]
    private bool _isPlayer;
    public bool IsPlayer => _isPlayer;

    public Position GridPosition { get; set; }

    private Action <GameObject>OnPieceTaken;
    private Action <GameObject>OnPieceMoved;
    private Action OnPieceUndo;

    private VfxManager _vfxManager;

    void Start()
    {
        _vfxManager = FindObjectOfType<VfxManager>();
        var gridPosition = PositionHelper.GridPosition(transform.position);
        transform.position = PositionHelper.WorldPosition(gridPosition);
    }

    private void OnEnable()
    {
        OnPieceTaken += SpawnVFX;
        OnPieceMoved += SpawnVFX;
        OnPieceTaken += DoCameraShake;
    }

    private void OnDisable()
    {
        OnPieceTaken -= SpawnVFX;
        OnPieceMoved -= SpawnVFX;
        OnPieceTaken -= DoCameraShake;
    }

    internal void Take()
    {
        OnPieceTaken?.Invoke(_explodeParticlePrefab);
        gameObject.SetActive(false);
    }

    internal void Undo()
    {
        OnPieceUndo?.Invoke();
        gameObject.SetActive(true);
    }

    internal void MoveTo(Position toPosition)
    {
        GridPosition = toPosition; // Update the grid position when moving
        transform.position = PositionHelper.WorldPosition(toPosition);
        OnPieceMoved?.Invoke(_moveParticlePrefab);
    }

    internal void Place(Position toPosition)
    {
        GridPosition = toPosition; // Update the grid position when placing
        gameObject.SetActive(true);
        MoveTo(toPosition);
    }

    public void DoCameraShake(GameObject camera = null)
    {
        CameraShaker.Presets.ShortShake3D();
    }

    public void SpawnVFX(GameObject prefab)
    {
        _vfxManager.SpawnEffect(prefab, transform.position);
    }
}

