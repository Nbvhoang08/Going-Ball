using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Hapiga.UI
{
    [AddComponentMenu(DUI.COMPONENT_MENU_UIBACKGROUND, DUI.MENU_PRIORITY_UIBACKGROUND)]
    public class UIBackground : MonoBehaviour
    {
        public UIElement uiElement;
        public Canvas canvas;
        public Image bgImg;
        private Color defaultColor;

        private void Awake()
        {
            defaultColor = bgImg.color;
        }

        public void SetInfo(int canvasSortingOrder)
        {
            canvas.sortingOrder = canvasSortingOrder;
            bgImg.color = defaultColor;
        }

        public void SetInfo(int canvasSortingOrder, Color backgroundColor)
        {
            canvas.sortingOrder = canvasSortingOrder;
            bgImg.color = backgroundColor;
        }

        #region Context Menu

#if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UIBACKGROUND, false, DUI.MENU_PRIORITY_UIBACKGROUND)]
        static void CreateButton(UnityEditor.MenuCommand menuCommand)
        {
            GameObject targetParent = null;
            GameObject selectedGO = menuCommand.context as GameObject;
            if (selectedGO == null || selectedGO.GetComponent<Canvas>() == null)
            {
                EditorUtility.DisplayDialog("Error", "Select Canvas to create UI Background", "OK");
                return;
            }
            targetParent = selectedGO;

            GameObject go = new GameObject("UIBackground", typeof(RectTransform), typeof(UIElement),typeof(UIBackground));
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, targetParent);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UIBackground uiBackground = go.GetComponent<UIBackground>();
            uiBackground.uiElement = go.GetComponent<UIElement>();
            uiBackground.uiElement.useCustomStartAnchoredPosition = true;
            uiBackground.uiElement.customStartAnchoredPosition = Vector3.zero;
            uiBackground.uiElement.inAnimations.fade.enabled = true;
            uiBackground.uiElement.outAnimations.fade.enabled = true;
            uiBackground.canvas = go.GetComponent<Canvas>();
            uiBackground.canvas.overrideSorting = true;
            RectTransform rectTrans =go.GetComponent<RectTransform>();
            rectTrans.localScale = Vector3.one;
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.one;
            rectTrans.sizeDelta = Vector2.zero;
            rectTrans.pivot = new Vector2(0.5f, 0.5f);

            Image bgImg = go.AddComponent<Image>();
            bgImg.sprite = DUI.UISprite;
            bgImg.type = Image.Type.Sliced;
            bgImg.fillCenter = true;
            bgImg.color = new Color(0f,0f,0f, 0.7f);
            uiBackground.bgImg = bgImg;
            UnityEditor.Selection.activeObject = go;
        }
#endif

        #endregion
    }
}