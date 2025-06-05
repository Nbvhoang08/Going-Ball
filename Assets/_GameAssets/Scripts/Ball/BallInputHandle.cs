using UnityEngine;
public class BallInputHandle : MonoBehaviour
{

    [SerializeField] private BallController _ballController;
    private Vector3 _inputDirection;
    public Vector3 InputDirection => _inputDirection;
    public Vector2 inputDirection;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isInputDown = false;
    public float inputLength;
    [SerializeField] private float swipeThreshold = 30f; // đơn vị: pixel


    private void Awake()
    {
        _ballController = GetComponent<BallController>();
    }

    public void HandleInput()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    //private void HandleTouchInput()
    //{
    //    if (Touchscreen.current == null || !Touchscreen.current.primaryTouch.press.isPressed)
    //    {
    //        if (isInputDown)
    //        {
    //            isInputDown = false;
    //            inputDirection = Vector2.zero;
    //            inputLength = 0f;
    //        }
    //        return;
    //    }

    //    var touch = Touchscreen.current.primaryTouch;
    //    var touchPhase = touch.phase.ReadValue();

    //    if (touchPhase == TouchPhase.Ended && isInputDown)
    //    {
    //        isInputDown = false;
    //        inputDirection = Vector2.zero;
    //        inputLength = 0f;
    //        return;
    //    }

    //    if (touchPhase == TouchPhase.Began)
    //    {
    //        startPosition = touch.position.ReadValue();
    //        endPosition = startPosition;
    //        isInputDown = true;
    //        return;
    //    }

    //    if (isInputDown)
    //    {
    //        endPosition = touch.position.ReadValue();
    //        float moveMagnitude = (endPosition - startPosition).magnitude;
    //        if (moveMagnitude > deadZone)
    //        {
    //            CalculateSwipeForce();
    //            startPosition = endPosition;
    //        }
    //    }
    //    else
    //    {
    //        inputDirection = Vector2.zero;
    //        inputLength = 0f;
    //    }
    //}
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended && isInputDown)
            {
                isInputDown = false;
                return;
            }

            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
                endPosition = touch.position;
                isInputDown = true;
                return;
            }

            if (isInputDown)
            {
                endPosition = touch.position;
                CalculateSwipeForce();
                startPosition = endPosition;
            }
            else
            {
                inputDirection = Vector2.zero;
                inputLength = 0f;
            }
        }
    }
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonUp(0) && isInputDown)
        {
            isInputDown = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            isInputDown = true;
            return;
        }

        if (isInputDown)
        {
            endPosition = Input.mousePosition;
            CalculateSwipeForce();
            startPosition = endPosition;
        }
        else
        {
            inputDirection = Vector2.zero;
            inputLength = 0f;
        }
    }
    //private void HandleMouseInput()
    //{
    //    if (Mouse.current == null) return;

    //    if (Mouse.current.leftButton.wasReleasedThisFrame && isInputDown)
    //    {
    //        isInputDown = false;
    //        return;
    //    }

    //    if (Mouse.current.leftButton.wasPressedThisFrame)
    //    {
    //        startPosition = Mouse.current.position.ReadValue();
    //        isInputDown = true;
    //        return;
    //    }

    //    if (isInputDown)
    //    {
    //        endPosition = Mouse.current.position.ReadValue();
    //        if (endPosition != startPosition)
    //        {
    //            CalculateSwipeForce();
    //            startPosition = endPosition;
    //        }
    //    }
    //    else
    //    {
    //        inputDirection = Vector2.zero;
    //        inputLength = 0f;
    //    }
    //}

    private void CalculateSwipeForce()
    {
        inputDirection = endPosition - startPosition;
        inputLength = inputDirection.magnitude / Screen.dpi;
        //inputLength = Mathf.Max(inputLength, 0.05f);
        if (_ballController == null)
        {
            return;
        }

        //_ballController.AddForceSpeedUp(inputDirection, inputLength);
    }
    public void ResetInput()
    {
        inputDirection = Vector2.zero;
        inputLength = 0f;
        isInputDown = false;
    }
}
