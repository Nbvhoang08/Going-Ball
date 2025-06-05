using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : UICanvas
{
    [SerializeField] LifeUI[] _lifeUI;
    [SerializeField] TextMeshProUGUI LVName;

    private void OnEnable()
    {
        GameEvent.OnUpdateUI += UpdateLife;
        GameEvent.OnGameStart += UpdateLVName;
    }
    private void OnDisable()
    {
        GameEvent.OnUpdateUI -= UpdateLife;
        GameEvent.OnGameStart -= UpdateLVName;
    }

    public void UpdateLife()
    {
        for (int i = 0; i < _lifeUI.Length; i++)
        {
            if (i < GameManager.Instance.CurrLife)
            {
                _lifeUI[i].ChangeLife(true);
            }
            else
            {
                _lifeUI[i].ChangeLife(false);
            }
        }
    }
    public void UpdateLVName()
    {
        LVName.text = $"Level {GameManager.Instance.Level +1}";
    }
    public void PauseBtn() 
    {
        UIManager.Instance.OpenUI<TestSetting>();
        GameManager.Instance.PauseGame();
    }

}
