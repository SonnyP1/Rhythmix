using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobingScript : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _height;
    private float _startPosZ;

    private void Start()
    {
        _startPosZ = transform.localPosition.z;
    }
    private void Update()
    {
        transform.localPosition = new Vector3(0,0, _height * Mathf.Sin(Time.realtimeSinceStartup * _speed) + _startPosZ);
    }
}
