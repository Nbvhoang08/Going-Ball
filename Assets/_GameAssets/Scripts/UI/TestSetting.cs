using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestSetting : UICanvas
{
    [SerializeField] TMP_InputField inputField;
    public void SelectLVButton()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
        {
            Debug.LogWarning("Vui lòng nhập số level!");
            return;
        }

        if (int.TryParse(inputField.text, out int levelIndex))
        {
            int index = levelIndex -1;
            inputField.text = string.Empty;
            GameManager.Instance.SelectLV(index);
            Time.timeScale = 1;

        }
        else
        {
            Debug.LogWarning("Chỉ được nhập số nguyên!");
        }
    }
    public void Back()
    {
        Time.timeScale = 1;
        UIManager.Instance.CloseUIDirectly<TestSetting>();
        GameManager.Instance.ResumeGame();
    }
}
