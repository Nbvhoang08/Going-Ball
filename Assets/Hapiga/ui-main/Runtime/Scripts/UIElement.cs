using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hapiga.UI
{
    [AddComponentMenu(DUI.COMPONENT_MENU_UIELEMENT, DUI.MENU_PRIORITY_UIELEMENT)]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [DisallowMultipleComponent]
    public class UIElement : MonoBehaviour
    {
        // #region Context Menu
// #if UNITY_EDITOR
//         [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UIELEMENT, false, DUI.MENU_PRIORITY_UIELEMENT)]
//         static void CreateElement(UnityEditor.MenuCommand menuCommand)
//         {
//             GameObject selectedGO = menuCommand.context as GameObject;
//             if (selectedGO == null || selectedGO.GetComponent<Canvas>() == null)
//             {
//                 EditorUtility.DisplayDialog("Error", "Select Canvas to create UI Element", "OK");
//                 return;
//             }
//             
//             GameObject go = new GameObject("UIElement", typeof(RectTransform), typeof(UIElement));
//             UnityEditor.GameObjectUtility.SetParentAndAlign(go, selectedGO);
//             UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
//             go.GetComponent<UIElement>().Reset();
//             go.GetComponent<RectTransform>().localScale = Vector3.one;
//             go.GetComponent<RectTransform>().anchorMin = Vector2.zero;
//             go.GetComponent<RectTransform>().anchorMax = Vector2.one;
//             go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
//             go.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
//
//             UnityEditor.Selection.activeObject = go;
//         }
// #endif
        // #endregion

        public IUiManager UIManager;

        /// <summary>
        /// This is an extra id tag given to the tweener in order to locate the proper tween that manages the loop animations.
        /// </summary>
        /// 
        public const string LOOP_ANIMATIONS_ID = "UIElementLoopAnimations";

        /// <summary>
        /// Disables this UIElement when it is not visible (it is hidden) by setting it's active state to false.
        /// <para>Use this only if you have scripts that you need to disable. Otherwise you don't need it as the system handles the drawcalls in an effecient manner.</para>
        /// </summary>
        public bool disableWhenHidden = false;
        /// <summary>
        /// This will disable the optimization that disables the Canvas and GraphicRaycaster when the UIElement is hidden. 
        /// Do not enable this unless you know what you are doing as, when set to TRUE, this will increase your draw calls.
        /// </summary>
        public bool dontDisableCanvasWhenHidden = false;
        /// <summary>
        /// This will disable the GraphicRaycaster at Awake and will never toggle it.
        /// This option can be used if there are not interactable objects as children of this UIElement.
        /// It's just a small optimisation.
        /// </summary>
        public bool disableGraphicRaycaster = false;

        /// <summary>
        /// Should this UIElement come from or go to a set custom position every time an In or Out animation is played? Default is set to false.
        /// </summary>
        public bool useCustomStartAnchoredPosition = false;
        /// <summary>
        /// The custom anchored position that this UIElement comes from or goes to when an In or Out animation is played. You can use this in code to cusomize on the fly this positon.
        /// </summary>
        public Vector3 customStartAnchoredPosition = Vector3.zero;

        /// <summary>
        /// This fixes a very strange issue inside Unity. When setting a VerticalLayoutGroup or a HorizontalLayoutGroup, the Image bounds get moved (the image appeares in a different place).
        /// <para>If you have this issue, just set this to true. Default is set to false.</para>
        /// <para>If you are curious about what this does, look at the ExecuteLayoutFix method.</para>
        /// </summary>
        public bool executeLayoutFix = false;

        /// <summary>
        /// Used by the UINotification. If this element is linked to a notification, then the notification should handle it's registration process in order to use an auto generated name. Do not change this value yourself.
        /// </summary>
        public bool autoRegister = true;
        /// <summary>
        /// Keeps track if this UIElement is visible or not. Do not change this value yourself.
        /// </summary>
        public bool isVisible = true;
        /// <summary>
        /// Internal variable that is set to true if this UIElement has other child UIElements. This is uesd by the system in order to handle special Show and Hide use case scenarios.
        /// </summary>
        private bool containsChildUIElements = false;

        #region IN ANIMATIONS
        /// <summary>
        /// In Animation Settings
        /// </summary>
        public Anim inAnimations = new Anim(Anim.AnimationType.In);

        /// <summary>
        /// UnityEvent invoked when In animations start.
        /// </summary>
        public UnityEvent OnInAnimationsStart = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when In animations finished.
        /// </summary>
        public UnityEvent OnInAnimationsFinish = new UnityEvent();

        /// <summary>
        /// Out Animations Preset Category Name
        /// </summary>
        public string inAnimationsPresetCategoryName = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Out Animations Preset Name
        /// </summary>
        public string inAnimationsPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Animation Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadInAnimationsPresetAtRuntime = false;

        #endregion
        #region OUT ANIMATIONS
        /// <summary>
        /// Out Animation Settings
        /// </summary>
        public Anim outAnimations = new Anim(Anim.AnimationType.Out);

        /// <summary>
        /// UnityEvent invoked when Out animations start.
        /// </summary>
        public UnityEvent OnOutAnimationsStart = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when Out animations finished.
        /// </summary>
        public UnityEvent OnOutAnimationsFinish = new UnityEvent();

        /// <summary>
        /// Out Animations Preset Category Name
        /// </summary>
        public string outAnimationsPresetCategoryName = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Out Animations Preset Name
        /// </summary>
        public string outAnimationsPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Animation Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOutAnimationsPresetAtRuntime = false;

        #endregion


        /// <summary>
        /// Internal variable that holds a reference to the RectTransform component.
        /// </summary>
        private RectTransform m_rectTransform;
        /// <summary>
        /// Returns the RectTransform component.
        /// </summary>
        public RectTransform RectTransform { get { if(m_rectTransform == null) { m_rectTransform = GetComponent<RectTransform>() == null ? gameObject.AddComponent<RectTransform>() : GetComponent<RectTransform>(); } return m_rectTransform; } }
        /// <summary>
        /// Internal variable that holds a reference to the Canvas component.
        /// </summary>
        ///  private Canvas m_canvas;
        /// <summary>
        /// Returns the Canvas component.
        /// </summary>
        ///         private Canvas m_canvas;
        private Canvas m_canvas;
        /// <summary>
        /// Returns the Canvas component.
        /// </summary>
        public Canvas Canvas { get { if(m_canvas == null) { m_canvas = GetComponent<Canvas>() == null ? gameObject.AddComponent<Canvas>() : GetComponent<Canvas>(); } return m_canvas; } }
        /// <summary>
        /// Internal variable that holds a reference to the GraphicRaycaster component.
        /// </summary>
        private GraphicRaycaster m_graphicRaycaster;
        /// <summary>
        /// Returns the GraphicRaycaster component.
        /// </summary>
        public GraphicRaycaster GraphicRaycaster { get { if(m_graphicRaycaster == null) { m_graphicRaycaster = GetComponent<GraphicRaycaster>() == null ? gameObject.AddComponent<GraphicRaycaster>() : GetComponent<GraphicRaycaster>(); } return m_graphicRaycaster; } }
        /// <summary>
        /// Internal variable that holds a reference to the CanvasGroup component.
        /// </summary>
        private CanvasGroup m_canvasGroup;
        /// <summary>
        /// Returns the CanvasGroup component.
        /// </summary>
        public CanvasGroup CanvasGroup { get { if(m_canvasGroup == null) { m_canvasGroup = GetComponent<CanvasGroup>() == null ? gameObject.AddComponent<CanvasGroup>() : GetComponent<CanvasGroup>(); } return m_canvasGroup; } }


        /// <summary>
        /// Internal variable that holds the start RectTransform.anchoredPosition3D.
        /// </summary>
        private Vector3 startPosition;
        /// <summary>
        /// Internal variable that holds the start RectTransform.localEulerAngles
        /// </summary>
        private Vector3 startRotation;
        /// <summary>
        /// Internal variable that holds the start RectTransform.localScale
        /// </summary>
        private Vector3 startScale;
        /// <summary>
        /// Internal variable that holds the start alpha. It does that by checking if a CanvasGroup component is attached (holding the alpha value) or it just rememebers 1 (as in 100% visibility)
        /// </summary>
        private float startAlpha;

        /// <summary>
        /// Internal variable used when disableWhenHidden is set to true. After the element has been hidden (the out animations finished), the system waits for an additional disableTimeBuffer before it sets this gameObject's active state to false. This is a failsafe mesure and fixes a small bug on iOS.
        /// </summary>
        private float disableTimeBuffer = 0.05f;

        /// <summary>
        /// Internal variable that holds a reference to the coroutine that shows the element.
        /// </summary>
        private Coroutine cShow;
        /// <summary>
        /// Internal variable that holds a reference to the coroutine that hides the element.
        /// </summary>
        private Coroutine cHide;
        /// <summary>
        /// Internal variable that holds a reference to the coroutine that automatically hides the element after being shown and after the autoHideDelay duration has passed.
        /// </summary>
        private Coroutine cAutoHide;
        /// <summary>
        /// Internal variable that holds a reference to the coroutine that disables button clicks when in transit (an In or Out animation is running).
        /// </summary>
        private Coroutine cDisableButtonClicks;

        /// <summary>
        /// Internal array that holds the references to all the child Canvases.
        /// </summary>
        private Canvas[] childCanvas;

        private LayoutGroup[] layoutGroups;

        /// <summary>
        /// Returns true if at least one In animation is enabled. This means that if either move or rotate or scale or fade are enabled it will return true and false otherwise.
        /// </summary>
        public bool InAnimationsEnabled { get { return inAnimations.Enabled; } }

        /// <summary>
        /// Returns true if at least one Out animation is enabled. This means that if either move or rotate or scale or fade are enabled it will return true and false otherwise.
        /// </summary>
        public bool OutAnimationsEnabled { get { return outAnimations.Enabled; } }

        protected WaitForSecondsRealtime waitDelay;
        protected WaitForSecondsRealtime waitDuration;


        public void Reset()
        {
            Canvas.overrideSorting = true;
        }
        
        void Awake()
        {
            m_rectTransform = RectTransform;
            m_canvas = Canvas;
            m_canvasGroup = CanvasGroup;
            m_graphicRaycaster = GraphicRaycaster;
            if(disableGraphicRaycaster)
            {
                GraphicRaycaster.enabled = false;
            }

            startPosition = useCustomStartAnchoredPosition ? customStartAnchoredPosition : RectTransform.anchoredPosition3D;
            startRotation = RectTransform.localEulerAngles;
            startScale = RectTransform.localScale;
            startAlpha = CanvasGroup == null ? 1 : CanvasGroup.alpha;

            childCanvas = GetComponentsInChildren<Canvas>();
            layoutGroups = GetComponentsInChildren<LayoutGroup>();
            UIManager = GetComponentInParent<IUiManager>();
            LoadRuntimeInAnimationsPreset();
            LoadRuntimeOutAnimationsPreset();

        }

        void OnEnable()
        {
            ExecuteLayoutFix();
            MoveToCustomStartPosition();
        }

        void Start()
        {
            if(autoRegister) { RegisterToUIManager(); }

            OnInAnimationsStart.AddListener(InAnimationsStart);
            OnInAnimationsFinish.AddListener(InAnimationsFinish);
            OnOutAnimationsStart.AddListener(OutAnimationsStart);
            OnOutAnimationsFinish.AddListener(OutAnimationsFinish);

            SetupElement();
        }

        void OnDisable()
        {
        }

        void OnDestroy()
        {
            UnregisterFromUIManager();
            OnInAnimationsStart.RemoveListener(InAnimationsStart);
            OnInAnimationsFinish.RemoveListener(InAnimationsFinish);
            OnOutAnimationsStart.RemoveListener(OutAnimationsStart);
            OnOutAnimationsFinish.RemoveListener(OutAnimationsFinish);
        }

        /// <summary>
        /// Moves the UIElement to the set custom start position.
        /// </summary>
        void MoveToCustomStartPosition()
        {
            if(useCustomStartAnchoredPosition) { RectTransform.anchoredPosition3D = customStartAnchoredPosition; }
        }

        #region RegisterToUIManager / UnregisterFromUIManager
        /// <summary>
        /// Registers this UIElement to the UIManager.
        /// </summary>
        public void RegisterToUIManager()
        {
            
        }
        /// <summary>
        /// Unregisters this UIElement from the UIManager.
        /// </summary>
        public void UnregisterFromUIManager()
        {

        }
        #endregion

        /// <summary>
        /// Executes the inital setup for this UIElement.
        /// </summary>
        protected virtual void SetupElement()
        {
            if (GetComponentsInChildren<UIElement>().Length > 1)
            {
                containsChildUIElements = true;
            }
            
            if (waitDelay == null)
            {
                waitDelay = new WaitForSecondsRealtime(inAnimations.StartDelay);
            }

            if (waitDuration == null)
            {
                waitDuration = new WaitForSecondsRealtime(inAnimations.TotalDuration);
            }
        }

        

        /// <summary>
        /// Shows all the UIElements that have the given name and category.
        /// </summary>
        public  void ShowUiElement()
        {
            ShowUiElement(false);
        }
        /// <summary>
        /// Shows all the UIElements that have the given name and the DEFAULT CATEGORY name.
        /// </summary>
        /// <param name="instantAction">Should the animation play instantly (in zero seconds)</param>
        public  void ShowUiElement(bool instantAction)
        {
            ExecuteShow(instantAction);
        }
       

        /// <summary>
        /// This executes the SHOW actions and forces an Instance if required.
        /// <para/> This is needed in order to call Show at Start
        /// </summary>
        private void ExecuteShow( bool instantAction)
        {
            if (this != null) //this null check has been added to fix the slim chance that we registered a UIElement to the registry and it has been destroyed/deleted (thus now it's null)
            {
                if (!this.gameObject.activeInHierarchy)
                {
                    this.gameObject.SetActive(true);
                }

                this.Show(instantAction);
            }
        }

        /// <summary>
        /// Hides all the UIElements that have the given name and category.
        /// </summary>
        /// <param name="instantAction">Should the animation play instantly (in zero seconds)</param>
        public void HideUiElement()
        {
            HideUiElement(false);
        }
      
        /// <summary>
        /// Hides all the UIElements that have the given name and category.
        /// </summary>
        /// <param name="instantAction">Should the animation play instantly (in zero seconds)</param>
        public void HideUiElement(bool instantAction)
        {
           ExecuteHide(instantAction);
        }

        /// <summary>
        /// This IEnumerator executes the Hide actions and forces an Instance if required.
        /// </summary>
        private void ExecuteHide(bool instantAction)
        {
            this.Hide(instantAction);
        }

        /// <summary>
        /// Resets the UIElement's RectTransfrom to the start values.
        /// </summary>
        void ResetRectTransform()
        {
            UIAnimator.ResetTarget(RectTransform, useCustomStartAnchoredPosition ? customStartAnchoredPosition : startPosition, startRotation, startScale, startAlpha);

            //this is a fix that forces a reset for any child LayoutGroup (this fixes the stacked buttons after show)
            if(layoutGroups != null && layoutGroups.Length > 0)
            {
                bool foundNullLayoutGroupReference = false; //remember if any of the references to LayoutGroups are not null

                for(int i = 0; i < layoutGroups.Length; i++)
                {
                    if(layoutGroups[i] == null) //check if this layoutGroup reference is null
                    {
                        foundNullLayoutGroupReference = true; //mark that a null reference was found and that it needs to be removed
                        continue; //skip this step
                    }

                    layoutGroups[i].enabled = !layoutGroups[i].enabled;
                    layoutGroups[i].enabled = !layoutGroups[i].enabled;
                }

                if(foundNullLayoutGroupReference) //a null reference to a LayoutGroup was found -> remove it from the array
                {
                    List<LayoutGroup> tempList = new List<LayoutGroup>(); //create a list and add non-null values
                    for(int i = 0; i < layoutGroups.Length; i++)
                    {
                        if(layoutGroups[i] == null) //is this a null reference?
                        {
                            continue; //skip this step
                        }
                        tempList.Add(layoutGroups[i]); //add reference to the list
                    }

                    layoutGroups = tempList.ToArray(); //update the layoutGroups array
                }
            }
        }

        #region Show Methods (IN Animations)
        /// <summary>
        /// Loads the In Animations Preset that is set to load at runtime.
        /// </summary>
        void LoadRuntimeInAnimationsPreset()
        {
            if(loadInAnimationsPresetAtRuntime)
            {
                Anim presetAnimation = UIAnimatorUtil.GetInAnim(inAnimationsPresetCategoryName, inAnimationsPresetName);
                if(presetAnimation != null) { inAnimations = presetAnimation.Copy(); }
            }
        }

        /// <summary>
        /// Special reset for the UIElement's RectTransfrom that is executed before every Show.
        /// </summary>
        void ResetBeforeShow(bool resetPosition, bool resetAlpha)
        {
            if(resetPosition)
            {
                RectTransform.anchoredPosition3D = useCustomStartAnchoredPosition ? customStartAnchoredPosition : startPosition;
            }
            RectTransform.eulerAngles = startRotation;
            RectTransform.localScale = startScale;
            if(resetAlpha)
            {
                if(CanvasGroup == null) { return; }
                CanvasGroup.alpha = 1f;
            }
        }

        /// <summary>
        /// Shows the element.
        /// </summary>
        /// <param name="instantAction">If set to <c>true</c> it will execute the animations in 0 seconds and with 0 delay</param>
        public virtual void Show(bool instantAction)
        {
            if(cHide != null)
            {
                isVisible = false;
                StopCoroutine(cHide);
                cHide = null;
            }

            if(!InAnimationsEnabled)
            {
                Debug.LogWarning("[UI] [" + name + "] You are trying to SHOW the  UIElement, but you didn't enable any IN animations. To fix this warning you should enable at least one IN animation.");
                return;
            }

            if(isVisible == false)
            {
                TriggerInAnimationsEvents(instantAction);
                UIAnimator.StopAnimations(RectTransform, Anim.AnimationType.Out);
                cShow = StartCoroutine(iShow(instantAction));
                isVisible = true;
                if(disableWhenHidden && containsChildUIElements) StartCoroutine(TriggerShowInTheNextFrame(instantAction));
                ExecuteLayoutFix();
            }
        }
            
        /// <summary>
        /// Executes the UIElement Show command in realtime.
        /// </summary>
        IEnumerator iShow(bool instantAction)
        {
            yield return null;
            ToggleCanvasAndGraphicRaycaster(true);
            ResetBeforeShow(!inAnimations.move.enabled, !inAnimations.fade.enabled);
            UIAnimator.Move(RectTransform, useCustomStartAnchoredPosition ? customStartAnchoredPosition : startPosition, inAnimations, StartMoveIn, FinishMoveIn, instantAction, false);
            UIAnimator.Rotate(RectTransform, startRotation, inAnimations, StartRotateIn, FinishRotateIn, instantAction, false);
            UIAnimator.Scale(RectTransform, startScale, inAnimations, StartScaleIn, FinishScaleIn, instantAction, false);
            UIAnimator.Fade(RectTransform, startAlpha, inAnimations, StartFadeIn, FinishFadeIn, instantAction, false);
            if(inAnimations.TotalDuration >= 0 && !instantAction)
            {
                yield return waitDuration;
            }
            ResetRectTransform();
            cShow = null;
        }

        void StartMoveIn() { }
        void FinishMoveIn() { }
        void StartRotateIn() { }
        void FinishRotateIn() { }
        void StartScaleIn() { }
        void FinishScaleIn() { }
        void StartFadeIn() { }
        void FinishFadeIn() { }

      
        #endregion

        #region Hide Methods (OUT Animations)
        /// <summary>
        /// Loads the Out Animations Preset that is set to load at runtime.
        /// </summary>
        void LoadRuntimeOutAnimationsPreset()
        {
            if(loadOutAnimationsPresetAtRuntime)
            {
                Anim presetAnimation = UIAnimatorUtil.GetOutAnim(outAnimationsPresetCategoryName, outAnimationsPresetName);
                if(presetAnimation != null) { outAnimations = presetAnimation.Copy(); }
            }
        }

        /// <summary>
        /// Hides the element.
        /// </summary>
        /// <param name="instantAction">If set to <c>true</c> it will execute the animations in 0 seconds and with 0 delay</param>
        public void Hide(bool instantAction)
        {
            Hide(instantAction, disableWhenHidden);
        }
        /// <summary>
        /// Hides the element.
        /// </summary>
        /// <param name="instantAction">If set to <c>true</c> it will execute the animations in 0 seconds and with 0 delay</param>
        public virtual void Hide(bool instantAction, bool shouldDisable)
        {
            if(cShow != null)
            {
                isVisible = true;
                StopCoroutine(cShow);
                cShow = null;
            }

            if(!OutAnimationsEnabled)
            {
                Debug.LogWarning("[UI] [" + name + "] You are trying to HIDE the UIElement, but you didn't enable any OUT animations. To fix this warning you should enable at least one OUT animation.");
                return;
            }

            if(isVisible)
            {
                if (!instantAction)
                {
                    TriggerOutAnimationsEvents();
                } //we do this check so that the events are not triggered onEnable when we have startHidden set as true
                else
                {
                    TriggerOutAnimationsEventsStart();
                }
                UIAnimator.StopAnimations(RectTransform, Anim.AnimationType.In);
                cHide = StartCoroutine(iHide(instantAction, shouldDisable));
                isVisible = false;
            }
        }
        /// <summary>
        /// Executes the UIElement Hide command in realtime.
        /// </summary>
        IEnumerator iHide(bool instantAction, bool shouldDisable = true)
        {
            float start = Time.realtimeSinceStartup;
            UIAnimator.StopLoops(RectTransform, LOOP_ANIMATIONS_ID);
            UIAnimator.Move(RectTransform, useCustomStartAnchoredPosition ? customStartAnchoredPosition : startPosition, outAnimations, StartMoveOut, FinishMoveOut, instantAction, false);
            UIAnimator.Rotate(RectTransform, startRotation, outAnimations, StartRotateOut, FinishRotateOut, instantAction, false);
            UIAnimator.Scale(RectTransform, startScale, outAnimations, StartScaleOut, FinishScaleOut, instantAction, false);
            UIAnimator.Fade(RectTransform, startAlpha, outAnimations, StartFadeOut, FinishFadeOut, instantAction, false);

            if(shouldDisable)
            {
                while(Time.realtimeSinceStartup < start + disableTimeBuffer)
                {
                    yield return null;
                }
                if(!instantAction)
                {
                    while(Time.realtimeSinceStartup < start + outAnimations.TotalDuration + disableTimeBuffer)
                    {
                        yield return null;
                    }
                }
                ToggleCanvasAndGraphicRaycaster(dontDisableCanvasWhenHidden ? true : false);
                gameObject.SetActive(false);
            }
            else
            {
                if(!instantAction)
                {
                    while(Time.realtimeSinceStartup < start + outAnimations.TotalDuration + disableTimeBuffer)
                    {
                        yield return null;
                    }
                }
                ToggleCanvasAndGraphicRaycaster(dontDisableCanvasWhenHidden ? true : false);
            }
            cHide = null;
        }

        void StartMoveOut() { }
        void FinishMoveOut() { }
        void StartRotateOut() { }
        void FinishRotateOut() { }
        void StartScaleOut() { }
        void FinishScaleOut() { }
        void StartFadeOut() { }
        void FinishFadeOut() { }
        #endregion

        #region Events
        /// <summary>
        /// Helper method that invokes an UnityEvent after a set delay. This happens in realtime.
        /// </summary>
        private Coroutine InvokeEvent(UnityEvent @event, WaitForSecondsRealtime waitDelay,bool isDelay = true)
        {
            return StartCoroutine(InvokeEventAfterDelay(@event, waitDelay, isDelay));
        }
        /// <summary>
        /// Executes invoke for the UnityEvent after the set delay. This happens in realtime.
        /// </summary>
        /// <param name="event"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator InvokeEventAfterDelay(UnityEvent @event, WaitForSecondsRealtime waitDelay, bool isDelay)
        {
            if (isDelay)
            {
                if (waitDelay != null)
                {
                    yield return waitDelay;
                    if (@event != null)
                    {
                        @event.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// Internal method that invokes the In Animations UnityEvents whenever an In Animations starts.
        /// </summary>
        private void TriggerInAnimationsEvents(bool instantAction)
        {
            InvokeEvent(OnInAnimationsStart, waitDelay);
            InvokeEvent(OnInAnimationsFinish, waitDuration);
        }
        /// <summary>
        /// Internal method that invokes the Out Animations UnityEvents whenever an Out Animations starts.
        /// </summary>
        private void TriggerOutAnimationsEvents()
        {
            InvokeEvent(OnOutAnimationsStart, waitDelay);
            InvokeEvent(OnOutAnimationsFinish, waitDuration);
        }

        private void TriggerOutAnimationsEventsStart()
        {
            InvokeEvent(OnOutAnimationsStart, waitDelay, false);
            InvokeEvent(OnOutAnimationsFinish, waitDuration, false);
        }

        #endregion

        void InAnimationsStart()
        {

        }
        void InAnimationsFinish() { }

        void OutAnimationsStart()
        {

        }
        void OutAnimationsFinish() { }

        /// <summary>
        /// Internal method that is used to disable the Canvas and the GraphicRaycaster when the UIElement is hidden and to enable them when the UIElement is visible. This manages the draw calls without setting the gameObject's active state to false.
        /// </summary>
        /// <param name="isEnabled"></param>
        void ToggleCanvasAndGraphicRaycaster(bool isEnabled)
        {
            Canvas.enabled = isEnabled;
            if(!disableGraphicRaycaster)
            {
                GraphicRaycaster.enabled = isEnabled;
            }
        }

        /// <summary>
        /// This fixes a very strange issue inside Unity. When setting a VerticalLayoutGroup or a HorizontalLayoutGroup, the Image bounds get moved (the image appeares in a different place).
        /// We could not find a better solution, but this should work for now.
        /// </summary>
        void ExecuteLayoutFix()
        {
            if(!executeLayoutFix) { return; }
            childCanvas = GetComponentsInChildren<Canvas>();
            if(childCanvas != null && childCanvas.Length > 0)
            {
                for(int i = 0; i < childCanvas.Length; i++)
                {
                    childCanvas[i].enabled = !childCanvas[i].enabled;
                    childCanvas[i].enabled = !childCanvas[i].enabled;
                }
            }
        }

        /// <summary>
        /// If this UIElement has other child UIElements and was disabled when it was hidden, this comes into play. It calls again, in the next frame, the Show command so that the child UIElements will show up (provided they have the same element category and name).
        /// </summary>
        IEnumerator TriggerShowInTheNextFrame(bool instantAction)
        {
            yield return null;
            //UIManager.ShowUiElement(this, instantAction);
            this.Show(instantAction);
        }
    }
}