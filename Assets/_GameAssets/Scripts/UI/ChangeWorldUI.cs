using DG.Tweening;
using DG.Tweening.Core.Easing;
using Hapiga.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWorldUI : UICanvas
{
    [SerializeField] Transform[] functions;
    [SerializeField] int indexWorldShowing = -1;
    [SerializeField] Button btnUnlock, btnAdsToUnlock;
    private void Start()
    {
        btnUnlock.onClick.AddListener(UnlockWorld);
        btnAdsToUnlock.onClick.AddListener(AdsToUnlockWorld);
    }
    private void OnEnable()
    {
        OnShow();
    }
    public void OnShow()
    {
        if (GameManager.Instance != null)
        {
            indexWorldShowing = GameManager.Instance.GameData.IndexWorldUsing();
        }
        DisplayUI();
    }
    public void Back()
    {
        GameManager.Instance.ShowWorldShop(false);
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<ChangeWorldUI>();
        }).OnComplete(() => UIManager.Instance.OpenUI<StatUI>());
    }
    public void Next()
    {
        GameManager.Instance.ShowNextWorld(ref indexWorldShowing);
        DisplayUI();
    }
    public void Previous()
    {
        GameManager.Instance.ShowPreviousWorld(ref indexWorldShowing);
        DisplayUI();
    }
    public void SelectWorld()
    {
        GameManager.Instance.SelectWolrd(indexWorldShowing);
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<ChangeWorldUI>();
        }).OnComplete(() => UIManager.Instance.OpenUI<StatUI>());
    }

    void DisplayUI()
    {
        SkyBoxInfors skyboxInfor = GameManager.Instance.GameData.GetWorldInfor(indexWorldShowing);

        DisplayFunction(skyboxInfor.state.Equals(ItemState.unlock), skyboxInfor);
    }
    void DisplayFunction(bool unlock, SkyBoxInfors skyboxInfor)
    {
        functions[(int)TypeFunction.unlock].gameObject.SetActive(unlock);
        functions[(int)TypeFunction.unlocked].gameObject.SetActive(!unlock);
        bool coinHightThanPrice = GameManager.Instance.Coin >= skyboxInfor.price;
        btnUnlock.interactable = coinHightThanPrice;
        btnUnlock.GetComponentInChildren<TMP_Text>().text = skyboxInfor.price.ToString();
        
    }
    public void UnlockWorld()
    {
        GameManager.Instance.UnlockWorld(indexWorldShowing);
        DisplayUI();
    }
    public void AdsToUnlockWorld()
    {
        if (GameManager.Instance.PaymentTicket(1))
        {
            RewardUnlockWorld();
        }
        else
        {
            //AdManager.Instance.ShowRewardedVideo(RewardUnlockWorld, "Unlock_World");
            RewardUnlockWorld();
        }
    }

    void RewardUnlockWorld()
    {
        GameManager.Instance.Coin -= GameManager.Instance.Coin;
        GameManager.Instance.UnlockWorld(indexWorldShowing, true);
        DisplayUI();
    }
    
}
