#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Hapiga.UI
{
    [RequireComponent(typeof(UIPanel))]
    public class BaseUI : MonoBehaviour
    {
        [SerializeField] protected UIPanel uiPanel;

        public UIPanel UiElement
        {
            get => uiPanel;
        }

        private void OnValidate()
        {
            uiPanel = GetComponent<UIPanel>();
        }

        public bool IsVisible()
        {
            return uiPanel.isVisible;
        }

        public virtual void OnInAnimationStart()
        {
        }

        public virtual void OnInAnimationFinish()
        {
        }

        public virtual void OnOutAnimationStart()
        {
        }

        public virtual void OnOutAnimationFinish()
        {
        }

        public void CloseUIElement()
        {
            uiPanel.Hide(false);
        }
#if ODIN_INSPECTOR
        [Button("Show")]
#else
        [ContextMenu("Show")]
#endif
        public void Show()
        {
            uiPanel.Show(false);
        }

#if ODIN_INSPECTOR
        [Button("Hide")]
#else
        [ContextMenu("Hide")]
#endif
        public void Hide()
        {
            uiPanel.Hide(false);
        }
    }
}