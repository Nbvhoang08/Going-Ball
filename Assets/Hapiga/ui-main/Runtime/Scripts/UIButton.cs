using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Hapiga.UI
{
    [AddComponentMenu(DUI.COMPONENT_MENU_UIBUTTON, DUI.MENU_PRIORITY_UIBUTTON)]
    [RequireComponent(typeof(RectTransform), typeof(Button))]
    [DisallowMultipleComponent]
    public class UIButton : MonoBehaviour, IPointerClickHandler, ISelectHandler, IDeselectHandler
    {
        #region Context Menu
#if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UIBUTTON, false, DUI.MENU_PRIORITY_UIBUTTON)]
        static void CreateButton(UnityEditor.MenuCommand menuCommand)
        {
            GameObject selectedGO = menuCommand.context as GameObject;
            if (selectedGO == null || selectedGO.GetComponent<RectTransform>() == null)
            {
                EditorUtility.DisplayDialog("Error", "Select RectTransform to create UI Button", "OK");
                return;
            }
            GameObject targetParent = selectedGO;
            GameObject go = new GameObject("UIButton", typeof(RectTransform), typeof(Button), typeof(UIButton));
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, targetParent);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            go.GetComponent<UIButton>().Reset();
            go.GetComponent<RectTransform>().localScale = Vector3.one;
            go.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            go.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(160f, 30f);
            go.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

            GameObject background = new GameObject("Background", typeof(RectTransform), typeof(Image));
            UnityEditor.GameObjectUtility.SetParentAndAlign(background, go);
            background.GetComponent<RectTransform>().localScale = Vector3.one;
            background.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            background.GetComponent<RectTransform>().anchorMax = Vector2.one;
            background.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            background.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            background.GetComponent<Image>().sprite = DUI.UISprite;
            background.GetComponent<Image>().type = Image.Type.Sliced;
            background.GetComponent<Image>().fillCenter = true;
            background.GetComponent<Image>().color = new Color(31f / 255f, 136f / 255f, 201f / 255f);
            GameObject label = new GameObject("Label TMPro", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
            UnityEditor.GameObjectUtility.SetParentAndAlign(label, go);
            label.GetComponent<RectTransform>().localScale = Vector3.one;
            label.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            label.GetComponent<RectTransform>().anchorMax = Vector2.one;
            label.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            label.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            label.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(2f / 255f, 10f / 255f, 15f / 255f);
            label.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 14;
            label.GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Center;
            label.GetComponent<TMPro.TextMeshProUGUI>().text = "UIButton";
            go.GetComponent<Button>().targetGraphic = background.GetComponent<Image>();
            UnityEditor.Selection.activeObject = go;
        }
#endif
        #endregion

        /// <summary>
        /// All the animationa a button can perform when interacted with.
        /// </summary>
        public enum ButtonAnimationType { Punch, State }
        /// <summary>
        /// All the action types a button can perform.
        /// </summary>
        public enum ButtonActionType {OnClick}
        /// <summary>
        /// All the click types actions a button can perform.
        /// </summary>
        public enum ButtonClickType { OnClick }

        /// <summary>
        /// Default value used to disable button after each click. Used when allow multiple clicks is set to false.
        /// </summary>
        public const float BETWEEN_CLICKS_DISABLE_INTERVAL = 0.4f;
        /// <summary>
        /// Default time interval used to register a double click. This is the time interval calculated between two sequencial clicks to determine if either a double click or two separate clicks occured.
        /// </summary>
        public const float DOUBLE_CLICK_REGISTER_INTERVAL = 0.2f;
        /// <summary>
        /// Special time interval added when deselecting a button. It fixses some anomalies.
        /// </summary>
        public const float DESELECT_BUTTON_DELAY = 0.1f;

        /// <summary>
        /// This is an extra id tag given to the tweener in order to locate the proper tween that manages the normal loop animations.
        /// </summary>
        public const string NORMAL_LOOP_ID = "ButtonNormalLoop";

        /// <summary>
        /// Enables debug logs.
        /// </summary>
        public bool debug = false;

        /// <summary>
        /// The name of this button. This is the value the system looks at when this button issues an action.
        /// </summary>
        public string buttonName = DUI.DEFAULT_BUTTON_NAME;

        /// <summary>
        /// Should the button get disabled for a set interval (disableButtonInterval) between each click. By default we allow the user to press the button multiple times.
        /// </summary>
        public bool allowMultipleClicks = true;
        /// <summary>
        /// Should the button get deselected after each click. This is useful if you do not want this button to get selected after a click.
        /// </summary>
        public bool deselectButtonOnClick = true;
        /// <summary>
        /// If allowMultipleClicks is false, then this is the interval that this button will be disabled for between each click.
        /// </summary>
        public float disableButtonInterval = BETWEEN_CLICKS_DISABLE_INTERVAL;

        public bool useOnClickAnimations = true;
        /// <summary>
        /// If enabled, the button action and game events are sent after the set animation has finished playing. This is useful if you want to be sure the uses see the button animation.
        /// </summary>
        public bool waitForOnClickAnimation = true;
        /// <summary>
        /// Setting for the OnClick trigger that marks if it should be registered instantly without checking if it's a double click or not.
        /// </summary>
        public enum SingleClickMode
        {
            /// <summary>
            /// The click will get registered instantly without checking if it's a double click or not. 
            /// <para>This is the normal behavior of a single click in any OS.</para>
            /// <para>Use this if you want to make sure a single click will get executed before a double click (dual actions).</para>
            /// <para>(usage example: SingleClick - selects, DoubleClick - executes an action)</para>
            /// </summary>
            Instant,
            /// <summary>
            /// The click will get registered after checking if it's a double click or not.
            /// <para>If it's a double click, the single click will not get triggered.</para>
            /// <para>Use this if you want to make sure the user does not execute a single click before a double click.</para>
            /// <para>The downside is that there is a delay when executing the single click (the delay is the double click register interval), so make sure you take that into account</para>
            /// </summary>
            Delayed
        }
        /// <summary>
        /// Determines if on click is triggered instantly or after it checks if it's a double click or not. Depending on your use case, you might need the Instant or Delayed mode. Default is set to Instant.
        /// </summary>
        public SingleClickMode singleClickMode = SingleClickMode.Instant;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnClickSound = false;
        /// <summary>
        /// UnityEvent invoked when on click has been captured by the system.
        /// </summary>
        public UnityEvent OnClick = new UnityEvent();
        /// <summary>
        /// Selects what type of animation this button is using OnClick.
        /// </summary>
        public ButtonAnimationType onClickAnimationType = ButtonAnimationType.Punch;
        /// <summary>
        /// Punch Animation Preset Category Name
        /// </summary>
        public string onClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Punch Animation Preset Name
        /// </summary>
        public string onClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Punch Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnClickPunchPresetAtRuntime = false;
        /// <summary>
        /// Punch Animation Settings
        /// </summary>
        public Punch onClickPunch = new Punch();
        /// <summary>
        /// State Animation Preset Category Name
        /// </summary>
        public string onClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// State Animation Preset Name
        /// </summary>
        public string onClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the State Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnClickStatePresetAtRuntime = false;
        /// <summary>
        /// State Animation Settings
        /// </summary>
        public Anim onClickState = new Anim(Anim.AnimationType.State);
        /// <summary>
        /// Toggles the OnDoubleClick functionality.
        /// </summary>
        public bool useOnDoubleClick = false;
        /// <summary>
        /// Time interval used to register a double click. This is the time interval calculated between two sequential clicks to determine if either a double click, or two separate clicks occurred.
        /// </summary>
        public float doubleClickRegisterInterval = DOUBLE_CLICK_REGISTER_INTERVAL;
        /// <summary>
        /// Internal variable used to calculate the time interval between two sequencial clicks.
        /// </summary>
        private float doubleClickTimeoutCounter = 0f;
        /// <summary>
        /// Internal variabled that is marked as true after one click occured.
        /// </summary>
        private bool clickedOnce = false;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
  
        public bool useOnLongClick = false;

        public string normalLoopPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Loop Animation Preset Name
        /// </summary>
        public string normalLoopPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Loop Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadNormalLoopPresetAtRuntime = false;
        /// <summary>
        /// Loop Animation Settings
        /// </summary>
        public Loop normalLoop = new Loop();
        /// <summary>
        /// Internal variable that is marked as true when normal loop animation is playing.
        /// </summary>
        private bool normalLoopIsPlaying = false;

      
        public bool IsSelected
        {
            get
            {
                if(EventSystem.current == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
                return EventSystem.current == null ? false : EventSystem.current.currentSelectedGameObject == gameObject;
            }
        }
        /// <summary>
        /// Internal variable that holds a reference to the RectTransform component.
        /// </summary>
        private RectTransform m_rectTransform;
        /// <summary>
        /// Returns the RectTransform component.
        /// </summary>
        public RectTransform RectTransform { get { if(m_rectTransform == null) { m_rectTransform = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>(); } return m_rectTransform; } }

        /// <summary>
        /// Internal variable that holds a reference to the Button component.
        /// </summary>
        private Button m_button;
        /// <summary>
        /// Returns the Button component.
        /// </summary>
        public Button Button { get { if(m_button == null) { m_button = GetComponent<Button>() ?? gameObject.AddComponent<Button>(); } return m_button; } }

        /// <summary>
        /// Returns true if the button's Button component is interactable. This also toggles this button's interactability.
        /// </summary>
        public bool Interactable { get { return Button.interactable; } set { Button.interactable = value; } }

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
        /// Internal variable that holds a reference to the coroutine that disables the button after click.
        /// </summary>
        private Coroutine cDisableButton;
        /// <summary>
        /// Internal variable used to determine if Start has ran. Used to differentiate code from Start to OnEnable.
        /// </summary>
        private bool Initialized = false;

        private bool isControlledByLayoutGroup = false;

        private WaitForSecondsRealtime waitDelaySelect;
        private WaitForSecondsRealtime waitDelayClick;
        private WaitForSecondsRealtime waitDuration;
        private WaitForEndOfFrame waitForEnd;
        private void Reset()
        {
            if(DUI.DUISettings == null) { return; }

            allowMultipleClicks = DUI.DUISettings.UIButton_allowMultipleClicks;
            disableButtonInterval = DUI.DUISettings.UIButton_disableButtonInterval;
            deselectButtonOnClick = DUI.DUISettings.UIButton_deselectButtonOnClick;

            
            useOnClickAnimations = DUI.DUISettings.UIButton_useOnClickAnimations;
            waitForOnClickAnimation = DUI.DUISettings.UIButton_waitForOnClickAnimation;
            singleClickMode = DUI.DUISettings.UIButton_singleClickMode;
            onClickAnimationType = DUI.DUISettings.UIButton_onClickAnimationType;
            customOnClickSound = DUI.DUISettings.UIButton_customOnClickSound;
            onClickPunchPresetCategory = DUI.DUISettings.UIButton_onClickPunchPresetCategory;
            onClickPunchPresetName = DUI.DUISettings.UIButton_onClickPunchPresetName;
            loadOnClickPunchPresetAtRuntime = DUI.DUISettings.UIButton_loadOnClickPunchPresetAtRuntime;
            onClickStatePresetCategory = DUI.DUISettings.UIButton_onClickStatePresetCategory;
            onClickStatePresetName = DUI.DUISettings.UIButton_onClickStatePresetName;
            loadOnClickStatePresetAtRuntime = DUI.DUISettings.UIButton_loadOnClickStatePresetAtRuntime;

            normalLoopPresetCategory = DUI.DUISettings.UIButton_normalLoopPresetCategory;
            normalLoopPresetName = DUI.DUISettings.UIButton_normalLoopPresetName;
            loadNormalLoopPresetAtRuntime = DUI.DUISettings.UIButton_loadNormalLoopPresetAtRuntime;

        }

        private void Awake()
        {
            startPosition = RectTransform.anchoredPosition3D;
            startRotation = RectTransform.localEulerAngles;
            startScale = RectTransform.localScale;
            startAlpha = GetComponent<CanvasGroup>() == null ? 1 : GetComponent<CanvasGroup>().alpha;
            waitForEnd = new WaitForEndOfFrame();
            AddActionsListeners();

            LoadRuntimePunchPresets();
            LoadRuntimeStatePresets();
            LoadRuntimeLoopPresets();
        }

        private void Start()
        {
            StartCoroutine(IUpdateStartPosition());
            ResetRectTransform();
            if(IsSelected) {} else { StartNormalLoop(); }
            Initialized = true;
        }

        private void OnEnable()
        {
            StartCoroutine(IUpdateStartPosition());
            if(!Initialized) { return; }
            ResetRectTransform();
            if(IsSelected) { } else { StartNormalLoop(); }
        }

        /// <summary>
        /// Handles the update of the startPosition when the RectTransfrom is a chid of a LayoutGroup.
        /// </summary>
        IEnumerator IUpdateStartPosition()
        {
            yield return null; //wait for 1 frame
            yield return null; //wait for another frame
            //we need to wait in order for the layout group to calculate the children sizes and positions

            if(transform.parent != null) //check that this is not a root object
            {
                isControlledByLayoutGroup = transform.parent.GetComponent<LayoutGroup>() != null; //check if the parent has a LayoutGroup component attached (thus it is a LayoutController)
                if(isControlledByLayoutGroup) //confirm
                {
                    startPosition = RectTransform.anchoredPosition3D; //get the set position
                }
            }
        }

        void OnDisable()
        {
            StopNormalLoop();
            if(cDisableButton != null)
            {
                StopCoroutine(cDisableButton);
                cDisableButton = null;
                EnableButton();
            }
        }

        /// <summary>
        /// Loads the animation values of all Punch Presets that are set to load at runtime.
        /// </summary>
        void LoadRuntimePunchPresets()
        {
            if(useOnClickAnimations && onClickAnimationType == ButtonAnimationType.Punch && loadOnClickPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onClickPunchPresetCategory, onClickPunchPresetName);
                if(presetPunch != null) { onClickPunch = presetPunch.Copy(); }
            }
          
        }
        /// <summary>
        /// Loads the animation values of all State Presets that are set to load at runtime.
        /// </summary>
        void LoadRuntimeStatePresets()
        {
            if(useOnClickAnimations && onClickAnimationType == ButtonAnimationType.State && loadOnClickStatePresetAtRuntime)
            {
                Anim presetState = UIAnimatorUtil.GetStateAnim(onClickStatePresetCategory, onClickStatePresetName);
                if(presetState != null) { onClickState = presetState.Copy(); }
            }
           
        }
        /// <summary>
        /// Loads the animation values of all Loop Presets that are set to load at runtime.
        /// </summary>
        void LoadRuntimeLoopPresets()
        {
            if(loadNormalLoopPresetAtRuntime)
            {
                Loop presetLoop = UIAnimatorUtil.GetLoop(normalLoopPresetCategory, normalLoopPresetName);
                if(presetLoop != null) { normalLoop = presetLoop.Copy(); }
            }
        }

        /// <summary>
        /// Initiates all the listeners for the button's actions.
        /// </summary>
        void AddActionsListeners()
        {
        }

        /// <summary>
        /// Starts playing the normal loop animations. These are the loop animations that play when the button is NOT selected.
        /// </summary>
        /// <param name="forced">Tries to play the animation even if it has not been enabled.</param>
        void StartNormalLoop(bool forced = false)
        {
            if(normalLoopIsPlaying) { return; }
            if(normalLoop == null || !normalLoop.Enabled) { return; }
            ResetRectTransform();
            UIAnimator.SetupLoops(RectTransform, startPosition, startRotation, startScale, startAlpha, normalLoop,
                                 null, null,
                                 null, null,
                                 null, null,
                                 null, null,
                                 NORMAL_LOOP_ID, true, forced);
            UIAnimator.PlayLoops(RectTransform, NORMAL_LOOP_ID);
            normalLoopIsPlaying = true;
        }
        /// <summary>
        /// Stops playing the normal loop animations and resets the RectTransform to the start values.
        /// </summary>
        void StopNormalLoop()
        {
            if(!normalLoopIsPlaying) { return; }
            UIAnimator.StopLoops(RectTransform, NORMAL_LOOP_ID);
            normalLoopIsPlaying = false;
            ResetRectTransform();
        }


        /// <summary>
        /// Resets the RectTransform to the start values.
        /// </summary>
        void ResetRectTransform()
        {
            UIAnimator.ResetTarget(RectTransform, startPosition, startRotation, startScale, startAlpha);
        }

        /// <summary>
        /// Deselects the button after the set delay.
        /// </summary>
        void DeselectButton(float delay)
        {
            if (waitDelaySelect == null)
            {
                waitDelaySelect = new WaitForSecondsRealtime(delay + DESELECT_BUTTON_DELAY);
            }
            StartCoroutine(iDeselectButton(delay));
        }
        /// <summary>
        /// Executes the button deselection in realtime.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator iDeselectButton(float delay)
        {
            yield return waitDelaySelect;
            if(EventSystem.current.currentSelectedGameObject == gameObject) { EventSystem.current.SetSelectedGameObject(null); }
        }

        /// <summary>
        /// Executes all the actions set for OnClick.
        /// </summary>
        void ExecuteOnClickActions()
        {
            if(!useOnClickAnimations)
            {
                if(!useOnDoubleClick && !useOnLongClick) { return; }
            }

            switch(onClickAnimationType)
            {
                case ButtonAnimationType.Punch: ExecutePunch(onClickPunch, deselectButtonOnClick); break;
                case ButtonAnimationType.State: ExecuteState(onClickState, deselectButtonOnClick); break;
            }
            if (waitDelayClick == null)
            {
                waitDelayClick = new WaitForSecondsRealtime(onClickPunch.TotalDuration);
            }
            StartCoroutine(iExecuteClickActionsWithDelay());
        }
        /// <summary>
        /// Simulates this button's click action, without playing the set on click sound and punch animation.
        /// </summary>
        public void SendButtonClick()
        {
            OnClick.Invoke();
        }
        /// <summary>
        /// Simulates this button's click action and plays the set on click sound and punch animation.
        /// </summary>
        public void SendButtonClick(bool playSound, bool animate, bool forced = false)
        {

            if(animate) { ExecutePunch(onClickPunch, deselectButtonOnClick, forced); }
            SendButtonClick();
        }
        IEnumerator iExecuteClickActionsWithDelay()
        {
            if (waitForOnClickAnimation)
            {
                yield return waitDelayClick;
            }
            SendButtonClick();
        }

        public void ExecutePunch(Punch punch, bool deselectButton = false, bool forced = false)
        {
            if(punch == null) { return; }
            UIAnimator.PunchMove(RectTransform, startPosition, punch, null, null, forced);
            UIAnimator.PunchRotate(RectTransform, startRotation, punch, null, null, forced);
            UIAnimator.PunchScale(RectTransform, startScale, punch, null, null, forced);
            if(deselectButton) { DeselectButton(punch.TotalDuration); }
        }

        /// <summary>
        /// Executes a given state animation.
        /// </summary>
        /// <param name="anim">State Animation Settings</param>
        /// <param name="deselectButton">Should the button get deselected after the animation finished.</param>
        /// <param name="forced">Should the animation play event if it is disabled.</param>
        public void ExecuteState(Anim anim, bool deselectButton = false, bool forced = false)
        {
            if(anim == null) { return; }
            UIAnimator.Move(RectTransform, startPosition, anim, null, null, false, forced);
            UIAnimator.Rotate(RectTransform, startRotation, anim, null, null, false, forced);
            UIAnimator.Scale(RectTransform, startScale, anim, null, null, false, forced);
            UIAnimator.Fade(RectTransform, startAlpha, anim, null, null, false, forced);
            if(deselectButton) { DeselectButton(anim.TotalDuration); }
        }

        #region EnableButton / DisableButton
        /// <summary>
        /// Sets Interactable to true.
        /// </summary>
        public void EnableButton() { Interactable = true; }
        /// <summary>
        /// Sets Interactable to false.
        /// </summary>
        public void DisableButton() { Interactable = false; }
        /// <summary>
        /// Sets Interactable to false for the set duration. After that it sets Interactable to true.
        /// </summary>
        public void DisableButton(float duration)
        {
            if(!Interactable) { return; }

            if (waitDuration == null)
            {
                waitDuration = new WaitForSecondsRealtime(duration);
            }
            cDisableButton = StartCoroutine(iDisableButton(duration));
        }
        /// <summary>
        /// Executes the button disabling in realtime.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        IEnumerator iDisableButton(float duration)
        {
            DisableButton();
            yield return waitDuration;
            EnableButton();
            cDisableButton = null;
        }
        /// <summary>
        /// Disables the button for the set disableButtonInterval value.
        /// </summary>
        void DisableButtonAfterClick() { DisableButton(disableButtonInterval); }
        #endregion

        #region OnClick
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if(debug) { Debug.Log("[UIButton] - " + name + " | OnPointerClick triggered"); }
            RegisterClick();
        }
        void RegisterClick()
        {
            StartCoroutine(ClickRegistered());
        }
        IEnumerator ClickRegistered()
        {
            if(!clickedOnce && doubleClickTimeoutCounter < doubleClickRegisterInterval)
            {
                clickedOnce = true;
            }
            else
            {
                clickedOnce = false;
                yield break; //button is pressed twice -> don't allow the second function call to fully execute
            }
            yield return waitForEnd;
            if(singleClickMode == SingleClickMode.Instant) { ExecuteClick(); }
            while(doubleClickTimeoutCounter < doubleClickRegisterInterval)
            {
                if(!clickedOnce)
                {
                    doubleClickTimeoutCounter = 0f;
                    clickedOnce = false;
                    yield break;
                }
                doubleClickTimeoutCounter += Time.unscaledDeltaTime; //increment counter by change in time between frames
                yield return null; //wait for the next frame
            }
            if(singleClickMode == SingleClickMode.Delayed) { ExecuteClick(); }
            doubleClickTimeoutCounter = 0f;
            clickedOnce = false;
            if(!allowMultipleClicks) { DisableButtonAfterClick(); }
        }
        /// <summary>
        /// Executes the OnClick trigger. You can force an execution of this trigger (regardless if it's enabled or not) by calling this method with forced set to TRUE
        /// </summary>
        /// <param name="forced">Fires this trigger regardless if it is enabled or not (default:false)</param>
        public void ExecuteClick(bool forced = false)
        {
            if(debug) { Debug.Log("[UIButton] - " + name + " | Executing OnClick" + (forced ? "initiated by force" : "")); }
            if(Interactable || forced)
            {
                ExecuteOnClickActions();
                //OnClick.Invoke();
            }
        }
        #endregion

        #region OnSelect / OnDeselect
        /// <summary>
        /// Used by ISelectHandler.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSelect(BaseEventData eventData)
        {
            if(eventData.selectedObject == gameObject)
            {
                StopNormalLoop();
            }
        }
        /// <summary>
        /// Used by IDeselectHandler.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDeselect(BaseEventData eventData)
        {
            if(eventData.selectedObject == gameObject)
            {
                StartNormalLoop();
            }
        }
        #endregion


    }
}


