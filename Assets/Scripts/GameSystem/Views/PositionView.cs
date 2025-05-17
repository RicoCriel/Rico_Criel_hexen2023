using CameraShake;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class PositionView : MonoBehaviour, IDropHandler,
    IPointerEnterHandler, IShake, IParticle
{
    [SerializeField]
    private UnityEvent OnActivate;

    [SerializeField]
    private UnityEvent OnDeactivate;

    [SerializeField]
    private UnityEvent OnDisablePosition;

    [SerializeField]
    private UnityEvent OnEnablePosition;

    private Action <GameObject> OnSpawnVFX;

    [SerializeField] private GameObject _particlePrefab;

    private BoardView _boardView;

    public Position GridPosition
        => PositionHelper.GridPosition(transform.position);

    private VfxManager _vfxManager;

    private void OnEnable()
    {
        //Debug.Log($"Before Add: {OnSpawnVFX?.GetInvocationList().Length ?? 0}");
        OnSpawnVFX += DoCameraShake;
        OnSpawnVFX += SpawnVFX;
        //Debug.Log($"After Add: {OnSpawnVFX?.GetInvocationList().Length ?? 0}");
    }

    private void OnDisable()
    {
        OnSpawnVFX -= DoCameraShake;
        OnSpawnVFX -= SpawnVFX;
    }

    void Start()
    {
        _vfxManager = FindObjectOfType<VfxManager>();
        _boardView = GetComponentInParent<BoardView>();
        var gridPosition = PositionHelper.GridPosition(transform.position);
        transform.position = PositionHelper.WorldPosition(gridPosition);
    }

    internal void Deactivate()
        => OnDeactivate?.Invoke();

    internal void Activate()
        => OnActivate?.Invoke();

    internal void Disable()
    {
        OnDisablePosition?.Invoke();
        OnSpawnVFX?.Invoke(_particlePrefab);
    }

    internal void Enable()
        => OnEnablePosition?.Invoke();

    public void OnPointerEnter(PointerEventData eventData)
    {
        var cardView = eventData.pointerDrag?.GetComponent<CardView>();
        if(cardView != null) 
            _boardView.OnCardViewHoveredOnPositionView(this, cardView);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var cardView = eventData.pointerDrag?.GetComponent<CardView>();
        if (cardView != null)
            _boardView.OnCardViewDroppedOnPositionView(this, cardView);
    }

    public void DoCameraShake(GameObject camera = null)
    {
        CameraShaker.Presets.ShortShake3D(0.2f,25,5);
    }

    public void SpawnVFX(GameObject prefab)
    {
        _vfxManager?.SpawnEffect(prefab, transform.position);
    }
}
