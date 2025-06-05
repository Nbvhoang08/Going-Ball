using Hapiga.Ads;
using Hapiga.Tracking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseUI : UICanvas
{
    public void RefillLoseGame()
    {
        if (GameManager.Instance.PaymentTicket(1))
        {
            RewardRefill();
        }
        else
        {
            //AdManager.Instance.ShowRewardedVideo(RewardRefill, "Refill");
            RewardRefill();
        }
    }

    void RewardRefill()
    { 
        GameManager.Instance.RewardReFill();
        UIManager.Instance.CloseUIDirectly<LoseUI>();
    }

    public void NoThankLoseGame()
    {
        GameManager.Instance.ResetCurrentLevel();
        UIManager.Instance.CloseUIDirectly<LoseUI>();
    }
}
