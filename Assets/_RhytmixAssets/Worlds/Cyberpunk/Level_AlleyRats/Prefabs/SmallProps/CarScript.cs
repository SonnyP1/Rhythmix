using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    [Header("Car Locations")]
    [SerializeField] Transform car;
    [SerializeField] Transform pointOne;
    [SerializeField] Transform pointTwo;


    [Header("Car Speeds")]
    [SerializeField] float speed1Lerp;
    [SerializeField] float speed2Lerp;

    [SerializeField] bool bHasTwoSpeeds;
    [SerializeField] bool bLoop;

    [SerializeField] float bobSpeed;
    [SerializeField] float bobHeight;

    [Header("Car Looks")]
    [SerializeField] float Glow;
    [SerializeField] Material CarMtl;
    [SerializeField] GameObject CarObj;
    [SerializeField] Color CarColor;
    private float time;
    private float currentTimeToLerp;

    private void Start()
    {
        currentTimeToLerp = speed1Lerp;
        Material materialToAssign = new Material(CarMtl);

        float factor = Mathf.Pow(2,Glow);
        CarColor = new Color(CarColor.r *factor,CarColor.g*factor,CarColor.b*factor, 255f);

        if(CarObj.GetComponent<Renderer>().material.HasProperty("_EmissionColor"))
        {
            //Debug.Log("Property does exist");
            materialToAssign.SetColor("_EmissionColor", CarColor);
        }
        CarObj.GetComponent<Renderer>().material = materialToAssign;
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
