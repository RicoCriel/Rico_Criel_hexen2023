using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class CardView : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler
{
    LayoutElement _layoutGroup;
    private RectTransform _rect;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private Image _image;

    [SerializeField]
    private CardType _cardType;
    public CardType CardType => _cardType;

    private void Start()
    {
        _layoutGroup = GetComponent<LayoutElement>();
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _layoutGroup.ignoreLayout = true;
        _canvasGroup.alpha = 0.5f;
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _layoutGroup.ignoreLayout = false;
        _canvasGroup.alpha = 1f;
        _image.raycastTarget = true;
    }
}
