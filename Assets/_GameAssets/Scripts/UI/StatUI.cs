using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StatUI : UICanvas
{
    [SerializeField] Image[] keys;
    [SerializeField] TMP_Text txtCoin, txtAdsTicket;
    public override void Open()
    {
        base.Open();
        OnShow();
    }

    public void ChangeBall()
    {
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<StatUI>();
        }).OnComplete(() => GameManager.Instance.ShowBallShop(true));
    }
    public void ChangeWorld()
    {
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<StatUI>();
        }).OnComplete(() => GameManager.Instance.ShowWorldShop(true));
    }
    public void ChangeTrail()
    {
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<StatUI>();
        }).OnComplete(() =>GameManager.Instance.ShowTrailsShop(true));
    }
    public void SettingBtn()
    {
        UIManager.Instance.OpenUI<SettingUI>();
    }
    private void Update()
    {
        if (!GameManager.Instance.IsPointerOverUIObject() && GameManager.Instance.GameMode.Equals(GameMode.Starting) && Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.StartGame();

            UIManager.Instance.CloseUIDirectly<StatUI>();

        }
    }
    public void OnShow()
    {
        txtAdsTicket.text = GameManager.Instance.AdsTicket.ToString();
        txtCoin.text = GameManager.Instance.Coin.ToString();
        CheckKeys();
    }

    void CheckKeys()
    {
        int totalKey = GameManager.Instance.Key;
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
        }
    }
}
