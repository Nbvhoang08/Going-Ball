using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BtnSetting : MonoBehaviour
{
    [SerializeField] Transform model;
    [SerializeField] TypeSetting typeSetting;
    [SerializeField] float maxX = 46.8f;

    Button button;
    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        bool value = GameManager.Instance.GetStateSetting(typeSetting);

        Vector3 setupPosition = model.localPosition;
        if (value)
            setupPosition.x = maxX;
        else
            setupPosition.x = -maxX;
        model.localPosition = setupPosition;
    }
    void OnClick()
    {
        float duration = 0.05f;
        bool newValue = GameManager.Instance.ChangeSetting(typeSetting);
        if (newValue)
            model.DOLocalMoveX(maxX, duration);
        else
            model.DOLocalMoveX(-maxX, duration);
    }
}
