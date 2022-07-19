using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private static InputManager _instance;
    public static InputManager Instance { get { return _instance;} }

    private RunnerInputAction _actionScheme;

    [SerializeField] private float _sqrSwipeDeadZone = 50.0f; 
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        SetupControl();
    }

    private void LateUpdate()
    {
        ResetInputs();
    }

    private void ResetInputs()
    {
        _tap = _swipeLeft = _swipeRight = _swipeUp = _swipeDown = false;
    }

    #region public properties

    public bool Tap { get { return _tap; } }
    public Vector2 TouchPosition { get { return _touchPosition; } }
    public bool SwipeLeft { get { return _swipeLeft; } }
    public bool SwipeRight { get { return _swipeRight; } }
    public bool SwipeUp { get { return _swipeUp; } }
    public bool SwipeDown { get { return _swipeDown; } }

    #endregion

    #region privates
    private bool _tap;
    private Vector2 _touchPosition;
    private Vector2 _startDrag;
    private bool _swipeLeft;
    private bool _swipeRight;
    private bool _swipeUp;
    private bool _swipeDown;
    #endregion


    private void SetupControl()
    {
        _actionScheme = new RunnerInputAction();
        _actionScheme.Gameplay.Tap.performed += ctx => OnTap(ctx);
        _actionScheme.Gameplay.TouchPosition.performed += ctx => OnPosition(ctx);
        _actionScheme.Gameplay.StarDrag.performed += ctx => OnStartDrag(ctx);
        _actionScheme.Gameplay.EndDrag.performed += ctx => EndDrag(ctx);
    }

    private void EndDrag(InputAction.CallbackContext ctx)
    {
        Vector2 delta = _touchPosition - _startDrag;
        float sqrDistance = delta.sqrMagnitude;

        if(sqrDistance > _sqrSwipeDeadZone)
        {
            float x = Math.Abs(delta.x);
            float y = Math.Abs(delta.y);

            if(x > y)
            {
                if (delta.x > 0)
                    _swipeRight = true;
                else
                    _swipeLeft = true;
            }
            else
            {

                if (delta.y > 0)
                    _swipeUp = true;
                else
                    _swipeDown = true;
            }
        }

        _startDrag = Vector2.zero;
    }

    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        _startDrag = _touchPosition;
    }

    private void OnPosition(InputAction.CallbackContext ctx)
    {
        _touchPosition = ctx.ReadValue<Vector2>();
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        _tap = true;
    }

    public void OnEnable()
    {
        _actionScheme.Enable();
    }

    public void OnDisable()
    {
        _actionScheme.Disable();
    }
}
