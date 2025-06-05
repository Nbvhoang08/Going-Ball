using DG.Tweening;
using Hapiga.Ads;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TreasureUI : UICanvas
{
    [SerializeField]
    Transform rewardBallUI;
    [SerializeField]
    Button btnHideRewardBallUI;

    [SerializeField] Button btnAds, btnNothank;
    [SerializeField]
    TreasureBox[] treasureBoxs;
    [Space]
    [SerializeField]
    Image[] keys;
    TypeBall typeBallReward;
    public int totalOpened
    {
        get { return _totalOpened; }
        set
        {
            _totalOpened = value;
            CheckKeys();
        }
    }
    bool rewardedBall;
    int _totalOpened;
    float[] coins = { 20, 40, 60, 70, 80, 100, 200, 400 };
    List<int> indexRewards;

    void Start()
    {
        btnAds.onClick.AddListener(AdsToGetKey);
        btnNothank.onClick.AddListener(Nothank);
        btnHideRewardBallUI.onClick.AddListener(HideRewardBall);
    }
    public override void Open()
    {
        base.Open();
        OnShow();
    }
    public void OnShow()
    {
        bool firstTreasure = PlayerPrefs.GetInt("FirstTreasure") == 0;
        if (firstTreasure)
            typeBallReward = GameManager.Instance.CaculationRewardBallOnTreasure(firstTreasure).typeBall;
        else
            typeBallReward = GameManager.Instance.CaculationRewardBallOnTreasure().typeBall;

        if (typeBallReward != TypeBall.Base)
            GameManager.Instance.ShowRenderBall(true, typeBallReward);
        _totalOpened = 0;
        rewardedBall = false;
        rewardBallUI.gameObject.SetActive(false);
        foreach (TreasureBox treasureBox in treasureBoxs)
        {
            treasureBox.OnShow(this);
        }
        indexRewards = new List<int>();
        for (int i = 0; i < coins.Length; i++)
        {
            indexRewards.Add(i);
        }
        CheckKeys();
        //HandleButtonNoThank();

    }
    void CheckKeys()
    {
        int totalKey = GameManager.Instance.Key;
        ShowFunctions(totalKey);
        if (totalKey >= 0)
        {
            if (totalKey < keys.Length)
            {
                for (int i = 0; i < totalKey; i++)
                {
                    keys[i].sprite = GameManager.Instance.GameData.keySprites[(int)KeySprite.owned];
                }
                for (int i = totalKey; i < keys.Length; i++)
                {
                    keys[i].sprite = GameManager.Instance.GameData.keySprites[(int)KeySprite.none];
                }
            }
            else
            {
                foreach (Image image in keys)
                {
                    image.sprite = GameManager.Instance.GameData.keySprites[(int)KeySprite.owned];
                }
            }
            if (totalKey == 0)
            {
                foreach (TreasureBox treasureBox in treasureBoxs)
                {
                    treasureBox.NoKey();
                }
            }
        }
    }
    public float CaculationReward()
    {
        if (totalOpened < 3)
        {
            int firstTreasure = PlayerPrefs.GetInt("FirstTreasure");
            if (firstTreasure.Equals(0) && totalOpened.Equals(2)) // 0 = firsttime
            {
                rewardedBall = true;
                Invoke("OnRewardBall", 0.7f);
                PlayerPrefs.SetInt("FirstTreasure", 1);
                return -1;
            }
            else
            {
                int indexReward = indexRewards[Random.Range(0, indexRewards.Count)];
                indexRewards.Remove(indexReward);
                Debug.Log("indexReward" + indexReward);
                return coins[indexReward];
            }
        }
        else
        {
            int randomIndex = rewardedBall ? Random.Range(0, indexRewards.Count) : Random.Range(0, indexRewards.Count + 1);
            if (randomIndex.Equals(indexRewards.Count))
            {
                if (!typeBallReward.Equals(TypeBall.Base))
                {
                    rewardedBall = true;
                    Invoke("OnRewardBall", 0.7f);
                    return -1;
                }
                else
                {
                    return 400f; // if all ball unlocked, rewardd 400 gold
                }
            }
            else
            {
                int indexReward = indexRewards[randomIndex];
                indexRewards.Remove(indexReward);
                return coins[indexReward];
            }
        }
    }
    public void OnRewardBall()
    {
        GameManager.Instance.UnlockBall(typeBallReward);
        GameManager.Instance.SelectBall(typeBallReward);
        rewardBallUI.gameObject.SetActive(true);
    }
    public void HideRewardBall()
    {
        rewardBallUI.gameObject.SetActive(false);
    }
    private void ShowFunctions(int totalKey)
    {
        btnAds.transform.parent.gameObject.SetActive(!(totalKey > 0));
        bool openedAll = true;
        foreach (TreasureBox treasureBox in treasureBoxs)
        {
            if (!treasureBox.Opened)
            {
                openedAll = false;
                break;
            }
        }
        
        btnAds.gameObject.SetActive(!openedAll);
        HandleButtonNoThank();
    }
    void AdsToGetKey()
    {
        if (GameManager.Instance.PaymentTicket(1))
        {
            RewardKeys();
        }
        else
        {
            RewardKeys();
            //AdManager.Instance.ShowRewardedVideo(RewardKeys, "Treasure_Keys");
        }
    }
    void RewardKeys()
    {
        GameManager.Instance.Key += Constants.maxKey;
        CheckKeys();
        foreach (TreasureBox treasureBox in treasureBoxs)
        {
            treasureBox.OnRewardAds();
        }
    }
    void HandleButtonNoThank()
    {
        btnNothank.gameObject.SetActive(false);
        Invoke("ShowButton", 0.7f);
    }
    void ShowButton()
    {
        btnNothank.gameObject.SetActive(true);
    }
    void Nothank()
    {
        GameManager.Instance.ShowRenderBall(false);
        UIManager.Instance.CloseUI<TreasureUI>(0.4f);
        DOVirtual.DelayedCall(0.4f, () => UIManager.Instance.OpenUI<GiftBoxUI>());
        
    }
}
