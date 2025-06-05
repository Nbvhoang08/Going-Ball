using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : UICanvas
{
    public void ContinueBtn()
    {
       
        DOVirtual.DelayedCall(0.4f, () =>
        {
            UIManager.Instance.CloseUIDirectly<EndLevel>();
        }).OnComplete(() => GameManager.Instance.NewLevel());
       
    }
}
