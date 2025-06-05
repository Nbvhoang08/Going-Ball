using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Hapiga.UI
{
    [AddComponentMenu(DUI.COMPONENT_MENU_UIPANEL, DUI.MENU_PRIORITY_UIPANEL)]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [DisallowMultipleComponent]
    public class UIPanel : UIElement
    {
        #if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UIPANEL, false, DUI.MENU_PRIORITY_UIPANEL)]
        static void CreateElement(MenuCommand menuCommand)
        {
            GameObject selectedGO = menuCommand.context as GameObject;
            if (selectedGO == null || selectedGO.GetComponent<Canvas>() == null)
            {
                EditorUtility.DisplayDialog("Error", "Select Canvas to create UI Panel", "OK");
                return;
            }
            
            GameObject go = new GameObject("UIPanel", typeof(RectTransform), typeof(UIPanel));
            GameObjectUtility.SetParentAndAlign(go, selectedGO);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            go.GetComponent<UIPanel>().Reset();
            go.GetComponent<RectTransform>().localScale = Vector3.one;
            go.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            go.GetComponent<RectTransform>().anchorMax = Vector2.one;
            go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            go.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

            GameObject background = new GameObject("Panel", typeof(RectTransform), typeof(Image));
            GameObjectUtility.SetParentAndAlign(background, go);
            background.GetComponent<RectTransform>().localScale = Vector3.one;
            background.GetComponent<RectTransform>().anchorMin =new Vector2(0.5f, 0.5f);
            background.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            background.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 800);
            background.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            background.GetComponent<Image>().sprite = DUI.Background;
            background.GetComponent<Image>().type = Image.Type.Sliced;
            background.GetComponent<Image>().fillCenter = true;
            
            GameObject title = new GameObject("title", typeof(RectTransform), typeof(TextMeshProUGUI));
            GameObjectUtility.SetParentAndAlign(title, background);
            RectTransform titleRect = title.GetComponent<RectTransform>();
            titleRect.localScale = Vector3.one;
            titleRect.anchorMin =new Vector2(0.5f, 1f);
            titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.sizeDelta = new Vector2(300, 100);
            titleRect.pivot = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0, 0); 
            TextMeshProUGUI titleTxt = title.GetComponent<TextMeshProUGUI>();
            titleTxt.text = "Title";
            titleTxt.alignment = TextAlignmentOptions.Center;
            titleTxt.fontStyle = FontStyles.Bold;
            Selection.activeObject = go;
        }
#endif
        
        public bool useBackground = true;
        public bool newColorBackground = false;
        public Color backgroundColor = Color.black;
        private UIBackground bg;

        /// <summary>
        /// Hide the UIElement at runtime at start. Initiates an instant Hide. Default is set to false.
        /// </summary>
        public bool startHidden = false;
        /// <summary>
        /// Animate the UIElement at runtime at start. Initiates a Show, thus playing an In animation. Default is set to false.
        /// </summary>
        public bool animateAtStart = false;

        protected override void SetupElement()
        {
            base.SetupElement();

            if (animateAtStart)
            {
                HideUiElement(true);
                StartCoroutine(StartShow());

            }
            else
            {
                if (startHidden)
                {
                    HideUiElement(true);
                }
                else
                {
                    CheckShowBackground(true);
                }
            }
        }
        
        protected IEnumerator StartShow()
        {
            yield return waitDelay;
            ShowUiElement(false);
        }
        
        protected void CheckShowBackground(bool instantAction)
        {
            
            if (useBackground && bg == null)
            {
                if (UIManager == null)
                {
                    Debug.LogError("Cannot find UiManager");
                    return;
                }
                bg = UIManager.GetBackgroundPanel();
                if (newColorBackground)
                {
                    bg.SetInfo(this.Canvas.sortingOrder - 1, backgroundColor);
                }
                else
                {
                    bg.SetInfo(this.Canvas.sortingOrder - 1);
                }
                bg.uiElement.Show(instantAction);
            }
        }

        protected void CheckHideBackground(bool instantAction)
        {
            if (useBackground && bg != null)
            {
                if (UIManager == null)
                {
                    Debug.LogError("Cannot find UiManager");
                    return;
                }
                UIManager.HideBackgroundPanel(bg, instantAction);
                bg = null;
            }
        }

        public override void Show(bool instantAction)
        {
            if(isVisible == false)
            {
                CheckShowBackground(instantAction);
            }
            base.Show(instantAction);
        }

        public override void Hide(bool instantAction, bool shouldDisable)
        {
            if (isVisible)
            {
                CheckHideBackground(instantAction);
            }
            base.Hide(instantAction, shouldDisable);
        }
    }
}