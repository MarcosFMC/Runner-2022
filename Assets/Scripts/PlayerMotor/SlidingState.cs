using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingState : BaseState
{
    public float slideDuration = 1.0f;

    private Vector3 initialCenter;
    private float initialSize;
    private float slideStart;


    public override void Construct()
    {

        _motor._anim?.SetTrigger("Slide");
        slideStart = Time.time;
        initialSize = _motor.controller.height;
        initialCenter = _motor.controller.center;


        _motor.controller.height = initialSize * 0.5f;
        _motor.controller.center = initialCenter * 0.5f;
    }

    public override void Destruct()
    {
        _motor.controller.height = initialSize;
        _motor.controller.center = initialCenter;
        _motor._anim?.SetTrigger("Running");
    }

    public override void Transition()
    {

        if (InputManager.Instance.SwipeLeft)
        {
            _motor.ChangeLane(-1);
        }

        if (InputManager.Instance.SwipeRight)
        {
            _motor.ChangeLane(1);
        }

        if (!_motor.isGrounded)
            _motor.ChangeState(GetComponent<FallingState>());

        if (InputManager.Instance.SwipeUp)
            _motor.ChangeState(GetComponent<JumpingState>());

        //Interesante
        if (Time.time - slideStart > slideDuration)
        {
            _motor.ChangeState(GetComponent<RunningState>());
        }
    }

    public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;

        m.x = _motor.SnapToLane();
        m.y = -1;
        m.z = _motor.baseRunSpeed;

        return m;
    }
}
