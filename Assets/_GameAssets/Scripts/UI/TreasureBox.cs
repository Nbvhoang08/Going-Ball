using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TreasureBox : MonoBehaviour
{
    [SerializeField]
    Transform treasure, coins, ball;

    TMP_Text txtCoin;
    Button btnOpen;
    [SerializeField] TreasureUI treasureUI ;

    public bool Opened => opened;
    bool opened;
    void Start()
    {
        txtCoin = coins.GetComponentInChildren<TMP_Text>();
        btnOpen = GetComponent<Button>();
        btnOpen.onClick.AddListener(Open);
    }
    public void OnShow(TreasureUI treasureUI)
    {
        this.treasureUI = treasureUI;
        opened = false;
        if (btnOpen != null)
            btnOpen.interactable = true;

        treasure.gameObject.SetActive(true);
        coins.gameObject.SetActive(false);
        ball.gameObject.SetActive(false);
    }
    void Open()
    {
        if (GameManager.Instance.UseKey(1))
        {
            float coin = treasureUI.CaculationReward();
            txtCoin.text = coin.ToString();
            bool rewardBall = coin < 0;
            treasure.gameObject.SetActive(false);
            ball.gameObject.SetActive(rewardBall);
            coins.gameObject.SetActive(!rewardBall);
            btnOpen.interactable = false;
            opened = true;
            treasureUI.totalOpened++;
            GameManager.Instance.PlaySound(Constants.Openchest);
        }
    }
    public void NoKey()
    {
        btnOpen.interactable = false;
    }
    public void OnRewardAds()
    {
        if (!opened)
        {
            btnOpen.interactable = true;
        }
    }
}
