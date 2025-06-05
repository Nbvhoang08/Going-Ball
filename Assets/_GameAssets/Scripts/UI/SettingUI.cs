using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : UICanvas
{
   public void Close() 
   {
      UIManager.Instance.CloseUIDirectly<SettingUI>();
    }



}
