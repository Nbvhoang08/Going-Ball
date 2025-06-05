using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTrailUI : UICanvas 
{
    [SerializeField] int indexTrailShowing = -1;
    private void OnEnable()
    {
        OnShow();
    }
    public void OnShow()
    {
        if (GameManager.Instance != null)
        {
            indexTrailShowing = GameManager.Instance.GameData.TrailIndexUsing;
        }
        else
        {
            return;
        }

      
    }

    public void ShowNextTrail()
    {
        GameManager.Instance.ShowNextTrails(ref indexTrailShowing);
    }
    public void ShowPeriousTrail()
    {
        GameManager.Instance.ShowPreviousTrails(ref indexTrailShowing);
    }

    public void SelectTrail()
    {
        
        GameManager.Instance.SelectTrails(indexTrailShowing);
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<ChangeTrailUI>();
        }).OnComplete(() => UIManager.Instance.OpenUI<StatUI>());
    }
    public void Back()
    {
        GameManager.Instance.ShowTrailsShop(false);
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<ChangeTrailUI>();
        }).OnComplete(() => UIManager.Instance.OpenUI<StatUI>());

    }
}
