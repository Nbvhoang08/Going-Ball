using DG.Tweening;
using DG.Tweening.Core.Easing;
using Hapiga.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBallUI : UICanvas
{
    [SerializeField] int indexBallShowing = -1;
    [SerializeField] Transform[] functions;
    [SerializeField]
    Button btnUnlock, btnAdsToUnlock;
    private void Start()
    {
        OnShow();
        btnUnlock.onClick.AddListener(UnlockBall);
        btnAdsToUnlock.onClick.AddListener(AdsToUnlockBall);
    }
    public void OnShow()
    {
        if (GameManager.Instance != null)
        {
            indexBallShowing = GameManager.Instance.GameData.BallIndexUsing;
        }
        DisplayUI();
    }
    public void Back()
    {
        GameManager.Instance.ShowBallShop(false);
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<ChangeBallUI>();
        }).OnComplete(() => UIManager.Instance.OpenUI<StatUI>());
    }
    public void Next()
    {
        GameManager.Instance.ShowNextBall(ref indexBallShowing);
        DisplayUI();
    }
    public void Previous()
    {
        GameManager.Instance.ShowPreviousBall(ref indexBallShowing);
        DisplayUI();
    }
    public void SelectBall()
    {
        GameManager.Instance.SelectBall(indexBallShowing);
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<ChangeBallUI>();
        }).OnComplete(() => UIManager.Instance.OpenUI<StatUI>());
    }

    void DisplayUI()
    {
        BallInfor skyboxInfor = GameManager.Instance.GameData.GetBallInfor(indexBallShowing);

        DisplayFunction(skyboxInfor.state.Equals(ItemState.unlock), skyboxInfor);
    }
    private void DisplayFunction(bool unlock, BallInfor ballInfor)
    {
        functions[(int)TypeFunction.unlock].gameObject.SetActive(unlock);
        functions[(int)TypeFunction.unlocked].gameObject.SetActive(!unlock);

        bool coinHightThanPrice = GameManager.Instance.Coin >= ballInfor.price;
        btnUnlock.interactable = coinHightThanPrice;
        btnUnlock.GetComponentInChildren<TMP_Text>().text = ballInfor.price.ToString();
    }

    public void UnlockBall()
    {
        GameManager.Instance.UnlockBallOnShop(indexBallShowing);
        DisplayUI();
    }
    public void AdsToUnlockBall()
    {
        if (GameManager.Instance.PaymentTicket(1))
        {
            RewardUnlockBall();
        }
        else
        {
            //AdManager.Instance.ShowRewardedVideo(RewardUnlockBall, "Unlock_Ball");
            RewardUnlockBall();
        }
    }
    void RewardUnlockBall()
    {
        GameManager.Instance.Coin -= GameManager.Instance.Coin;
        GameManager.Instance.UnlockBallOnShop(indexBallShowing, true);
        DisplayUI();
    }
}
