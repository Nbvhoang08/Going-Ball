using QuickEditor;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Hapiga.UI.Editor
{
    [CustomEditor(typeof(UIButton), true)]
    [DisallowMultipleComponent]
    [CanEditMultipleObjects]
    public class UIButtonEditor : QEditor
    {

        UIButton uiButton { get { return (UIButton)target; } }

        DUISettings EditorSettings { get { return DUISettings.Instance; } }

#pragma warning disable 0414
        private SerializedProperty

            #region ButtonCategory

            buttonCategory,

            #endregion

            #region ButtonName

            buttonName,

            #endregion

            #region Settings

            allowMultipleClicks,
            disableButtonInterval,
            deselectButtonOnClick,

            #endregion

            #region PointerEnter

            #endregion

            #region PointerExit

            #endregion

            #region PointerDown

            #endregion

            #region PointerUp

            #endregion

            #region OnClick

            useOnClickAnimations,
            waitForOnClickAnimation,
            singleClickMode,
            OnClick,
            onClickAnimationType,
            onClickPunchPresetCategory,
            onClickPunchPresetName,
            loadOnClickPunchPresetAtRuntime,
            onClickStatePresetCategory,
            onClickStatePresetName,
            loadOnClickStatePresetAtRuntime,
            onClickGameEvents,

            #endregion

            #region OnDoubleClick

            useOnDoubleClick,
            onDoubleClickGameEvents,

            #endregion

            #region OnLongClick

            useOnLongClick,
            onLongClickGameEvents,

            #endregion

            #region NormalLoop

            normalLoopPresetCategory,
            normalLoopPresetName,
            loadNormalLoopPresetAtRuntime,
            normalLoop,
            normalLoopMove,
            normalLoopMoveEnabled,
            normalLoopRotate,
            normalLoopRotateEnabled,
            normalLoopScale,
            normalLoopScaleEnabled,
            normalLoopFade,
            normalLoopFadeEnabled,

            #endregion

            #region SelectedLoop

            selectedLoopPresetCategory,
            selectedLoopPresetName,
            loadSelectedLoopPresetAtRuntime,
            selectedLoop;
        #endregion

        AnimBool
            showOnPointerEnter, showOnPointerEnterEvents, showOnPointerEnterGameEvents, showOnPointerEnterNavigation,
            showOnPointerExit, showOnPointerExitEvents, showOnPointerExitGameEvents, showOnPointerExitNavigation,
            showOnPointerDown, showOnPointerDownEvents, showOnPointerDownGameEvents, showOnPointerDownNavigation,
            showOnPointerUp, showOnPointerUpEvents, showOnPointerUpGameEvents, showOnPointerUpNavigation,
            showOnClick, showOnClickEvents, showOnClickGameEvents, showOnClickNavigation,
            showOnDoubleClickNavigation,
            showOnLongClickNavigation,
            showNormalAnimation,
            showSelectedAnimation;

#pragma warning restore 0414

        Name newPresetCategoryName = new Name();
        Name newPresetName = new Name();

        AnimBool createNewCategoryName;
        AnimBool onPointerEnterPunchNewPreset;
        AnimBool onPointerEnterStateNewPreset;

        AnimBool onPointerExitPunchNewPreset;
        AnimBool onPointerExitStateNewPreset;

        AnimBool onPointerDownPunchNewPreset;
        AnimBool onPointerDownStateNewPreset;

        AnimBool onPointerUpPunchNewPreset;

        AnimBool onPointerUpStateNewPreset;

        Index onClickPunchPresetCategoryNameIndex = new Index();
        Index onClickPunchPresetNameIndex = new Index();
        AnimBool onClickPunchNewPreset;
        Index onClickStatePresetCategoryNameIndex = new Index();
        Index onClickStatePresetNameIndex = new Index();
        AnimBool onClickStateNewPreset;

        AnimBool onDoubleClickPunchNewPreset;

        AnimBool onDoubleClickStateNewPreset;

        AnimBool onLongClickPunchNewPreset;
        AnimBool onLongClickStateNewPreset;

        Index normalLoopPresetCategoryIndex = new Index();
        Index normalLoopPresetNameIndex = new Index();
        AnimBool normalLoopNewPreset;

        AnimBool selectedLoopNewPreset;


        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat = 0;
        //bool tempBool = false;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            #region ButtonCategory
            buttonCategory = serializedObject.FindProperty("buttonCategory");
            #endregion
            #region ButtonName
            buttonName = serializedObject.FindProperty("buttonName");
            #endregion
            #region Settings
            allowMultipleClicks = serializedObject.FindProperty("allowMultipleClicks");
            disableButtonInterval = serializedObject.FindProperty("disableButtonInterval");
            deselectButtonOnClick = serializedObject.FindProperty("deselectButtonOnClick");
            #endregion
            #region PointerEnter
           
            #endregion
            #region PointerExit
           
            #endregion
            #region PointerDown
          
            #endregion
            #region PointerUp
          
            #endregion
            #region OnClick
            useOnClickAnimations = serializedObject.FindProperty("useOnClickAnimations");
            waitForOnClickAnimation = serializedObject.FindProperty("waitForOnClickAnimation");
            singleClickMode = serializedObject.FindProperty("singleClickMode");
            OnClick = serializedObject.FindProperty("OnClick");
            onClickAnimationType = serializedObject.FindProperty("onClickAnimationType");
            onClickPunchPresetCategory = serializedObject.FindProperty("onClickPunchPresetCategory");
            onClickPunchPresetName = serializedObject.FindProperty("onClickPunchPresetName");
            loadOnClickPunchPresetAtRuntime = serializedObject.FindProperty("loadOnClickPunchPresetAtRuntime");
            onClickStatePresetCategory = serializedObject.FindProperty("onClickStatePresetCategory");
            onClickStatePresetName = serializedObject.FindProperty("onClickStatePresetName");
            loadOnClickStatePresetAtRuntime = serializedObject.FindProperty("loadOnClickStatePresetAtRuntime");
            onClickGameEvents = serializedObject.FindProperty("onClickGameEvents");
            #endregion
          
            #region NormalLoop
            normalLoopPresetCategory = serializedObject.FindProperty("normalLoopPresetCategory");
            normalLoopPresetName = serializedObject.FindProperty("normalLoopPresetName");
            loadNormalLoopPresetAtRuntime = serializedObject.FindProperty("loadNormalLoopPresetAtRuntime");
            normalLoop = serializedObject.FindProperty("normalLoop");
            normalLoopMove = normalLoop.FindPropertyRelative("move");
            normalLoopMoveEnabled = normalLoopMove.FindPropertyRelative("enabled");
            normalLoopRotate = normalLoop.FindPropertyRelative("rotate");
            normalLoopRotateEnabled = normalLoopRotate.FindPropertyRelative("enabled");
            normalLoopScale = normalLoop.FindPropertyRelative("scale");
            normalLoopScaleEnabled = normalLoopScale.FindPropertyRelative("enabled");
            normalLoopFade = normalLoop.FindPropertyRelative("fade");
            normalLoopFadeEnabled = normalLoopFade.FindPropertyRelative("enabled"); 
            #endregion
          
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("OnPointerEnterLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnPointerExitLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnPointerDownLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnPointerUpLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnClickLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnDoubleClickLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnLongClickLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("NormalLoopLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + normalLoopPresetCategory.stringValue + " / " + normalLoopPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadNormalLoopPresetAtRuntime.boolValue, Repaint)
                            });

        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showOnClick = new AnimBool(EditorSettings.autoExpandEnabledFeatures ? useOnClickAnimations.boolValue : false, Repaint);
            showOnClickEvents = new AnimBool(EditorSettings.autoExpandEnabledFeatures ? uiButton.OnClick.GetPersistentEventCount() > 0 : false, Repaint);
            showOnClickGameEvents = new AnimBool(EditorSettings.autoExpandEnabledFeatures ? onClickGameEvents.arraySize > 0 : false, Repaint);
            showNormalAnimation = new AnimBool(EditorSettings.autoExpandEnabledFeatures ? uiButton.normalLoop.Enabled || loadNormalLoopPresetAtRuntime.boolValue : false, Repaint);

            showSelectedAnimation = new AnimBool(false, Repaint);

            createNewCategoryName = new AnimBool(false, Repaint);

            onPointerEnterPunchNewPreset = new AnimBool(false, Repaint);
            onPointerEnterStateNewPreset = new AnimBool(false, Repaint);

            onPointerExitPunchNewPreset = new AnimBool(false, Repaint);
            onPointerExitStateNewPreset = new AnimBool(false, Repaint);

            onPointerDownPunchNewPreset = new AnimBool(false, Repaint);
            onPointerDownStateNewPreset = new AnimBool(false, Repaint);

            onPointerUpPunchNewPreset = new AnimBool(false, Repaint);
            onPointerUpStateNewPreset = new AnimBool(false, Repaint);

            onClickPunchNewPreset = new AnimBool(false, Repaint);
            onClickStateNewPreset = new AnimBool(false, Repaint);

            onDoubleClickPunchNewPreset = new AnimBool(false, Repaint);
            onDoubleClickStateNewPreset = new AnimBool(false, Repaint);

            onLongClickPunchNewPreset = new AnimBool(false, Repaint);
            onLongClickStateNewPreset = new AnimBool(false, Repaint);

            normalLoopNewPreset = new AnimBool(false, Repaint);
            selectedLoopNewPreset = new AnimBool(false, Repaint);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;
        }

        public override void OnInspectorGUI()
        {

            if(IsEditorLocked) { return; }

            serializedObject.Update();

            DrawDatabaseButtons(GlobalWidth);
            QUI.Space(SPACE_8);

            DrawSettings(GlobalWidth);
            QUI.Space(SPACE_4);

            //OnClick
            if(EditorSettings.UIButton_Inspector_HideOnClick)
            {
                useOnClickAnimations.boolValue = false;
            }
            else
            {
                DrawOnClick(GlobalWidth);
                QUI.Space(SPACE_4);
            }
            //NormalLoop
            if(EditorSettings.UIButton_Inspector_HideNormalLoop)
            {
                loadNormalLoopPresetAtRuntime.boolValue = false;

                normalLoopMoveEnabled.boolValue = false;
                normalLoopRotateEnabled.boolValue = false;
                normalLoopScaleEnabled.boolValue = false;
                normalLoopFadeEnabled.boolValue = false;
            }
            else
            {
                DrawNormalLoop(GlobalWidth);
                QUI.Space(-SPACE_4);
            }

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawDatabaseButtons(float width)
        {
            
        }


        void DrawSettings(float width)
        {
            QUI.BeginHorizontal(width);
            {
                //ALLOW MULTIPLE CLICKS
                QUI.QToggle("allow multiple clicks", allowMultipleClicks);

                QUI.FlexibleSpace();

                if(allowMultipleClicks.boolValue) { GUI.enabled = false; }

                QLabel.text = "disable button interval";
                QLabel.style = Style.Text.Normal;

                tempFloat = QLabel.x; //save first label width
                tempFloat += 40; //add property field width

                QLabel.text = "seconds";
                QLabel.style = Style.Text.Normal;

                tempFloat += QLabel.x; //add second label width
                tempFloat += 8; //add extra space
                tempFloat += 24; //compensate for unity margins

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, allowMultipleClicks.boolValue ? QColors.Color.Gray : QColors.Color.Blue), tempFloat, 18);
                QUI.Space(-tempFloat + 4);

                QLabel.text = "disable button interval";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                //DISABLE BUTTON INTERVAL
                QUI.PropertyField(disableButtonInterval, 40);

                QLabel.text = "seconds";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                QUI.Space(SPACE_4);

                GUI.enabled = true;
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_4);

            //DESELECT ON BUTTON CLICK
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("deselect button on click", deselectButtonOnClick);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_4);

        }

        void DrawOnClick(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnClick", useOnClickAnimations, showOnClick, width, BarHeight);
            if(loadOnClickPunchPresetAtRuntime.boolValue) { loadOnClickStatePresetAtRuntime.boolValue = false; }
            if(loadOnClickStatePresetAtRuntime.boolValue) { loadOnClickPunchPresetAtRuntime.boolValue = false; }
            DUIUtils.DrawLoadPresetInfoMessage("OnClickLoadPresetAtRuntime",
                                               infoMessage,
                                               useOnClickAnimations.boolValue && (loadOnClickPunchPresetAtRuntime.boolValue || loadOnClickStatePresetAtRuntime.boolValue),
                                               loadOnClickPunchPresetAtRuntime.boolValue ? onClickPunchPresetCategory.stringValue : onClickStatePresetCategory.stringValue,
                                               loadOnClickPunchPresetAtRuntime.boolValue ? onClickPunchPresetName.stringValue : onClickStatePresetName.stringValue,
                                               showOnClick,
                                               ((UIButton.ButtonAnimationType)onClickAnimationType.enumValueIndex).ToString(),
                                               width);

            if(!useOnClickAnimations.boolValue) { return; }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOnClick.faded);
                if(QUI.BeginFadeGroup(showOnClick.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        DrawOnClickSettings(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        DrawOnClickPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        switch(uiButton.onClickAnimationType)
                        {
                            case UIButton.ButtonAnimationType.Punch: DUIUtils.DrawPunch(uiButton.onClickPunch, uiButton, width - SPACE_8); break;
                            case UIButton.ButtonAnimationType.State: DUIUtils.DrawAnim(uiButton.onClickState, uiButton, width - SPACE_8); break;
                        }
                        QUI.Space(SPACE_4 * showOnClick.faded);
                        DrawOnClickSound(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnClick.faded);
                        DrawOnClickEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        DrawOnClickGameEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        DrawOnClickNavigation(width - SPACE_8);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();
        }
        void DrawOnClickSettings(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("wait for animation", waitForOnClickAnimation);
                QUI.FlexibleSpace();

                QLabel.text = "single click mode";
                QLabel.style = Style.Text.Normal;

                tempFloat = QLabel.x; //save label width
                tempFloat += 80; //add dropdown width
                tempFloat += 8; //add extra space
                tempFloat += 12; //compensate for unity margins

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat, 20);
                QUI.Space(-tempFloat + SPACE_4);

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();
                QUI.PropertyField(singleClickMode, 80);
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }
        void DrawOnClickPreset(float width)
        {
            DrawPreset(onClickAnimationType,
                       ButtonAnimType.OnClick,
                       loadOnClickPunchPresetAtRuntime, onClickPunchNewPreset, onClickPunchPresetCategoryNameIndex, onClickPunchPresetCategory, onClickPunchPresetNameIndex, onClickPunchPresetName,
                       loadOnClickStatePresetAtRuntime, onClickStateNewPreset, onClickStatePresetCategoryNameIndex, onClickStatePresetCategory, onClickStatePresetNameIndex, onClickStatePresetName,
                       width);
        }
        void DrawOnClickSound(float width)
        {
          //  DUIUtils.DrawSound(this, "Play Sound", customOnClickSound, onClickSound, onClickSoundIndex, width);
        }
        void DrawOnClickEvents(float width)
        {
            //DUIUtils.DrawUnityEvents(uiButton.OnClick.GetPersistentEventCount() > 0, showOnClickEvents, OnClick, "OnClick", width, MiniBarHeight);
        }
        void DrawOnClickGameEvents(float width)
        {
            //DrawGameEvents("OnClick", showOnClickGameEvents, onClickGameEvents, width);
        }
        void DrawOnClickNavigation(float width)
        {
           // DUIUtils.DrawNavigation(this, uiButton.onClickNavigation, onClickEditorNavigationData, showOnClickNavigation, UpdateAllNavigationData, true, uiButton.IsBackButton, width, MiniBarHeight);
        }

        void DrawNormalLoop(float width)
        {
            DrawLoopAnimations(ButtonLoopType.Normal, uiButton.normalLoop.Enabled, loadNormalLoopPresetAtRuntime, showNormalAnimation, normalLoopPresetCategoryIndex, normalLoopPresetCategory, normalLoopPresetNameIndex, normalLoopPresetName, normalLoopNewPreset, width);
        }

        void DrawGameEvents(string tag, AnimBool show, SerializedProperty gameEvents, float width)
        {
            QUI.DrawCollapsableList("Game Events", show, gameEvents.arraySize > 0 ? QColors.Color.Blue : QColors.Color.Gray, gameEvents, width, MiniBarHeight, "Not sending any Game Events on " + tag + "... Click [+] to start...");
        }

        void ResetNewPreset()
        {
            onPointerEnterPunchNewPreset.target = false;
            onPointerEnterStateNewPreset.target = false;

            onPointerExitPunchNewPreset.target = false;
            onPointerExitStateNewPreset.target = false;

            onPointerDownPunchNewPreset.target = false;
            onPointerDownStateNewPreset.target = false;

            onPointerUpPunchNewPreset.target = false;
            onPointerUpStateNewPreset.target = false;

            onClickPunchNewPreset.target = false;
            onClickStateNewPreset.target = false;

            onDoubleClickPunchNewPreset.target = false;
            onDoubleClickStateNewPreset.target = false;

            onLongClickPunchNewPreset.target = false;
            onLongClickStateNewPreset.target = false;

            normalLoopNewPreset.target = false;
            selectedLoopNewPreset.target = false;

            createNewCategoryName.target = false;
            newPresetCategoryName.name = string.Empty;
            newPresetName.name = string.Empty;

            QUI.ResetKeyboardFocus();
        }
        void DrawLoopBar(string title, bool enabled, SerializedProperty loadPresetAtRuntime, AnimBool show, AnimBool newPreset, float width, float height)
        {
            if(QUI.GhostBar(title, !enabled && !loadPresetAtRuntime.boolValue ? QColors.Color.Gray : QColors.Color.Blue, show, width - height * 4, height))
            {
                show.target = !show.target;
                if(!show.target) //if closing -> reset any new preset settings
                {
                    if(newPreset.target)
                    {
                        ResetNewPreset();
                        QUI.ExitGUI();
                    }
                }
            }
        }
        void DrawLoopButtons(ButtonLoopType loopType, float width, float height)
        {
            switch(loopType)
            {
                case ButtonLoopType.Normal:
                    if(QUI.GhostButton("M", uiButton.normalLoop.move.enabled ? QColors.Color.Green : QColors.Color.Gray, BarHeight, BarHeight, showNormalAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleMove" + loopType);
                        uiButton.normalLoop.move.enabled = !uiButton.normalLoop.move.enabled;
                        if(uiButton.normalLoop.move.enabled) { showNormalAnimation.target = true; }
                    }
                    if(QUI.GhostButton("R", uiButton.normalLoop.rotate.enabled ? QColors.Color.Orange : QColors.Color.Gray, BarHeight, BarHeight, showNormalAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleRotate" + loopType);
                        uiButton.normalLoop.rotate.enabled = !uiButton.normalLoop.rotate.enabled;
                        if(uiButton.normalLoop.rotate.enabled) { showNormalAnimation.target = true; }
                    }
                    if(QUI.GhostButton("S", uiButton.normalLoop.scale.enabled ? QColors.Color.Red : QColors.Color.Gray, BarHeight, BarHeight, showNormalAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleScale" + loopType);
                        uiButton.normalLoop.scale.enabled = !uiButton.normalLoop.scale.enabled;
                        if(uiButton.normalLoop.scale.enabled) { showNormalAnimation.target = true; }
                    }
                    if(QUI.GhostButton("F", uiButton.normalLoop.fade.enabled ? QColors.Color.Purple : QColors.Color.Gray, BarHeight, BarHeight, showNormalAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleFade" + loopType);
                        uiButton.normalLoop.fade.enabled = !uiButton.normalLoop.fade.enabled;
                        if(uiButton.normalLoop.fade.enabled) { showNormalAnimation.target = true; }
                    }
                    break;
            }
        }

        void DrawLoopAnimations(ButtonLoopType loopType, bool enabled, SerializedProperty loadPresetAtRuntime, AnimBool showAnimation, Index presetCategoryIndex, SerializedProperty presetCategory, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            QUI.BeginHorizontal(width);
            {
                DrawLoopBar(loopType + " Loop Animations", enabled, loadPresetAtRuntime, showAnimation, newPreset, width, BarHeight);
                DrawLoopButtons(loopType, BarHeight, BarHeight);
            }
            QUI.EndHorizontal();

            DUIUtils.DrawLoadPresetInfoMessage(loopType + "LoopLoadPresetAtRuntime", infoMessage, loadPresetAtRuntime.boolValue, presetCategory.stringValue, presetName.stringValue, showAnimation, loopType.ToString(), width);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showAnimation.faded);
                if(QUI.BeginFadeGroup(showAnimation.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showAnimation.faded);
                        DUIUtils.DrawLoop(uiButton.normalLoop, target, width - SPACE_8); //draw loop animations - generic method
                        QUI.Space(SPACE_16 * showAnimation.faded);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_8 * (1 - showAnimation.faded));
        }
        void DrawLoopAnimationsPreset(ButtonLoopType loopType, SerializedProperty loadPresetAtRuntime, Index presetCategoryIndex, SerializedProperty presetCategory, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            QUI.Space(SPACE_2);
           // DUIUtils.DrawLoadPresetAtRuntime(loadPresetAtRuntime, width);
            DUIUtils.DrawPresetBackground(newPreset, createNewCategoryName, width);

            if(newPreset.faded < 0.5f) //NORMAL VIEW
            {
                DrawLoopPresetNormalView(presetCategoryIndex, presetCategory, presetNameIndex, presetName, newPreset, width);
                QUI.Space(SPACE_2);
                DrawLoopPresetNormalViewPresetButtons(loopType, presetCategory, presetName, newPreset, width);

            }
            else //NEW PRESET VIEW
            {
                DrawLoopPresetNewPresetView(presetCategoryIndex, presetCategory, newPreset, width);
                QUI.Space(SPACE_2);
                DrawLoopPresetNewPresetViewPresetButtons(loopType, presetCategoryIndex, presetCategory, presetCategoryIndex, presetName, newPreset, width);
            }
        }
        void DrawLoopPresetNormalView(Index presetCategoryIndex, SerializedProperty presetCategory, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            tempFloat = (width - 6) / 2 - 5; //dropdown lists width
        }
        void DrawLoopPresetNormalViewPresetButtons(ButtonLoopType loopType, SerializedProperty presetCategory, SerializedProperty presetName, AnimBool newPreset, float width)
        {
           
        }
        void DrawLoopPresetNewPresetView(Index presetCategoryIndex, SerializedProperty presetCategory, AnimBool newPreset, float width)
        {
            tempFloat = (width - 6) / 2 - 5; //dropdown lists width
        }
        void DrawLoopPresetNewPresetViewPresetButtons(ButtonLoopType loopType, Index presetCategoryIndex, SerializedProperty presetCategoryName, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
        {
           
        }

        void DrawPreset(SerializedProperty animationType,
                        ButtonAnimType buttonAnimType,
                        SerializedProperty loadPunchPresetAtRuntime, AnimBool punchNewPreset, Index punchPresetCategoryNameIndex, SerializedProperty punchPresetCategory, Index punchPresetNameIndex, SerializedProperty punchPresetName,
                        SerializedProperty loadStatePresetAtRuntime, AnimBool stateNewPreset, Index statePresetCategoryNameIndex, SerializedProperty statePresetCategory, Index statePresetNameIndex, SerializedProperty statePresetName,
                        float width)
        {
            QUI.Space(SPACE_2);
            QUI.QObjectPropertyField("Animation Type", animationType, width, 20, false);
            QUI.Space(SPACE_2);
           
        }
    }
}
