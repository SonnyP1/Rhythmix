using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobingScript : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _height;
    [SerializeField] bool bobYAxis;
    Vector3 _startPos;
    private float _startPosZ;
    private float _startPosY;
    private void Start()
    {
        _startPos = transform.localPosition;
    }
    private void Update()
    {
        if(!bobYAxis)
        {
            transform.localPosition = new Vector3(_startPos.x,_startPos.y, _height * Mathf.Sin(Time.realtimeSinceStartup * _speed) + _startPosZ);
        }
        else
        {
            transform.localPosition = new Vector3(_startPos.x, _height * Mathf.Sin(Time.realtimeSinceStartup * _speed) + _startPosY, _startPos.z);
        }
    }
}
