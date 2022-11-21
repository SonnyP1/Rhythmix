using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    [SerializeField] Transform car;
    [SerializeField] Transform pointOne;
    [SerializeField] Transform pointTwo;
    [SerializeField] float timeToLerp;
    float time;
    void Update()
    {
        time += Time.deltaTime;
        car.position = Vector3.Lerp(pointOne.position,pointTwo.position,time/timeToLerp);
    }
}
