using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPortal : MonoBehaviour
{
    public Renderer portal;
    public Material SkyboxMaterial;
    [SerializeField] private float _edgeWidthMax = 0.05f;
    private Tween _amountTweens;
    private Tween _widthTweens;
    private Material _mat;
    private Collider _Col;
    [SerializeField] GameObject[] visual;
    void OnEnable()
    {
        _Col = GetComponent<Collider>();
        _mat = portal.material;

    }
    public void Disapear()
    {
        GameEvent.OnChangeSkyboxMethod(SkyboxMaterial);
        _Col.enabled = false;
        foreach (GameObject obj in visual)
        {
            obj.SetActive(false);
        }
        PlayDissolve(0f, 1f,3);

    }
    private void PlayDissolve(float from, float to, float _duration)
    {
        // Dừng tất cả tween cũ
        _amountTweens.Kill();
        _widthTweens.Kill();
        _amountTweens = null;
        _widthTweens = null;


        _mat.SetFloat("_DissolveAmount", from);

        var widthUp = DOTween.To(
            () => _mat.GetFloat("_EdgeWidth"),
            x => _mat.SetFloat("_EdgeWidth", x),
            _edgeWidthMax,
            _duration * 0.5f
        ).SetEase(Ease.OutSine);

        var dissolve = DOTween.To(
            () => _mat.GetFloat("_DissolveAmount"),
            x => _mat.SetFloat("_DissolveAmount", x),
            to,
            _duration
        ).SetEase(Ease.InOutSine)
         .OnComplete(() =>
         {
             var widthDown = DOTween.To(
                 () => _mat.GetFloat("_EdgeWidth"),
                 x => _mat.SetFloat("_EdgeWidth", x),
                 0f,
                 _duration * 0.25f
             ).SetEase(Ease.InSine).OnComplete(() => gameObject.SetActive(false));

         });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Disapear();
        }
    }
}

