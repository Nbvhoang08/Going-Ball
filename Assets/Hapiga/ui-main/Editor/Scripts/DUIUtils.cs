using QuickEditor;
using System.Collections.Generic;
using Hapiga.UI;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Hapiga.UI.Editor
{
    public class Index
    {
        public int index;

        public Index()
        {
            this.index = 0;
        }

        public Index(int index)
        {
            this.index = index;
        }
    }

    public class Name
    {
        public string name;

        public Name()
        {
            this.name = string.Empty;
        }

        public Name(string name)
        {
            this.name = name;
        }
    }

    public enum ButtonAnimType { OnClick }
    public enum ToggleAnimType { OnPointerEnter, OnPointerExit, OnClick }

    public enum ButtonLoopType { Normal, Selected }
}

namespace  Hapiga.UI.Editor
{
    public class DUIUtils
    {
        public static Color AccentColorBlue { get { return QUI.IsProSkin ? QColors.Blue.Color : QColors.BlueLight.Color; } }
        public static Color AccentColorGray { get { return QUI.IsProSkin ? QColors.UnityLight.Color : QColors.UnityLight.Color; } }

        private static bool result;

        #region BARS
        public static bool MoveSlicedBar(string text, QColors.Color color, AnimBool aBool, float width, float height = 16)
        {
            result = false;
            if(height > 16)
            {
                QUI.GetSlicedBarStyle(color, aBool.value).fontSize = Mathf.RoundToInt(height * 0.6f);
            }
            QUI.BeginVertical(width, height);
            {
                if(QUI.Button(text, QUI.GetSlicedBarStyle(color, aBool.value), width, height))
                {
                    result = true;
                }
                QUI.Space(-height);
                QUI.FlexibleSpace();
                DrawMoveCaret(aBool);
                QUI.FlexibleSpace();
            }
            QUI.EndVertical();
            if(height > 16)
            {
                QUI.GetSlicedBarStyle(color, aBool.value).fontSize = QStyles.GetTextFontSize(Style.Text.Bar);
            }
            return result;
        }
        public static bool RotateSlicedBar(string text, QColors.Color color, AnimBool aBool, float width, float height = 16)
        {
            result = false;
            if(height > 16)
            {
                QUI.GetSlicedBarStyle(color, aBool.value).fontSize = Mathf.RoundToInt(height * 0.6f);
            }
            QUI.BeginVertical(width, height);
            {
                if(QUI.Button(text, QUI.GetSlicedBarStyle(color, aBool.value), width, height))
                {
                    result = true;
                }
                QUI.Space(-height);
                QUI.FlexibleSpace();
                DrawRotateCaret(aBool);
                QUI.FlexibleSpace();
            }
            QUI.EndVertical();
            if(height > 16)
            {
                QUI.GetSlicedBarStyle(color, aBool.value).fontSize = QStyles.GetTextFontSize(Style.Text.Bar);
            }
            return result;
        }
        public static bool ScaleSlicedBar(string text, QColors.Color color, AnimBool aBool, float width, float height = 16)
        {
            result = false;
            if(height > 16)
            {
                QUI.GetSlicedBarStyle(color, aBool.value).fontSize = Mathf.RoundToInt(height * 0.6f);
            }
            QUI.BeginVertical(width, height);
            {
                if(QUI.Button(text, QUI.GetSlicedBarStyle(color, aBool.value), width, height))
                {
                    result = true;
                }
                QUI.Space(-height);
                QUI.FlexibleSpace();
                DrawScaleCaret(aBool);
                QUI.FlexibleSpace();
            }
            QUI.EndVertical();
            if(height > 16)
            {
                QUI.GetSlicedBarStyle(color, aBool.value).fontSize = QStyles.GetTextFontSize(Style.Text.Bar);
            }
            return result;
        }
        public static bool FadeSlicedBar(string text, QColors.Color color, AnimBool aBool, float width, float height = 16)
        {
            result = false;
            if(height > 16)
            {
                QUI.GetSlicedBarStyle(color, aBool.value).fontSize = Mathf.RoundToInt(height * 0.6f);
            }
            QUI.BeginVertical(width, height);
            {
                if(QUI.Button(text, QUI.GetSlicedBarStyle(color, aBool.value), width, height))
                {
                    result = true;
                }
                QUI.Space(-height);
                QUI.FlexibleSpace();
                DrawFadeCaret(aBool);
                QUI.FlexibleSpace();
            }
            QUI.EndVertical();
            if(height > 16)
            {
                QUI.GetSlicedBarStyle(color, aBool.value).fontSize = QStyles.GetTextFontSize(Style.Text.Bar);
            }
            return result;
        }

        public static void DrawPresetCaret(AnimBool aBool)
        {
            QUI.BeginHorizontal(16, 16);
            {
                QUI.Space(4);
                if(aBool.faded == 0) { QUI.DrawTexture(DUIResources.presetCaret10.texture, 16, 16); }
                else if(aBool.faded <= 0.1f) { QUI.DrawTexture(DUIResources.presetCaret9.texture, 16, 16); }
                else if(aBool.faded <= 0.2f) { QUI.DrawTexture(DUIResources.presetCaret8.texture, 16, 16); }
                else if(aBool.faded <= 0.3f) { QUI.DrawTexture(DUIResources.presetCaret7.texture, 16, 16); }
                else if(aBool.faded <= 0.4f) { QUI.DrawTexture(DUIResources.presetCaret6.texture, 16, 16); }
                else if(aBool.faded <= 0.5f) { QUI.DrawTexture(DUIResources.presetCaret5.texture, 16, 16); }
                else if(aBool.faded <= 0.6f) { QUI.DrawTexture(DUIResources.presetCaret4.texture, 16, 16); }
                else if(aBool.faded <= 0.7f) { QUI.DrawTexture(DUIResources.presetCaret3.texture, 16, 16); }
                else if(aBool.faded <= 0.8f) { QUI.DrawTexture(DUIResources.presetCaret2.texture, 16, 16); }
                else if(aBool.faded <= 0.9f) { QUI.DrawTexture(DUIResources.presetCaret1.texture, 16, 16); }
                else { QUI.DrawTexture(DUIResources.presetCaret0.texture, 16, 16); }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        public static void DrawMoveCaret(AnimBool aBool)
        {
            QUI.BeginHorizontal(16, 16);
            {
                QUI.Space(4);
                if(aBool.faded == 0) { QUI.DrawTexture(DUIResources.moveCaret10.texture, 16, 16); }
                else if(aBool.faded <= 0.1f) { QUI.DrawTexture(DUIResources.moveCaret9.texture, 16, 16); }
                else if(aBool.faded <= 0.2f) { QUI.DrawTexture(DUIResources.moveCaret8.texture, 16, 16); }
                else if(aBool.faded <= 0.3f) { QUI.DrawTexture(DUIResources.moveCaret7.texture, 16, 16); }
                else if(aBool.faded <= 0.4f) { QUI.DrawTexture(DUIResources.moveCaret6.texture, 16, 16); }
                else if(aBool.faded <= 0.5f) { QUI.DrawTexture(DUIResources.moveCaret5.texture, 16, 16); }
                else if(aBool.faded <= 0.6f) { QUI.DrawTexture(DUIResources.moveCaret4.texture, 16, 16); }
                else if(aBool.faded <= 0.7f) { QUI.DrawTexture(DUIResources.moveCaret3.texture, 16, 16); }
                else if(aBool.faded <= 0.8f) { QUI.DrawTexture(DUIResources.moveCaret2.texture, 16, 16); }
                else if(aBool.faded <= 0.9f) { QUI.DrawTexture(DUIResources.moveCaret1.texture, 16, 16); }
                else { QUI.DrawTexture(DUIResources.moveCaret0.texture, 16, 16); }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        public static void DrawRotateCaret(AnimBool aBool)
        {
            QUI.BeginHorizontal(16, 16);
            {
                QUI.Space(4);
                if(aBool.faded == 0) { QUI.DrawTexture(DUIResources.rotateCaret10.texture, 16, 16); }
                else if(aBool.faded <= 0.1f) { QUI.DrawTexture(DUIResources.rotateCaret9.texture, 16, 16); }
                else if(aBool.faded <= 0.2f) { QUI.DrawTexture(DUIResources.rotateCaret8.texture, 16, 16); }
                else if(aBool.faded <= 0.3f) { QUI.DrawTexture(DUIResources.rotateCaret7.texture, 16, 16); }
                else if(aBool.faded <= 0.4f) { QUI.DrawTexture(DUIResources.rotateCaret6.texture, 16, 16); }
                else if(aBool.faded <= 0.5f) { QUI.DrawTexture(DUIResources.rotateCaret5.texture, 16, 16); }
                else if(aBool.faded <= 0.6f) { QUI.DrawTexture(DUIResources.rotateCaret4.texture, 16, 16); }
                else if(aBool.faded <= 0.7f) { QUI.DrawTexture(DUIResources.rotateCaret3.texture, 16, 16); }
                else if(aBool.faded <= 0.8f) { QUI.DrawTexture(DUIResources.rotateCaret2.texture, 16, 16); }
                else if(aBool.faded <= 0.9f) { QUI.DrawTexture(DUIResources.rotateCaret1.texture, 16, 16); }
                else { QUI.DrawTexture(DUIResources.rotateCaret0.texture, 16, 16); }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        public static void DrawScaleCaret(AnimBool aBool)
        {
            QUI.BeginHorizontal(16, 16);
            {
                QUI.Space(4);
                if(aBool.faded == 0) { QUI.DrawTexture(DUIResources.scaleCaret10.texture, 16, 16); }
                else if(aBool.faded <= 0.1f) { QUI.DrawTexture(DUIResources.scaleCaret9.texture, 16, 16); }
                else if(aBool.faded <= 0.2f) { QUI.DrawTexture(DUIResources.scaleCaret8.texture, 16, 16); }
                else if(aBool.faded <= 0.3f) { QUI.DrawTexture(DUIResources.scaleCaret7.texture, 16, 16); }
                else if(aBool.faded <= 0.4f) { QUI.DrawTexture(DUIResources.scaleCaret6.texture, 16, 16); }
                else if(aBool.faded <= 0.5f) { QUI.DrawTexture(DUIResources.scaleCaret5.texture, 16, 16); }
                else if(aBool.faded <= 0.6f) { QUI.DrawTexture(DUIResources.scaleCaret4.texture, 16, 16); }
                else if(aBool.faded <= 0.7f) { QUI.DrawTexture(DUIResources.scaleCaret3.texture, 16, 16); }
                else if(aBool.faded <= 0.8f) { QUI.DrawTexture(DUIResources.scaleCaret2.texture, 16, 16); }
                else if(aBool.faded <= 0.9f) { QUI.DrawTexture(DUIResources.scaleCaret1.texture, 16, 16); }
                else { QUI.DrawTexture(DUIResources.scaleCaret0.texture, 16, 16); }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        public static void DrawFadeCaret(AnimBool aBool)
        {
            QUI.BeginHorizontal(16, 16);
            {
                QUI.Space(4);
                if(aBool.faded == 0) { QUI.DrawTexture(DUIResources.fadeCaret10.texture, 16, 16); }
                else if(aBool.faded <= 0.1f) { QUI.DrawTexture(DUIResources.fadeCaret9.texture, 16, 16); }
                else if(aBool.faded <= 0.2f) { QUI.DrawTexture(DUIResources.fadeCaret8.texture, 16, 16); }
                else if(aBool.faded <= 0.3f) { QUI.DrawTexture(DUIResources.fadeCaret7.texture, 16, 16); }
                else if(aBool.faded <= 0.4f) { QUI.DrawTexture(DUIResources.fadeCaret6.texture, 16, 16); }
                else if(aBool.faded <= 0.5f) { QUI.DrawTexture(DUIResources.fadeCaret5.texture, 16, 16); }
                else if(aBool.faded <= 0.6f) { QUI.DrawTexture(DUIResources.fadeCaret4.texture, 16, 16); }
                else if(aBool.faded <= 0.7f) { QUI.DrawTexture(DUIResources.fadeCaret3.texture, 16, 16); }
                else if(aBool.faded <= 0.8f) { QUI.DrawTexture(DUIResources.fadeCaret2.texture, 16, 16); }
                else if(aBool.faded <= 0.9f) { QUI.DrawTexture(DUIResources.fadeCaret1.texture, 16, 16); }
                else { QUI.DrawTexture(DUIResources.fadeCaret0.texture, 16, 16); }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        #endregion

        #region Reusable Editor Draw Modules
        static QLabel _qLabel;
        static QLabel QLabel { get { if(_qLabel == null) { _qLabel = new QLabel(); } return _qLabel; } }
        static float tempWidth;
        static float tempFloat;
        static int tempInt;
        static Vector3 tempVector3;

        public static void DrawAnim(Anim anim, Object targetObject, float width)
        {
            anim.UpdateAnimBools();
            QUI.Space(4);
            DrawAnimMove(anim, targetObject, width);
            QUI.Space(4);
            DrawAnimRotate(anim, targetObject, width);
            QUI.Space(4);
            DrawAnimScale(anim, targetObject, width);
            QUI.Space(4);
            DrawAnimFade(anim, targetObject, width);
            QUI.Space(4);
        }
        public static void DrawAnimMove(Anim anim, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Green), width, 18 + 42 * anim.moveIsExpanded.faded);
            QUI.Space(-(18 + 42 * anim.moveIsExpanded.faded));
            if(DUIUtils.MoveSlicedBar("Move", anim.moveIsExpanded.target ? QColors.Color.Green : QColors.Color.Gray, anim.moveIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleMove");
                anim.move.enabled = !anim.move.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(anim.moveIsExpanded.target, width);

            if(QUI.BeginFadeGroup(anim.moveIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.Green.Color : QColors.GreenLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = anim.move.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            anim.move.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = anim.move.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            anim.move.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        if(anim.move.animationType != Anim.AnimationType.State) //what to draw if this is an IN or an OUT animation
                        {
                            QLabel.text = "move" + (anim.move.animationType == Anim.AnimationType.In ? " from" : " to");
                            QUI.Label(QLabel);
                            tempWidth -= QLabel.x;
                            Move.MoveDirection moveDirection = anim.move.moveDirection;
                            QUI.BeginChangeCheck();
                            moveDirection = (Move.MoveDirection)EditorGUILayout.EnumPopup(moveDirection, GUILayout.Width(tempWidth - 28));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateMoveDirection");
                                anim.move.moveDirection = moveDirection;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        else //what to draw if this is a STATE animation
                        {
                            QLabel.text = "move by";
                            QUI.Label(QLabel);
                            tempWidth -= QLabel.x;
                            Vector3 customPosition = anim.move.customPosition;
                            QUI.BeginChangeCheck();
                            customPosition = EditorGUILayout.Vector3Field("", customPosition, GUILayout.Width(tempWidth - 28));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateMove");
                                anim.move.customPosition = customPosition;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        UIAnimator.EaseType easeType = anim.move.easeType;
                        QUI.BeginChangeCheck();
                        easeType = (UIAnimator.EaseType)EditorGUILayout.EnumPopup(easeType, GUILayout.Width(110));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateEaseType");
                            anim.move.easeType = easeType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 110;
                        if((anim.move.moveDirection == Move.MoveDirection.CustomPosition))
                        {
                            if(anim.move.easeType == UIAnimator.EaseType.Ease)
                            {
                                DG.Tweening.Ease ease = anim.move.ease;
                                QUI.BeginChangeCheck();
                                ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(ease, GUILayout.Width(100));
                                if(QUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(targetObject, "UpdateEase");
                                    anim.move.ease = ease;
                                    QUI.SetDirty(targetObject);
                                    AssetDatabase.SaveAssets();
                                }
                            }
                            else
                            {
                                AnimationCurve animationCurve = anim.move.animationCurve;
                                QUI.BeginChangeCheck();
                                animationCurve = EditorGUILayout.CurveField(animationCurve, GUILayout.Width(100));
                                if(QUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(targetObject, "UpdateCurve");
                                    anim.move.animationCurve = animationCurve;
                                    QUI.SetDirty(targetObject);
                                    AssetDatabase.SaveAssets();
                                }
                            }
                            tempWidth -= 100;
                            QUI.Space(2);
                            tempWidth -= 2;
                            Vector3 customPosition = anim.move.customPosition;
                            QUI.BeginChangeCheck();
                            customPosition = EditorGUILayout.Vector3Field("", customPosition, GUILayout.Width(tempWidth - 18));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateCustomPosition");
                                anim.move.customPosition = customPosition;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        else
                        {
                            if(anim.move.easeType == UIAnimator.EaseType.Ease)
                            {
                                DG.Tweening.Ease ease = anim.move.ease;
                                QUI.BeginChangeCheck();
                                ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(ease, GUILayout.Width(tempWidth - 12));
                                if(QUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(targetObject, "UpdateEase");
                                    anim.move.ease = ease;
                                    QUI.SetDirty(targetObject);
                                    AssetDatabase.SaveAssets();
                                }
                            }
                            else
                            {
                                AnimationCurve animationCurve = anim.move.animationCurve;
                                QUI.BeginChangeCheck();
                                animationCurve = EditorGUILayout.CurveField(animationCurve, GUILayout.Width(tempWidth - 14));
                                if(QUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(targetObject, "UpdateCurve");
                                    anim.move.animationCurve = animationCurve;
                                    QUI.SetDirty(targetObject);
                                    AssetDatabase.SaveAssets();
                                }
                            }
                        }

                    }
                    QUI.EndHorizontal();

                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        public static void DrawAnimRotate(Anim anim, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Orange), width, 18 + 42 * anim.rotateIsExpanded.faded);
            QUI.Space(-(18 + 42 * anim.rotateIsExpanded.faded));
            if(DUIUtils.RotateSlicedBar("Rotate", anim.rotateIsExpanded.target ? QColors.Color.Orange : QColors.Color.Gray, anim.rotateIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleRotate");
                anim.rotate.enabled = !anim.rotate.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(anim.rotateIsExpanded.target, width);

            if(QUI.BeginFadeGroup(anim.rotateIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.Orange.Color : QColors.OrangeLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = anim.rotate.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            anim.rotate.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = anim.rotate.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            anim.rotate.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = anim.move.animationType != Anim.AnimationType.State ? "rotation" : "rotate by";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        Vector3 rotation = anim.rotate.rotation;
                        QUI.BeginChangeCheck();
                        rotation = EditorGUILayout.Vector3Field("", rotation, GUILayout.Width(tempWidth - 28));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateRotation");
                            anim.rotate.rotation = rotation;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        UIAnimator.EaseType easeType = anim.rotate.easeType;
                        QUI.BeginChangeCheck();
                        easeType = (UIAnimator.EaseType)EditorGUILayout.EnumPopup(easeType, GUILayout.Width(110));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateEaseType");
                            anim.rotate.easeType = easeType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 110;
                        if(anim.rotate.easeType == UIAnimator.EaseType.Ease)
                        {
                            DG.Tweening.Ease ease = anim.rotate.ease;
                            QUI.BeginChangeCheck();
                            ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(ease, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateEase");
                                anim.rotate.ease = ease;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        else
                        {
                            AnimationCurve animationCurve = anim.rotate.animationCurve;
                            QUI.BeginChangeCheck();
                            animationCurve = EditorGUILayout.CurveField(animationCurve, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateCurve");
                                anim.rotate.animationCurve = animationCurve;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                    }
                    QUI.EndHorizontal();
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        public static void DrawAnimScale(Anim anim, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Red), width, 18 + 42 * anim.scaleIsExpanded.faded);
            QUI.Space(-(18 + 42 * anim.scaleIsExpanded.faded));
            if(DUIUtils.ScaleSlicedBar("Scale", anim.scaleIsExpanded.target ? QColors.Color.Red : QColors.Color.Gray, anim.scaleIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleScale");
                anim.scale.enabled = !anim.scale.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(anim.scaleIsExpanded.target, width);

            if(QUI.BeginFadeGroup(anim.scaleIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.RedLight.Color : QColors.RedLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = anim.scale.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            anim.scale.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = anim.scale.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            anim.scale.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = anim.move.animationType != Anim.AnimationType.State ? "scale" : "scale by";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        Vector3 scale = anim.scale.scale;
                        QUI.BeginChangeCheck();
                        scale = EditorGUILayout.Vector3Field("", scale, GUILayout.Width(tempWidth - 28));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateScale");
                            anim.scale.scale = scale;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        UIAnimator.EaseType easeType = anim.scale.easeType;
                        QUI.BeginChangeCheck();
                        easeType = (UIAnimator.EaseType)EditorGUILayout.EnumPopup(easeType, GUILayout.Width(110));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateEaseType");
                            anim.scale.easeType = easeType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 110;
                        if(anim.scale.easeType == UIAnimator.EaseType.Ease)
                        {
                            DG.Tweening.Ease ease = anim.scale.ease;
                            QUI.BeginChangeCheck();
                            ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(ease, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateEase");
                                anim.scale.ease = ease;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        else
                        {
                            AnimationCurve animationCurve = anim.scale.animationCurve;
                            QUI.BeginChangeCheck();
                            animationCurve = EditorGUILayout.CurveField(animationCurve, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateCurve");
                                anim.scale.animationCurve = animationCurve;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                    }
                    QUI.EndHorizontal();

                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        public static void DrawAnimFade(Anim anim, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Purple), width, 18 + 42 * anim.fadeIsExpanded.faded);
            QUI.Space(-(18 + 42 * anim.fadeIsExpanded.faded));
            if(DUIUtils.FadeSlicedBar("Fade", anim.fadeIsExpanded.target ? QColors.Color.Purple : QColors.Color.Gray, anim.fadeIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleFade");
                anim.fade.enabled = !anim.fade.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(anim.fadeIsExpanded.target, width);

            if(QUI.BeginFadeGroup(anim.fadeIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.Purple.Color : QColors.PurpleLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = anim.fade.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            anim.fade.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = anim.fade.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            anim.fade.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = anim.move.animationType != Anim.AnimationType.State ? "alpha (transparency)" : "alpha (transparency) to";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        float alpha = anim.fade.alpha;
                        QUI.BeginChangeCheck();
                        alpha = EditorGUILayout.DelayedFloatField(alpha, GUILayout.Width(tempWidth - 28));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateAlpha");
                            anim.fade.alpha = alpha;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        UIAnimator.EaseType easeType = anim.fade.easeType;
                        QUI.BeginChangeCheck();
                        easeType = (UIAnimator.EaseType)EditorGUILayout.EnumPopup(easeType, GUILayout.Width(110));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateEaseType");
                            anim.fade.easeType = easeType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 110;
                        if(anim.fade.easeType == UIAnimator.EaseType.Ease)
                        {
                            DG.Tweening.Ease ease = anim.fade.ease;
                            QUI.BeginChangeCheck();
                            ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(ease, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateEase");
                                anim.fade.ease = ease;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        else
                        {
                            AnimationCurve animationCurve = anim.fade.animationCurve;
                            QUI.BeginChangeCheck();
                            animationCurve = EditorGUILayout.CurveField(animationCurve, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateCurve");
                                anim.fade.animationCurve = animationCurve;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                    }
                    QUI.EndHorizontal();

                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }

        public static void DrawLoop(Loop loop, Object targetObject, float width)
        {
            loop.UpdateAnimBools();
            QUI.Space(4);
            DrawLoopMove(loop, targetObject, width);
            QUI.Space(4);
            DrawLoopRotate(loop, targetObject, width);
            QUI.Space(4);
            DrawLoopScale(loop, targetObject, width);
            QUI.Space(4);
            DrawLoopFade(loop, targetObject, width);
            QUI.Space(4);
        }
        public static void DrawLoopMove(Loop loop, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Green), width, 18 + 62 * loop.moveIsExpanded.faded);
            QUI.Space(-(18 + 62 * loop.moveIsExpanded.faded));
            if(DUIUtils.MoveSlicedBar("Move Loop", loop.moveIsExpanded.target ? QColors.Color.Green : QColors.Color.Gray, loop.moveIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleMove");
                loop.move.enabled = !loop.move.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(loop.moveIsExpanded.target, width);

            if(QUI.BeginFadeGroup(loop.moveIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.Green.Color : QColors.GreenLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.move.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            loop.move.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.move.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            loop.move.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "loops";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempInt = loop.move.loops;
                        QUI.BeginChangeCheck();
                        tempInt = EditorGUILayout.DelayedIntField(tempInt, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateLoops");
                            loop.move.loops = tempInt;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "loop type";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        Loop.LoopType loopType = loop.move.loopType;
                        QUI.BeginChangeCheck();
                        loopType = (Loop.LoopType)EditorGUILayout.EnumPopup(loopType, GUILayout.Width(tempWidth - 36));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateLoopType");
                            loop.move.loopType = loopType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "movement";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempVector3 = loop.move.movement;
                        QUI.BeginChangeCheck();
                        tempVector3 = EditorGUILayout.Vector3Field("", tempVector3, GUILayout.Width(tempWidth - 14));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateMovement");
                            loop.move.movement = tempVector3;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        UIAnimator.EaseType easeType = loop.move.easeType;
                        QUI.BeginChangeCheck();
                        easeType = (UIAnimator.EaseType)EditorGUILayout.EnumPopup(easeType, GUILayout.Width(110));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateEaseType");
                            loop.move.easeType = easeType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 110;
                        if(loop.move.easeType == UIAnimator.EaseType.Ease)
                        {
                            DG.Tweening.Ease ease = loop.move.ease;
                            QUI.BeginChangeCheck();
                            ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(ease, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateEase");
                                loop.move.ease = ease;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        else
                        {
                            AnimationCurve animationCurve = loop.move.animationCurve;
                            QUI.BeginChangeCheck();
                            animationCurve = EditorGUILayout.CurveField(animationCurve, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateCurve");
                                loop.move.animationCurve = animationCurve;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                    }
                    QUI.EndHorizontal();
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        public static void DrawLoopRotate(Loop loop, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Orange), width, 18 + 62 * loop.rotateIsExpanded.faded);
            QUI.Space(-(18 + 62 * loop.rotateIsExpanded.faded));
            if(DUIUtils.RotateSlicedBar("Rotate Loop", loop.rotateIsExpanded.target ? QColors.Color.Orange : QColors.Color.Gray, loop.rotateIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleRotate");
                loop.rotate.enabled = !loop.rotate.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(loop.rotateIsExpanded.target, width);

            if(QUI.BeginFadeGroup(loop.rotateIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.Orange.Color : QColors.OrangeLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.rotate.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(32));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            loop.rotate.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 32;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.rotate.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(32));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            loop.rotate.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 32;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "loops";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempInt = loop.rotate.loops;
                        QUI.BeginChangeCheck();
                        tempInt = EditorGUILayout.DelayedIntField(tempInt, GUILayout.Width(32));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateLoops");
                            loop.rotate.loops = tempInt;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 32;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "loop type";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        Loop.LoopType loopType = loop.rotate.loopType;
                        QUI.BeginChangeCheck();
                        loopType = (Loop.LoopType)EditorGUILayout.EnumPopup(loopType, GUILayout.Width(tempWidth - 36));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateLoopType");
                            loop.rotate.loopType = loopType;

                            switch(loopType)
                            {
                                case Loop.LoopType.Restart: loop.rotate.rotateMode = Loop.RotateMode.FastBeyond360; break;
                                case Loop.LoopType.Yoyo: loop.rotate.rotateMode = Loop.RotateMode.Fast; break;
                                case Loop.LoopType.Incremental: loop.rotate.rotateMode = Loop.RotateMode.FastBeyond360; break;
                            }

                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "rotation";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempWidth -= 154;
                        tempVector3 = loop.rotate.rotation;
                        QUI.BeginChangeCheck();
                        tempVector3 = EditorGUILayout.Vector3Field("", tempVector3, GUILayout.Width(154));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateRotation");
                            loop.rotate.rotation = tempVector3;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "rotation mode";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        Loop.RotateMode rotateMode = loop.rotate.rotateMode;
                        QUI.BeginChangeCheck();
                        rotateMode = (Loop.RotateMode)EditorGUILayout.EnumPopup(rotateMode, GUILayout.Width(tempWidth - 20));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateRotateMode");
                            loop.rotate.rotateMode = rotateMode;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        UIAnimator.EaseType easeType = loop.rotate.easeType;
                        QUI.BeginChangeCheck();
                        easeType = (UIAnimator.EaseType)EditorGUILayout.EnumPopup(easeType, GUILayout.Width(110));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateEaseType");
                            loop.rotate.easeType = easeType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 110;
                        if(loop.rotate.easeType == UIAnimator.EaseType.Ease)
                        {
                            DG.Tweening.Ease ease = loop.rotate.ease;
                            QUI.BeginChangeCheck();
                            ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(ease, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateEase");
                                loop.rotate.ease = ease;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        else
                        {
                            AnimationCurve animationCurve = loop.rotate.animationCurve;
                            QUI.BeginChangeCheck();
                            animationCurve = EditorGUILayout.CurveField(animationCurve, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateCurve");
                                loop.rotate.animationCurve = animationCurve;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                    }
                    QUI.EndHorizontal();

                    QUI.ResetColors();

                    switch(loop.rotate.rotateMode)
                    {
                        case Loop.RotateMode.Fast:
                            QUI.DrawInfoMessage(new InfoMessage
                            {
                                title = "Rotate Mode: " + loop.rotate.rotateMode,
                                message = "Fastest way that never rotates beyond 360°",
                                show = new AnimBool(true),
                                type = InfoMessageType.Help
                            },
                                                width);
                            break;
                        case Loop.RotateMode.FastBeyond360:
                            QUI.DrawInfoMessage(new InfoMessage
                            {
                                title = "Rotate Mode: " + loop.rotate.rotateMode,
                                message = "Fastest way that rotates beyond 360°",
                                show = new AnimBool(true),
                                type = InfoMessageType.Help
                            },
                                               width);
                            break;
                        case Loop.RotateMode.WorldAxisAdd:
                            QUI.DrawInfoMessage(new InfoMessage
                            {
                                title = "Rotate Mode: " + loop.rotate.rotateMode,
                                message = "Adds the given rotation to the transform using world axis and an advanced precision mode (like when using transform.Rotate(Space.World)). In this mode the end value is is always considered relative.",
                                show = new AnimBool(true),
                                type = InfoMessageType.Help
                            },
                                             width);
                            break;
                        case Loop.RotateMode.LocalAxisAdd:
                            QUI.DrawInfoMessage(new InfoMessage
                            {
                                title = "Rotate Mode: " + loop.rotate.rotateMode,
                                message = "Adds the given rotation to the transform's local axis (like when rotating an object with the 'local' switch enabled in Unity's editor or using transform.Rotate(Space.Self)). In this mode the end value is is always considered relative.",
                                show = new AnimBool(true),
                                type = InfoMessageType.Help
                            },
                                          width);
                            break;
                    }
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }
        public static void DrawLoopScale(Loop loop, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Red), width, 18 + 62 * loop.scaleIsExpanded.faded);
            QUI.Space(-(18 + 62 * loop.scaleIsExpanded.faded));
            if(DUIUtils.ScaleSlicedBar("Scale Loop", loop.scaleIsExpanded.target ? QColors.Color.Red : QColors.Color.Gray, loop.scaleIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleScale");
                loop.scale.enabled = !loop.scale.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(loop.scaleIsExpanded.target, width);

            if(QUI.BeginFadeGroup(loop.scaleIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.RedLight.Color : QColors.RedLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.scale.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            loop.scale.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.scale.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            loop.scale.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "loops";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempInt = loop.scale.loops;
                        QUI.BeginChangeCheck();
                        tempInt = EditorGUILayout.DelayedIntField(tempInt, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateLoops");
                            loop.scale.loops = tempInt;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "loop type";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        Loop.LoopType loopType = loop.scale.loopType;
                        QUI.BeginChangeCheck();
                        loopType = (Loop.LoopType)EditorGUILayout.EnumPopup(loopType, GUILayout.Width(tempWidth - 36));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateLoopType");
                            loop.scale.loopType = loopType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "min";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempVector3 = loop.scale.min;
                        QUI.BeginChangeCheck();
                        tempVector3 = EditorGUILayout.Vector3Field("", tempVector3, GUILayout.Width((width - 68) / 2));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateMin");
                            loop.scale.min = tempVector3;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "max";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempVector3 = loop.scale.max;
                        QUI.BeginChangeCheck();
                        tempVector3 = EditorGUILayout.Vector3Field("", tempVector3, GUILayout.Width((width - 68) / 2));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateMax");
                            loop.scale.max = tempVector3;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        UIAnimator.EaseType easeType = loop.scale.easeType;
                        QUI.BeginChangeCheck();
                        easeType = (UIAnimator.EaseType)EditorGUILayout.EnumPopup(easeType, GUILayout.Width(110));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateEaseType");
                            loop.scale.easeType = easeType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 110;
                        if(loop.scale.easeType == UIAnimator.EaseType.Ease)
                        {
                            DG.Tweening.Ease ease = loop.scale.ease;
                            QUI.BeginChangeCheck();
                            ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(ease, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateEase");
                                loop.scale.ease = ease;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        else
                        {
                            AnimationCurve animationCurve = loop.scale.animationCurve;
                            QUI.BeginChangeCheck();
                            animationCurve = EditorGUILayout.CurveField(animationCurve, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateCurve");
                                loop.scale.animationCurve = animationCurve;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                    }
                    QUI.EndHorizontal();
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        public static void DrawLoopFade(Loop loop, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Purple), width, 18 + 62 * loop.fadeIsExpanded.faded);
            QUI.Space(-(18 + 62 * loop.fadeIsExpanded.faded));
            if(DUIUtils.FadeSlicedBar("Fade Loop", loop.fadeIsExpanded.target ? QColors.Color.Purple : QColors.Color.Gray, loop.fadeIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleFade");
                loop.fade.enabled = !loop.fade.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(loop.fadeIsExpanded.target, width);

            if(QUI.BeginFadeGroup(loop.fadeIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.Purple.Color : QColors.PurpleLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.fade.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            loop.fade.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.fade.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            loop.fade.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "loops";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempInt = loop.fade.loops;
                        QUI.BeginChangeCheck();
                        tempInt = EditorGUILayout.DelayedIntField(tempInt, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateLoops");
                            loop.fade.loops = tempInt;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 40;
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "loop type";
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        Loop.LoopType loopType = loop.fade.loopType;
                        QUI.BeginChangeCheck();
                        loopType = (Loop.LoopType)EditorGUILayout.EnumPopup(loopType, GUILayout.Width(tempWidth - 36));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateLoopType");
                            loop.fade.loopType = loopType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        QLabel.text = "min";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.fade.min;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width((width - 68) / 2));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateMin");
                            loop.fade.min = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.Space(4);
                        tempWidth -= 4;
                        QLabel.text = "max";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempWidth -= QLabel.x;
                        tempFloat = loop.fade.max;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width((width - 68) / 2));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateMax");
                            loop.fade.max = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        tempWidth = width;
                        UIAnimator.EaseType easeType = loop.fade.easeType;
                        QUI.BeginChangeCheck();
                        easeType = (UIAnimator.EaseType)EditorGUILayout.EnumPopup(easeType, GUILayout.Width(110));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateEaseType");
                            loop.fade.easeType = easeType;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        tempWidth -= 110;
                        if(loop.fade.easeType == UIAnimator.EaseType.Ease)
                        {
                            DG.Tweening.Ease ease = loop.fade.ease;
                            QUI.BeginChangeCheck();
                            ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup(ease, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateEase");
                                loop.fade.ease = ease;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        else
                        {
                            AnimationCurve animationCurve = loop.fade.animationCurve;
                            QUI.BeginChangeCheck();
                            animationCurve = EditorGUILayout.CurveField(animationCurve, GUILayout.Width(tempWidth - 12));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(targetObject, "UpdateCurve");
                                loop.fade.animationCurve = animationCurve;
                                QUI.SetDirty(targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }
                    }
                    QUI.EndHorizontal();
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }

        public static void DrawPunch(Punch punch, Object targetObject, float width)
        {
            punch.UpdateAnimBools();
            QUI.Space(4);
            DrawPunchMove(punch, targetObject, width);
            QUI.Space(4);
            DrawPunchRotate(punch, targetObject, width);
            QUI.Space(4);
            DrawPunchScale(punch, targetObject, width);
            QUI.Space(4);
        }
        public static void DrawPunchMove(Punch punch, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Green), width, 18 + 42 * punch.moveIsExpanded.faded);
            QUI.Space(-(18 + 42 * punch.moveIsExpanded.faded));
            if(DUIUtils.MoveSlicedBar("Punch Move", punch.moveIsExpanded.target ? QColors.Color.Green : QColors.Color.Gray, punch.moveIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleMove");
                punch.move.enabled = !punch.move.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(punch.moveIsExpanded.target, width);

            if(QUI.BeginFadeGroup(punch.moveIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.Green.Color : QColors.GreenLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempFloat = punch.move.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            punch.move.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempFloat = punch.move.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            punch.move.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                        QLabel.text = "vibrato";
                        QUI.Label(QLabel);
                        tempInt = punch.move.vibrato;
                        QUI.BeginChangeCheck();
                        tempInt = EditorGUILayout.DelayedIntField(tempInt, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateVibrato");
                            punch.move.vibrato = tempInt;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                        QLabel.text = "elasticity";
                        QUI.Label(QLabel);
                        tempFloat = punch.move.elasticity;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateElasticity");
                            punch.move.elasticity = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.Space(8);
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        QLabel.text = "punch";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempVector3 = punch.move.punch;
                        QUI.BeginChangeCheck();
                        tempVector3 = EditorGUILayout.Vector3Field("", tempVector3, GUILayout.Width(width - QLabel.x - 12));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdatePunch");
                            punch.move.punch = tempVector3;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        public static void DrawPunchRotate(Punch punch, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Orange), width, 18 + 42 * punch.rotateIsExpanded.faded);
            QUI.Space(-(18 + 42 * punch.rotateIsExpanded.faded));
            if(DUIUtils.RotateSlicedBar("Punch Rotate", punch.rotateIsExpanded.target ? QColors.Color.Orange : QColors.Color.Gray, punch.rotateIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleRotate");
                punch.rotate.enabled = !punch.rotate.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(punch.rotateIsExpanded.target, width);

            if(QUI.BeginFadeGroup(punch.rotateIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.Orange.Color : QColors.OrangeLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempFloat = punch.rotate.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            punch.rotate.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempFloat = punch.rotate.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            punch.rotate.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                        QLabel.text = "vibrato";
                        QUI.Label(QLabel);
                        tempInt = punch.rotate.vibrato;
                        QUI.BeginChangeCheck();
                        tempInt = EditorGUILayout.DelayedIntField(tempInt, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateVibrato");
                            punch.rotate.vibrato = tempInt;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                        QLabel.text = "elasticity";
                        QUI.Label(QLabel);
                        tempFloat = punch.rotate.elasticity;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateElasticity");
                            punch.rotate.elasticity = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.Space(8);
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        QLabel.text = "punch";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempVector3 = punch.rotate.punch;
                        QUI.BeginChangeCheck();
                        tempVector3 = EditorGUILayout.Vector3Field("", tempVector3, GUILayout.Width(width - QLabel.x - 12));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdatePunch");
                            punch.rotate.punch = tempVector3;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }
        public static void DrawPunchScale(Punch punch, Object targetObject, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Red), width, 18 + 42 * punch.scaleIsExpanded.faded);
            QUI.Space(-(18 + 42 * punch.scaleIsExpanded.faded));
            if(DUIUtils.ScaleSlicedBar("Punch Scale", punch.scaleIsExpanded.target ? QColors.Color.Red : QColors.Color.Gray, punch.scaleIsExpanded, width, 18))
            {
                Undo.RecordObject(targetObject, "ToggleScale");
                punch.scale.enabled = !punch.scale.enabled;
                QUI.SetDirty(targetObject);
                AssetDatabase.SaveAssets();
            }

            DrawEnabledDisabledBarText(punch.scaleIsExpanded.target, width);

            if(QUI.BeginFadeGroup(punch.scaleIsExpanded.faded))
            {
                QUI.SetGUIBackgroundColor(QUI.IsProSkin ? QColors.RedLight.Color : QColors.RedLight.Color);
                QUI.BeginVertical(width);
                {
                    QUI.BeginHorizontal(width);
                    {
                        QLabel.text = "start delay";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempFloat = punch.scale.startDelay;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateStartDelay");
                            punch.scale.startDelay = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                        QLabel.text = "duration";
                        QUI.Label(QLabel);
                        tempFloat = punch.scale.duration;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateDuration");
                            punch.scale.duration = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                        QLabel.text = "vibrato";
                        QUI.Label(QLabel);
                        tempInt = punch.scale.vibrato;
                        QUI.BeginChangeCheck();
                        tempInt = EditorGUILayout.DelayedIntField(tempInt, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateVibrato");
                            punch.scale.vibrato = tempInt;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.FlexibleSpace();
                        QLabel.text = "elasticity";
                        QUI.Label(QLabel);
                        tempFloat = punch.scale.elasticity;
                        QUI.BeginChangeCheck();
                        tempFloat = EditorGUILayout.DelayedFloatField(tempFloat, GUILayout.Width(40));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdateElasticity");
                            punch.scale.elasticity = tempFloat;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                        QUI.Space(8);
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        QLabel.text = "punch";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        tempVector3 = punch.scale.punch;
                        QUI.BeginChangeCheck();
                        tempVector3 = EditorGUILayout.Vector3Field("", tempVector3, GUILayout.Width(width - QLabel.x - 12));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(targetObject, "UpdatePunch");
                            punch.scale.punch = tempVector3;
                            QUI.SetDirty(targetObject);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                }
                QUI.EndVertical();
                QUI.ResetColors();
            }
            QUI.EndFadeGroup();
        }

        static void DrawEnabledDisabledBarText(bool isEnabled, float width)
        {
            QUI.Space(-19);
            QUI.BeginHorizontal(width);
            {
                QUI.FlexibleSpace();
                QLabel.text = isEnabled ? "enabled" : "disabled";
                QLabel.style = Style.Text.Small;
                QUI.Label(QLabel);
                QUI.Space(8);
            }
            QUI.EndHorizontal();
            QUI.Space(-1);
        }

        public static void DrawLoadPresetAtRuntime(SerializedProperty loadPresetAtRuntime, float width, string typeOfPreset = "")
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, loadPresetAtRuntime.boolValue ? QColors.Color.Blue : QColors.Color.Gray), width, 18); //if a preset is set to load at runtime -> set a background color (visual aid for the dev)
            QUI.Space(-18);

            if(loadPresetAtRuntime.boolValue) //if a preset is set to load at runtime -> set a background color (visual aid for the dev)
            {
                QUI.SetGUIBackgroundColor(AccentColorBlue);
            }
            QUI.BeginHorizontal(width);
            {
                QUI.Space(4);
                QUI.Toggle(loadPresetAtRuntime);
                QLabel.text = "Load selected " + typeOfPreset + " preset at runtime";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(2);
                }
                QUI.EndVertical();
                QUI.Space(-6);
                QLabel.text = "(overrides current settings)";
                QLabel.style = Style.Text.Help;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(1);
                }
                QUI.EndVertical();
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.ResetColors();
        }

        public static void DrawPresetBackground(AnimBool newPreset, AnimBool createNewCategoryName, float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, newPreset.target && createNewCategoryName.target ? QColors.Color.Blue : QColors.Color.Gray), (width - 2) / 2, 34); //if creating a new category -> color the background
                QUI.Space(2);
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, newPreset.target ? QColors.Color.Blue : QColors.Color.Gray), (width - 2) / 2, 34); //if creating a new preset -> color the background
            }
            QUI.EndHorizontal();

            QUI.Space(-36);

            tempFloat = (width - 6) / 2; //column width

            QUI.BeginHorizontal(width);
            {
                QUI.Space(6);

                QLabel.text = newPreset.target
                              ? createNewCategoryName.target
                                ? "Enter a new category name"
                                : "Select Category"
                              : "Preset Category";
                QLabel.style = Style.Text.Small;
                QUI.BeginHorizontal(tempFloat);
                {
                    QUI.Label(QLabel);
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();

                QLabel.text = newPreset.target
                              ? "Enter a new preset name"
                              : "Preset Name";
                QLabel.style = Style.Text.Small;
                QUI.BeginHorizontal(tempFloat);
                {
                    QUI.Label(QLabel);
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
            }
            QUI.EndHorizontal();

            QUI.Space(-4);
        }

        public static void DrawInfoMessage(Dictionary<string, InfoMessage> infoMessage, string key, float width)
        {
            if(infoMessage == null) { Debug.Log("The infoMessage database is null."); return; }
            if(infoMessage.Count == 0) { Debug.Log("The infoMessage database is empty. Add the key to the database before you try to darw its message."); return; }
            if(!infoMessage.ContainsKey(key)) { Debug.Log("The infoMessage database does not contain any key named '" + key + "'. Check if it was added to the database or if you spelled the key wrong."); return; }
            QUI.DrawInfoMessage(infoMessage[key], width);
        }

        #endregion

        public static void DrawBarWithEnableDisableButton(string title, SerializedProperty use, AnimBool show, float width, float barHeight)
        {
            QUI.BeginHorizontal(width);
            {
                if (!use.boolValue && show.target) //if disabled -> close the container
                {
                    show.target = false;
                }

                if (QUI.GhostBar(title, use.boolValue ? QColors.Color.Blue : QColors.Color.Gray, show, width - 80, barHeight))
                {
                    show.target = !show.target;
                }
                if (QUI.GhostButton(use.boolValue ? "ENABLED" : "DISABLED", use.boolValue ? QColors.Color.Green : QColors.Color.Gray, 80, barHeight, show.target))
                {
                    use.boolValue = !use.boolValue;
                    if (use.boolValue)
                    {
                        show.target = true;
                    }
                }
            }
            QUI.EndHorizontal();
        }

        public static void DrawLoadPresetInfoMessage(string loadPresetInfoMessageTag, Dictionary<string, InfoMessage> infoMessage, bool loadPresetAtRuntime, string categoryName, string presetName, AnimBool show, string presetType, float width)
        {
            infoMessage[loadPresetInfoMessageTag].show.target = loadPresetAtRuntime;  //check if a preset is set to load at runtime
            infoMessage[loadPresetInfoMessageTag].title = "Runtime " + presetType + " Preset: " + categoryName + " / " + presetName; //set the preset category and name that are set to load at runtime
            DrawInfoMessage(infoMessage, loadPresetInfoMessageTag, width); //draw info if a preset is set to load at runtime
            QUI.Space(4 * (1 - show.faded) * infoMessage[loadPresetInfoMessageTag].show.faded); //space added if a preset is set to load at runtime
        }

        public static void DrawUnityEvents(bool enabled, AnimBool showEvents, SerializedProperty unityEvent, string unityEventTitle, float width, float miniBarHeight)
        {
            if (QUI.GhostBar("Unity Events", enabled ? QColors.Color.Blue : QColors.Color.Gray, showEvents, width, miniBarHeight))
            {
                showEvents.target = !showEvents.target;
            }
            QUI.BeginHorizontal(width);
            {
                QUI.Space(8 * showEvents.faded);
                if (QUI.BeginFadeGroup(showEvents.faded))
                {
                    QUI.SetGUIBackgroundColor(enabled ? AccentColorBlue : AccentColorGray);
                    QUI.BeginVertical(width - 16);
                    {
                        QUI.Space(2 * showEvents.faded);
                        QUI.PropertyField(unityEvent, new GUIContent() { text = unityEventTitle }, width - 8);
                        QUI.Space(2 * showEvents.faded);
                    }
                    QUI.EndVertical();
                    QUI.ResetColors();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }
    }
}
