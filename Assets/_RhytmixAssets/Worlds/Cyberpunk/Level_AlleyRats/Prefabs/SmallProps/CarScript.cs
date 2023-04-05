using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    [SerializeField] Transform car;
    [SerializeField] Transform pointOne;
    [SerializeField] Transform pointTwo;

    [SerializeField] float speed1Lerp;
    [SerializeField] float speed2Lerp;

    [SerializeField] bool bHasTwoSpeeds;
    [SerializeField] bool bLoop;

    [SerializeField] float bobSpeed;
    [SerializeField] float bobHeight;
    float time;
    private float currentTimeToLerp;

    private void Start()
    {
        currentTimeToLerp = speed1Lerp;
    }
    void Update()
    {
        time += Time.deltaTime;
        car.position = Vector3.Lerp(pointOne.position,pointTwo.position,time/currentTimeToLerp);
        car.position = new Vector3(car.position.x, car.position.y + Mathf.Sin(Time.realtimeSinceStartup * bobSpeed) * bobHeight, car.position.z);

        if(bLoop && time/ currentTimeToLerp >= 1)
        {
            time = 0;
            if(bHasTwoSpeeds)
            {
                if (currentTimeToLerp == speed1Lerp)
                    currentTimeToLerp = speed2Lerp;
                else currentTimeToLerp = speed1Lerp;
            }
        }
    }
}
