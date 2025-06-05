using System;
using System.Collections.Generic;
using Hapiga.Core.Runtime.Singleton;
using UnityEngine;    
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
namespace Hapiga.UI
{
    public abstract class UIManagerSingleton<T> : Singleton<T>,IUiManager where T : MonoBehaviour 
    {
        public override void Init()
        {
            InitBackgroundQueue();
        }
#if ODIN_INSPECTOR
        [FoldoutGroup("Background")]
#endif
        [SerializeField] protected bool isUseBackground = true;
#if ODIN_INSPECTOR
        [FoldoutGroup("Background"), ShowIf("$isUseBackground")]
#endif
        [SerializeField] protected UIBackground background;
        [SerializeField] protected Queue<UIBackground> backgroundQueue;

        public UIBackground GetBackgroundPanel()
        {
            UIBackground bg = null;
            Debug.Log(backgroundQueue.Count);
            if (backgroundQueue != null)
            {
                if (backgroundQueue.Count < 1)
                {
                    UIBackground newBg = Instantiate<UIBackground>(background, transform);
                    // newBg.transform.SetParent(canvasTransform, false);
                    newBg.uiElement.Hide(true);
                    backgroundQueue.Enqueue(newBg);
                }

                if (backgroundQueue.Count > 0)
                {
                    bg = backgroundQueue.Dequeue();
                }
            }
            else
                Debug.LogError("bg queue null");

            return bg;
        }

        public void HideBackgroundPanel(UIBackground bg, bool instantAction)
        {
            bg.uiElement.Hide(instantAction);
            backgroundQueue.Enqueue(bg);
        }

        public void InitBackgroundQueue(int initCount = 1)
        {
            
            if (!isUseBackground)
            {
                return;
            }
            
            if (background == null)
            {
                Debug.LogError("No background attached");
                return;
            }
            
            backgroundQueue = new Queue<UIBackground>(3);

            for (int i = 0; i < initCount; i++)
            {
                UIBackground newBg = Instantiate<UIBackground>(background, transform);
                newBg.uiElement.Hide(true);
                backgroundQueue.Enqueue(newBg);
            }
            background.uiElement.Hide(true);
            backgroundQueue.Enqueue(background);
        }
    }
}
