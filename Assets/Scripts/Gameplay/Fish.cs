using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerMotor player = other.gameObject.GetComponent<PlayerMotor>();
        if (player != null)
            PickUpFish();
    }

    public void PickUpFish()
    {
        GetComponent<SphereCollider>().enabled = false;
        anim?.SetTrigger("Pickup");
        GameStats.Instance.CollectedFish();
    }

    public void OnShowChunk()
    {
        GetComponent<SphereCollider>().enabled = true;
        anim?.SetTrigger("Idle"); 
    }
}
