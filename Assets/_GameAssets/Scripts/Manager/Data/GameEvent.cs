using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent
{
    public delegate void GameStart();
    public static GameStart OnGameStart;
    public static void OnGameStartMethod()
    {
        if (OnGameStart != null)
        {
            OnGameStart();
        }
    }
    public delegate void GameFinish();
    public static GameFinish OnGameFinish;
    public static void OnGameFinishMethod()
    {
        if (OnGameFinish != null)
        {
            OnGameFinish();
        }
    }
    public delegate void SetUpNewLevel();
    public static SetUpNewLevel OnSetUpNewLevel;
    public static void OnSetUpNewLevelMethod()
    {
        if (OnSetUpNewLevel != null)
        {
            OnSetUpNewLevel();
 
        }
    }

    public delegate void ResetLevel();
    public static ResetLevel OnResetLevel;
    public static void OnResetLevelMethod()
    {
        if (OnResetLevel != null)
        {
            OnResetLevel();
        }
    }
    public delegate void Dead();
    public static Dead OnDead;
    public static void OnDeadMethod()
    {
        if (OnDead != null)
        {
            OnDead();
        }
    }

    public delegate void ChangeBallSkin();
    public static ChangeBallSkin OnChangeBallSkin;
    public static void OnChangeBallSkinMethod()
    {
        if (OnChangeBallSkin != null)
        {
            OnChangeBallSkin();

        }
    }
    public delegate void UpdateUI();
    public static UpdateUI OnUpdateUI;
    public static void OnUpdateUIMethod()
    {
        if (OnUpdateUI != null)
        {
            OnUpdateUI();
        }
    }
    public delegate void ChangeSkybox(Material skyMat);
    public static ChangeSkybox OnChangeSkybox;
    public static void OnChangeSkyboxMethod(Material skyMat)
    {
        if (OnChangeSkybox != null)
        {
            OnChangeSkybox(skyMat);
        }
    }

    public static void OnShowShopBallMethod(bool show)
    {
        if (OnShowShopBall != null)
        {
            OnShowShopBall(show);
        }
    }
    public delegate void ShowShopBall(bool show);
    public static ShowShopBall OnShowShopBall;


    public delegate void ChangeCurrency(TypeCurrency typeCurrency);
    public static ChangeCurrency OnChangeCurrency;
    public static void OnChangeCurrencyMethod(TypeCurrency typeCurrency)
    {
        if (OnChangeCurrency != null)
        {
            OnChangeCurrency(typeCurrency);
        }
    }

}
