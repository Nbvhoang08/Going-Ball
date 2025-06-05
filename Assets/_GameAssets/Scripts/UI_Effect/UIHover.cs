using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    public float hoverScale = 1.1f;
    public float duration = 0.2f;

    private Vector3 _originalScale;
    private Tween _currentTween;

    void Start()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cancel any ongoing tween
        _currentTween?.Kill();
        _currentTween = transform.DOScale(_originalScale * hoverScale, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _currentTween?.Kill();
        _currentTween = transform.DOScale(_originalScale, duration).SetEase(Ease.OutBack);
    }
}