using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButtonClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Click Settings")]
    public float clickScale = 1.2f;
    public float duration = 0.2f;
    public Ease ease = Ease.OutQuad;

    private Vector3 _originalScale;
    private Tween _clickTween;

    void Start()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Kill tween trước đó (nếu có)
        _clickTween?.Kill();
        GameManager.Instance.PlaySound("ButtonClick");
        // Sequence: thu nhỏ -> phóng to lại
        _clickTween = DOTween.Sequence()
            .Append(transform.DOScale(_originalScale * clickScale, duration).SetEase(Ease.InQuad))
            .Append(transform.DOScale(_originalScale, duration).SetEase(ease));
    }
}
