using QuickEditor;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

namespace Hapiga.UI.Editor
{
    [CustomEditor(typeof(UIElement), true)]
    [DisallowMultipleComponent]
    [CanEditMultipleObjects]
    public class UIElementEditor : QEditor
    {
        UIElement uiElement { get { return (UIElement)target; } }

#pragma warning disable 0414

        SerializedProperty
            
            useCustomStartAnchoredPosition, customStartAnchoredPosition, 
            inAnimations,
            OnInAnimationsStart, OnInAnimationsFinish,
            inAnimationsPresetCategoryName, inAnimationsPresetName, loadInAnimationsPresetAtRuntime,

            outAnimations,
            OnOutAnimationsStart, OnOutAnimationsFinish,
            outAnimationsPresetCategoryName, outAnimationsPresetName, loadOutAnimationsPresetAtRuntime;

        protected AnimBool
            showAutoHideDelay,
            showCustomStartPosition,
            showInAnimations, showInAnimationsEvents,
            showOutAnimations, showOutAnimationsEvents,
            showLoopAnimations, showLoopAnimationsPreset;

#pragma warning restore 0414

        bool autoExpandEnabledFeatures = false;
        //bool localShowHide = false;
        AnimBool createNewCategoryName;
        AnimBool inAnimationsNewPreset;
        AnimBool outAnimationsNewPreset;
        AnimBool loopAnimationsNewPreset;

        //bool currentCategoryNameIsCustomName = false;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat = 0;

        enum AnimType { In, Out, Loop }

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            

            useCustomStartAnchoredPosition = serializedObject.FindProperty("useCustomStartAnchoredPosition");
            customStartAnchoredPosition = serializedObject.FindProperty("customStartAnchoredPosition");

            inAnimations = serializedObject.FindProperty("inAnimations");

            OnInAnimationsStart = serializedObject.FindProperty("OnInAnimationsStart");
            OnInAnimationsFinish = serializedObject.FindProperty("OnInAnimationsFinish");


            inAnimationsPresetCategoryName = serializedObject.FindProperty("inAnimationsPresetCategoryName");
            inAnimationsPresetName = serializedObject.FindProperty("inAnimationsPresetName");
            loadInAnimationsPresetAtRuntime = serializedObject.FindProperty("loadInAnimationsPresetAtRuntime");

            outAnimations = serializedObject.FindProperty("outAnimations");

            OnOutAnimationsStart = serializedObject.FindProperty("OnOutAnimationsStart");
            OnOutAnimationsFinish = serializedObject.FindProperty("OnOutAnimationsFinish");

            outAnimationsPresetCategoryName = serializedObject.FindProperty("outAnimationsPresetCategoryName");
            outAnimationsPresetName = serializedObject.FindProperty("outAnimationsPresetName");
            loadOutAnimationsPresetAtRuntime = serializedObject.FindProperty("loadOutAnimationsPresetAtRuntime");
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("LocalShowHide",
                            new InfoMessage()
                            {
                                title = "Local Show/Hide",
                                message = "Only this UIElement will get shown/hidden when using the SHOW/HIDE buttons. Any other UIElements with the same element category and element name will not be animated.",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("GlobalShowHide",
                           new InfoMessage()
                           {
                               title = "Global Show/Hide",
                               message = "All the UIElements with the same element category and element name will get shown/hidden when using the SHOW/HIDE buttons.",
                               type = InfoMessageType.Info,
                               show = new AnimBool(false, Repaint)
                           });

            infoMessage.Add("InAnimationsDisabled",
                            new InfoMessage()
                            {
                                title = "Enable at least one In Animation for SHOW to work.",
                                message = "",
                                type = InfoMessageType.Warning,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OutAnimationsDisabled",
                            new InfoMessage()
                            {
                                title = "Enable at least one Out Animation for HIDE to work.",
                                message = "",
                                type = InfoMessageType.Warning,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("InAnimationsLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + inAnimationsPresetCategoryName.stringValue + " / " + inAnimationsPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadInAnimationsPresetAtRuntime.boolValue, Repaint)
                            });

            infoMessage.Add("OutAnimationsLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + outAnimationsPresetCategoryName.stringValue + " / " + outAnimationsPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadOutAnimationsPresetAtRuntime.boolValue, Repaint)
                            });

        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();
            showCustomStartPosition = new AnimBool(useCustomStartAnchoredPosition.boolValue, Repaint);
            showInAnimations = new AnimBool(autoExpandEnabledFeatures ? uiElement.InAnimationsEnabled || loadInAnimationsPresetAtRuntime.boolValue : false, Repaint);
            showInAnimationsEvents = new AnimBool(autoExpandEnabledFeatures ? uiElement.OnInAnimationsStart.GetPersistentEventCount() > 0 || uiElement.OnInAnimationsFinish.GetPersistentEventCount() > 0 : false, Repaint);

            showOutAnimations = new AnimBool(autoExpandEnabledFeatures ? uiElement.OutAnimationsEnabled || loadOutAnimationsPresetAtRuntime.boolValue : false, Repaint);
            showOutAnimationsEvents = new AnimBool(autoExpandEnabledFeatures ? uiElement.OnOutAnimationsStart.GetPersistentEventCount() > 0 || uiElement.OnOutAnimationsFinish.GetPersistentEventCount() > 0 : false, Repaint);

            createNewCategoryName = new AnimBool(false, Repaint);
            inAnimationsNewPreset = new AnimBool(false, Repaint);
            outAnimationsNewPreset = new AnimBool(false, Repaint);
            loopAnimationsNewPreset = new AnimBool(false, Repaint);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            AddMissingComponents();
        }

        void AddMissingComponents()
        {
            if (uiElement.GetComponent<Canvas>() == null) { uiElement.gameObject.AddComponent<Canvas>(); }
            if (uiElement.GetComponent<CanvasGroup>() == null) { uiElement.gameObject.AddComponent<CanvasGroup>(); }
            if (uiElement.GetComponent<GraphicRaycaster>() == null) { uiElement.gameObject.AddComponent<GraphicRaycaster>(); }
        }

        public override void OnInspectorGUI()
        {
            if (IsEditorLocked) { return; }

            serializedObject.Update();

            DrawSettings(GlobalWidth);

            QUI.Space(SPACE_8);

            DrawInAnimations(GlobalWidth);

            DrawOutAnimations(GlobalWidth);

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        protected virtual void DrawSettings(float width)
        {
            #region CUSTOM START POSITION
            QUI.Space(SPACE_4);
            QUI.BeginHorizontal(width);
            {
                //CUSTOM START POSITION
                QLabel.text = "custom start position";
                QLabel.style = Style.Text.Normal;
                tempFloat = width - QLabel.x - 16 - 12; //extra space after the custom start position label
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, useCustomStartAnchoredPosition.boolValue ? QColors.Color.Blue : QColors.Color.Gray), QLabel.x + 16 + 12 + tempFloat * showCustomStartPosition.faded, 18 + 24 * showCustomStartPosition.faded);
                QUI.Space(-QLabel.x - 12 - 12 - tempFloat * showCustomStartPosition.faded);

                QUI.Toggle(useCustomStartAnchoredPosition);
                QUI.BeginVertical(QLabel.x + 8, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                if (showCustomStartPosition.faded > 0.4f)
                {
                    QUI.PropertyField(customStartAnchoredPosition, (tempFloat - 4) * showCustomStartPosition.faded);
                }
            }
            QUI.EndHorizontal();
            showCustomStartPosition.target = useCustomStartAnchoredPosition.boolValue;

            QUI.Space(-20 * showCustomStartPosition.faded); //lift the buttons on the background

            if (showCustomStartPosition.faded > 0.4f)
            {
                tempFloat = (width - 16 - 16) / 3; //button width (3 buttons) that takes into account spaces
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(20 * showCustomStartPosition.faded);
                    if (QUI.GhostButton("Get Position", QColors.Color.Blue, tempFloat * showCustomStartPosition.faded, 16 * showCustomStartPosition.faded))
                    {
                        customStartAnchoredPosition.vector3Value = uiElement.RectTransform.anchoredPosition3D;
                    }

                    QUI.Space(SPACE_4);

                    if (QUI.GhostButton("Set Position", QColors.Color.Blue, tempFloat * showCustomStartPosition.faded, 16 * showCustomStartPosition.faded))
                    {
                        Undo.RecordObject(uiElement.RectTransform, "SetPosition");
                        uiElement.RectTransform.anchoredPosition3D = customStartAnchoredPosition.vector3Value;
                    }

                    QUI.Space(SPACE_4);

                    if (QUI.GhostButton("Reset Position", QColors.Color.Blue, tempFloat * showCustomStartPosition.faded, 16 * showCustomStartPosition.faded))
                    {
                        customStartAnchoredPosition.vector3Value = Vector3.zero;
                    }

                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
            }
            #endregion

            QUI.Space(SPACE_4);

        }

        void DrawInAnimations(float width)
        {
            QUI.BeginHorizontal(width);
            {
                DrawMainBar("In Animations", uiElement.InAnimationsEnabled, loadInAnimationsPresetAtRuntime, showInAnimations, inAnimationsNewPreset, width, BarHeight);
                DrawMainGhostButtons(AnimType.In, BarHeight, BarHeight);
            }
            QUI.EndHorizontal();

            DrawDisabledInfoMessages("InAnimationsDisabled", uiElement.InAnimationsEnabled, loadInAnimationsPresetAtRuntime, showInAnimations, width);
            DrawLoadPresetInfoMessage("InAnimationsLoadPresetAtRuntime", loadInAnimationsPresetAtRuntime, inAnimationsPresetCategoryName.stringValue, inAnimationsPresetName.stringValue, showInAnimations, width);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showInAnimations.faded);
                if (QUI.BeginFadeGroup(showInAnimations.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showInAnimations.faded);
                        QUI.Space(SPACE_2 * showInAnimations.faded);
                        DUIUtils.DrawAnim(uiElement.inAnimations, uiElement, width - SPACE_8); //draw in animations - generic method
                        QUI.Space(SPACE_8 * showInAnimations.faded);
                        DrawInAnimationsEvents(width - SPACE_8);
                        QUI.Space(SPACE_16 * showInAnimations.faded);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_8 * (1 - showInAnimations.faded));
        }

        void DrawInAnimationsEvents(float width)
        {
            DrawUnityEvents((uiElement.InAnimationsEnabled || loadInAnimationsPresetAtRuntime.boolValue) && (uiElement.OnInAnimationsStart.GetPersistentEventCount() > 0 || uiElement.OnInAnimationsFinish.GetPersistentEventCount() > 0),
                            showInAnimationsEvents,
                            OnInAnimationsStart,
                            "OnInAnimationsStart",
                            OnInAnimationsFinish,
                            "OnInAnimationsFinish",
                            width);
        }

        void DrawOutAnimations(float width)
        {
            QUI.BeginHorizontal(width);
            {
                DrawMainBar("Out Animations", uiElement.OutAnimationsEnabled, loadOutAnimationsPresetAtRuntime, showOutAnimations, outAnimationsNewPreset, width, BarHeight);
                DrawMainGhostButtons(AnimType.Out, BarHeight, BarHeight);
            }
            QUI.EndHorizontal();

            DrawDisabledInfoMessages("OutAnimationsDisabled", uiElement.OutAnimationsEnabled, loadOutAnimationsPresetAtRuntime, showOutAnimations, width);
            DrawLoadPresetInfoMessage("OutAnimationsLoadPresetAtRuntime", loadOutAnimationsPresetAtRuntime, outAnimationsPresetCategoryName.stringValue, outAnimationsPresetName.stringValue, showOutAnimations, width);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOutAnimations.faded);
                if (QUI.BeginFadeGroup(showOutAnimations.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOutAnimations.faded);
                        QUI.Space(SPACE_2 * showOutAnimations.faded);
                        DUIUtils.DrawAnim(uiElement.outAnimations, uiElement, width - SPACE_8); //draw out animations - generic method
                        QUI.Space(SPACE_8 * showOutAnimations.faded);
                        DrawOutAnimationsEvents(width - SPACE_8);
                        QUI.Space(SPACE_16 * showOutAnimations.faded);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_8 * (1 - showOutAnimations.faded));
        }

        void DrawOutAnimationsEvents(float width)
        {
            DrawUnityEvents((uiElement.OutAnimationsEnabled || loadOutAnimationsPresetAtRuntime.boolValue) && (uiElement.OnOutAnimationsStart.GetPersistentEventCount() > 0 || uiElement.OnOutAnimationsFinish.GetPersistentEventCount() > 0),
                            showOutAnimationsEvents,
                            OnOutAnimationsStart,
                            "OnOutAnimationsStart",
                            OnOutAnimationsFinish,
                            "OnOutAnimationsFinish",
                            width);
        }

        void DrawDisabledInfoMessages(string disabledInfoMessageTag, bool enabled, SerializedProperty loadPresetAtRuntime, AnimBool show, float width)
        {
            infoMessage[disabledInfoMessageTag].show.target = !enabled && !loadPresetAtRuntime.boolValue; //check if the animations are disabled
            DrawInfoMessage(disabledInfoMessageTag, width); //draw warning if the animations are disabled
            QUI.Space(SPACE_4 * (1 - show.faded) * infoMessage[disabledInfoMessageTag].show.faded); //space added if the animations are disabled
        }
        void DrawLoadPresetInfoMessage(string loadPresetInfoMessageTag, SerializedProperty loadPresetAtRuntime, string categoryName, string presetName, AnimBool show, float width)
        {
            infoMessage[loadPresetInfoMessageTag].show.target = loadPresetAtRuntime.boolValue; //check if a preset is set to load at runtime
            infoMessage[loadPresetInfoMessageTag].title = "Runtime Preset: " + categoryName + " / " + presetName; //set the preset category and name that are set to load at runtime
            DrawInfoMessage(loadPresetInfoMessageTag, width); //draw info if a preset is set to load at runtime
            QUI.Space(SPACE_4 * (1 - show.faded) * infoMessage[loadPresetInfoMessageTag].show.faded); //space added if a preset is set to load at runtime
        }

        void DrawUnityEvents(bool enabled, AnimBool showEvents, SerializedProperty OnStart, string OnStartTitle, SerializedProperty OnFinish, string OnFinishTitle, float width)
        {
            if (QUI.GhostBar("Unity Events", enabled ? QColors.Color.Blue : QColors.Color.Gray, showEvents, width, MiniBarHeight))
            {
                showEvents.target = !showEvents.target;
            }
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showEvents.faded);
                if (QUI.BeginFadeGroup(showEvents.faded))
                {
                    QUI.SetGUIBackgroundColor(enabled ? QUI.AccentColorBlue : QUI.AccentColorGray);
                    QUI.BeginVertical(width - SPACE_16);
                    {
                        QUI.Space(SPACE_2 * showEvents.faded);
                        QUI.PropertyField(OnStart, new GUIContent() { text = OnStartTitle }, width - 8);
                        QUI.Space(SPACE_2 * showEvents.faded);
                        QUI.PropertyField(OnFinish, new GUIContent() { text = OnFinishTitle }, width - 8);
                        QUI.Space(SPACE_2 * showEvents.faded);
                    }
                    QUI.EndVertical();
                    QUI.ResetColors();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }

        void ResetNewPresetState()
        {
            inAnimationsNewPreset.target = false;
            outAnimationsNewPreset.target = false;
            loopAnimationsNewPreset.target = false;

            createNewCategoryName.target = false;

            QUI.ResetKeyboardFocus();
        }

        void DrawMainBar(string title, bool enabled, SerializedProperty loadPresetAtRuntime, AnimBool show, AnimBool newPreset, float width, float height)
        {
            if (QUI.GhostBar(title, !enabled && !loadPresetAtRuntime.boolValue ? QColors.Color.Gray : QColors.Color.Blue, show, width - height * 4, height))
            {
                show.target = !show.target;
                if (!show.target) //if closing -> reset any new preset settings
                {
                    if (newPreset.target)
                    {
                        ResetNewPresetState();
                        QUI.ExitGUI();
                    }
                }
            }
        }
        void DrawMainGhostButtons(AnimType animType, float width, float height)
        {
            switch (animType)
            {
                case AnimType.In:
                    if (QUI.GhostButton("M", uiElement.inAnimations.move.enabled ? QColors.Color.Green : QColors.Color.Gray, BarHeight, BarHeight, showInAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleMove" + animType);
                        uiElement.inAnimations.move.enabled = !uiElement.inAnimations.move.enabled;
                        if (uiElement.inAnimations.move.enabled) { showInAnimations.target = true; }
                    }
                    if (QUI.GhostButton("R", uiElement.inAnimations.rotate.enabled ? QColors.Color.Orange : QColors.Color.Gray, BarHeight, BarHeight, showInAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleRotate" + animType);
                        uiElement.inAnimations.rotate.enabled = !uiElement.inAnimations.rotate.enabled;
                        if (uiElement.inAnimations.rotate.enabled) { showInAnimations.target = true; }
                    }
                    if (QUI.GhostButton("S", uiElement.inAnimations.scale.enabled ? QColors.Color.Red : QColors.Color.Gray, BarHeight, BarHeight, showInAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleScale" + animType);
                        uiElement.inAnimations.scale.enabled = !uiElement.inAnimations.scale.enabled;
                        if (uiElement.inAnimations.scale.enabled) { showInAnimations.target = true; }
                    }
                    if (QUI.GhostButton("F", uiElement.inAnimations.fade.enabled ? QColors.Color.Purple : QColors.Color.Gray, BarHeight, BarHeight, showInAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleFade" + animType);
                        uiElement.inAnimations.fade.enabled = !uiElement.inAnimations.fade.enabled;
                        if (uiElement.inAnimations.fade.enabled) { showInAnimations.target = true; }
                    }
                    break;
                case AnimType.Out:
                    if (QUI.GhostButton("M", uiElement.outAnimations.move.enabled ? QColors.Color.Green : QColors.Color.Gray, BarHeight, BarHeight, showOutAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleMove" + animType);
                        uiElement.outAnimations.move.enabled = !uiElement.outAnimations.move.enabled;
                        if (uiElement.outAnimations.move.enabled) { showOutAnimations.target = true; }
                    }
                    if (QUI.GhostButton("R", uiElement.outAnimations.rotate.enabled ? QColors.Color.Orange : QColors.Color.Gray, BarHeight, BarHeight, showOutAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleRotate" + animType);
                        uiElement.outAnimations.rotate.enabled = !uiElement.outAnimations.rotate.enabled;
                        if (uiElement.outAnimations.rotate.enabled) { showOutAnimations.target = true; }
                    }
                    if (QUI.GhostButton("S", uiElement.outAnimations.scale.enabled ? QColors.Color.Red : QColors.Color.Gray, BarHeight, BarHeight, showOutAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleScale" + animType);
                        uiElement.outAnimations.scale.enabled = !uiElement.outAnimations.scale.enabled;
                        if (uiElement.outAnimations.scale.enabled) { showOutAnimations.target = true; }
                    }
                    if (QUI.GhostButton("F", uiElement.outAnimations.fade.enabled ? QColors.Color.Purple : QColors.Color.Gray, BarHeight, BarHeight, showOutAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleFade" + animType);
                        uiElement.outAnimations.fade.enabled = !uiElement.outAnimations.fade.enabled;
                        if (uiElement.outAnimations.fade.enabled) { showOutAnimations.target = true; }
                    }
                    break;             
            }

        }
    }
}
