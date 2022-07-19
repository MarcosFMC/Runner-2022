using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFloor : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Material _material;
    [SerializeField] private float _snowSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.forward * _player.transform.position.z;
        _material.SetVector("Vector2_4467DBC8", new Vector3(0, transform.position.z) * _snowSpeed);
    }
}
