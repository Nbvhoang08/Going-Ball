using Hapiga.Ads;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GiftBoxUI : UICanvas
{
    [SerializeField]
    Transform mainGiftBox, RewardBall;
    [SerializeField]
    Image giftBox;
    [SerializeField]
    Button btnAccelerate, btnNothank, btnAdsToClaimBall, btnLostBall;
    [SerializeField]
    TMP_Text txtPercent, txtPrice;
    int latePercent;
    TypeBall typeBallReward;
    private void Start()
    {
        btnAccelerate.onClick.AddListener(AdsToAccelerate);
        btnNothank.onClick.AddListener(Nothank);
        btnAdsToClaimBall.onClick.AddListener(AdsToGetBall);
        btnLostBall.onClick.AddListener(Nothank);
    }
    public override void Open()
    {
        base.Open();
        OnShow();
    }
    public void OnShow()
    {
        float durationAnim = 1f;
        float currentGift = GameManager.Instance.Gift;
        GameManager.Instance.Gift += Constants.PercentGiftPerLevel;
        giftBox.fillAmount = currentGift / 100f;
        float targetGift = GameManager.Instance.Gift / 100f;
        giftBox.DOFillAmount(targetGift, durationAnim).OnComplete(() => {
            if (GameManager.Instance.Gift >= 100)
            {
                GiftBall();
            }
        });

        latePercent = (int)currentGift;
        txtPercent.text = currentGift.ToString("0") + "%";

        mainGiftBox.gameObject.SetActive(true);
        RewardBall.gameObject.SetActive(false);
        btnNothank.gameObject.SetActive(false);
        Invoke("ShowNothank", durationAnim * 0.7f);
    }
    void LateUpdate()
    {
        int newPercent = Mathf.RoundToInt(giftBox.fillAmount * 100f);
        if (newPercent != latePercent)
        {
            txtPercent.text = (giftBox.fillAmount * 100f).ToString("0") + "%";
            latePercent = newPercent;
        }
    }

    void ShowNothank()
    {
        btnNothank.gameObject.SetActive(true);
    }
    void AdsToAccelerate()
    {
        if (GameManager.Instance.PaymentTicket(1))
        {
            RewardAccelerate();
        }
        else
        {
            //AdManager.Instance.ShowRewardedVideo(RewardAccelerate, "Giftbox_Accelerate");
            RewardAccelerate();
        }
    }

    void RewardAccelerate()
    {
        float durationAnim = 1f;
        float currentGift = GameManager.Instance.Gift;
        GameManager.Instance.Gift = Mathf.Min(GameManager.Instance.Gift + Constants.PercentGiftAccelerate, 100f);
        float targetGift = GameManager.Instance.Gift / 100f;
        giftBox.DOFillAmount(targetGift, durationAnim).OnComplete(() => {
            if (GameManager.Instance.Gift >= 100)
            {
                GiftBall();
            }
        });
        latePercent = Mathf.RoundToInt(currentGift);
        txtPercent.text = currentGift.ToString("0") + "%";
    }

    void AdsToGetBall()
    {
        if (GameManager.Instance.PaymentTicket(1))
        {
            RewardAdsGetBall();
        }
        else
        {
            RewardAdsGetBall();
            //AdManager.Instance.ShowRewardedVideo(RewardAdsGetBall, "Giftbox_Ball");
        }
    }

    void RewardAdsGetBall()
    {
        GameManager.Instance.UnlockBall(typeBallReward);
        GameManager.Instance.SelectBall(typeBallReward);
        Nothank();
    }

    void GiftBall()
    {
        BallInfor ballInfor = GameManager.Instance.CaculationRewardBallOnGiftBox();
        typeBallReward = ballInfor.typeBall;
        Debug.Log(typeBallReward);
        if (typeBallReward != TypeBall.Base)
        {
            GameManager.Instance.ShowRenderBall(true, typeBallReward);

        }

        mainGiftBox.gameObject.SetActive(false);
        RewardBall.gameObject.SetActive(true);
        txtPrice.text = ballInfor.price.ToString();
        GameManager.Instance.Gift = 0f;
        HandleLostBallButton();
       
    }
    void HandleLostBallButton()
    {
        btnLostBall.gameObject.SetActive(false);
        Invoke("ShowButtonLostBall", 0.7f);
    }
    void ShowButtonLostBall()
    {
        btnLostBall.gameObject.SetActive(true);
    }
    void Nothank()
    {
        DOVirtual.DelayedCall(0.4f, () =>
        {
            GameManager.Instance.ShowRenderBall(false);
            GameManager.Instance.NewLevel();
        });
        UIManager.Instance.CloseUI<GiftBoxUI>(0.4f);
    }
}
