using Hapiga.Ads;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelBonusUI : UICanvas
{
    [SerializeField] MultiReward multiReward;
    [SerializeField] TMP_Text txtCoin;
    [SerializeField] Button btnAds, btnNothank;
    TMP_Text txtMultiCoin;
    float oldGoldReward;
    private void Start()
    {
        btnAds.onClick.AddListener(adsToComplete);
        btnNothank.onClick.AddListener(Complete);
        txtMultiCoin = btnAds.GetComponentInChildren<TMP_Text>();
    }
    public override void Open()
    {
        base.Open();
        OnShow();
    }
    private void Update()
    {
        float multi = multiReward.caculationMulti(false, true);

        float GoldReward = (int)(GameManager.Instance.CoinCollected * Constants.ValuePerCoin * multi);
        if (GoldReward != oldGoldReward)
        {
            txtMultiCoin.text = GoldReward.ToString("0");
            oldGoldReward = GoldReward;
        }
    }
    public void OnShow()
    {
        multiReward.OnStart();
        txtCoin.text = (GameManager.Instance.CoinCollected * Constants.ValuePerCoin).ToString();
    }
    void Complete(bool ads)
    {
        float multi = multiReward.caculationMulti(true, ads);
        if (ads)
            GameManager.Instance.Coin += GameManager.Instance.CoinCollected * Constants.ValuePerCoin * multi;
        StartCoroutine(CompleteDelayAction());
        
    }
    IEnumerator CompleteDelayAction()
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance.Key.Equals(Constants.maxKey))
            UIManager.Instance.OpenUI<TreasureUI>();
        else
            UIManager.Instance.OpenUI<GiftBoxUI>();
        UIManager.Instance.CloseUIDirectly<WheelBonusUI>();
    }
    void adsToComplete()
    {
        if (GameManager.Instance.PaymentTicket(1))
        {
            RewardToComplete();
        }
        else
        {
            //AdManager.Instance.ShowRewardedVideo(RewardToComplete, "Wheelbonus_Multi");
            RewardToComplete();
        }
    }
    void RewardToComplete()
    {
        Complete(true);
    }
    void Complete()
    {
        Complete(false);
    }
}
