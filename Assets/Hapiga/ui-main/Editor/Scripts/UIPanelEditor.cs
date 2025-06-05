using QuickEditor;
using UnityEditor;
using UnityEngine;

namespace Hapiga.UI.Editor
{
    [CustomEditor(typeof(UIPanel), true)]
    [DisallowMultipleComponent]
    [CanEditMultipleObjects]
    public class UIPanelEditor : UIElementEditor
    {
        UIPanel uiPanel { get { return (UIPanel)target; } }
        private SerializedProperty startHidden, animateAtStart, useBackground, newColorBackground, backgroundColor;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();
            useBackground = serializedObject.FindProperty("useBackground");
            newColorBackground = serializedObject.FindProperty("newColorBackground");
            backgroundColor = serializedObject.FindProperty("backgroundColor");
            startHidden = serializedObject.FindProperty("startHidden");
            animateAtStart = serializedObject.FindProperty("animateAtStart");
        }

        protected override void DrawSettings(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if (animateAtStart.boolValue) { GUI.enabled = false; }
                QUI.QToggle("hide @START", startHidden);
                GUI.enabled = true;
                QUI.Space(SPACE_8);

                if (startHidden.boolValue) { GUI.enabled = false; }
                QUI.QToggle("animate @START", animateAtStart);
                GUI.enabled = true;

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_4);
            QUI.Space(SPACE_8 * showCustomStartPosition.faded);
            QUI.BeginHorizontal(width);
            {
                if (useBackground.boolValue) { GUI.enabled = true; }
                QUI.QToggle("use @Background", useBackground);
                
                
                if (!useBackground.boolValue) { GUI.enabled = false; }
                QUI.QToggle("new background Color", newColorBackground);
                GUI.enabled = true;

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            
            if (newColorBackground.boolValue)
            {
                backgroundColor.colorValue = QUI.ColorField(backgroundColor.colorValue, true, true, false);
            }
            GUI.enabled = true;
            base.DrawSettings(width);

        }
    }
}