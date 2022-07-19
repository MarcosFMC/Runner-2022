using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BaseState
{

    [SerializeField] private Vector3 knockbackForce = new Vector3(0, 4f, -3f);
    private Vector3 currentKnockBack;


    public override void Construct()
    {
        _motor.hasDied = true;
        _motor._anim?.SetTrigger("Death");
        currentKnockBack = knockbackForce;
    }
    public override Vector3 ProcessMotion()
    {
        Vector3 m = currentKnockBack;

        currentKnockBack = new Vector3(0f, currentKnockBack.y -= _motor.gravity * Time.deltaTime, currentKnockBack.z += 2.0f * Time.deltaTime);

        if(currentKnockBack.z > 0)
        {
            currentKnockBack.z = 0;
            GameManager.Instance.ChangeState(GameManager.Instance.GetComponent<GameStateDeath>());
        }
        return currentKnockBack;
    }
 

}
