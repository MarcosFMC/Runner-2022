using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : BaseState
{

    [SerializeField] private float _verticalDistance = 25f;
    [SerializeField] private float _inmunityTime = 1f;

    private float _startTime;
    public override void Construct()
    {
        _motor.hasDied = false;
        _startTime = Time.time;
        _motor.controller.enabled = false;
        _motor.transform.position = new Vector3(0, _verticalDistance, _motor.transform.position.z);
        _motor.controller.enabled = true;

        _motor.verticalVelocity = 0.0f;
        _motor.currentLane = 0;

        _motor._anim?.SetTrigger("Respawn");

        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }

    public override void Transition()
    {
        if (_motor.isGrounded && (Time.time - _startTime) > _inmunityTime)
            _motor.ChangeState(GetComponent<RunningState>());

        if (InputManager.Instance.SwipeLeft)
        {
            _motor.ChangeLane(-1);
        }

        if (InputManager.Instance.SwipeRight)
        {
            _motor.ChangeLane(1);
        }
    }

    public override void Destruct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Game);
    }


    public override Vector3 ProcessMotion()
    {
        _motor.ApplyGravity();

        Vector3 m = Vector3.zero;

        m.x = _motor.SnapToLane();
        m.y = _motor.verticalVelocity;
        m.z = _motor.baseRunSpeed;

        return m;
    }



}
