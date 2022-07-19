using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BaseState
{

    public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;

        m.x = _motor.SnapToLane();
        m.y = -1;
        m.z = _motor.baseRunSpeed;

        return m;
    }

    public override void Transition()
    {
        if (InputManager.Instance.SwipeDown )
        {
            _motor.ChangeState(GetComponent<SlidingState>());
        }
        if (InputManager.Instance.SwipeLeft)
        {
            _motor.ChangeLane(-1);
        }

        if (InputManager.Instance.SwipeRight)
        {
            _motor.ChangeLane(1);
        }

        if (InputManager.Instance.SwipeUp && _motor.isGrounded)
        {
            _motor.ChangeState(GetComponent<JumpingState>());
        }
        if (!_motor.isGrounded)
            _motor.ChangeState(GetComponent<FallingState>());
    }
}
