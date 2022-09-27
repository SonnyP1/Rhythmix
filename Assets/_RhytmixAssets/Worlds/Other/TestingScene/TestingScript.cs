using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    [SerializeField] float RotateSpeed = 5f;
    [SerializeField] GameObject ThingToRotate;
    [SerializeField] float TimeToWaitThenSpin = 1f;

    private void Start()
    {
        StartCoroutine(SpinObject(TimeToWaitThenSpin));
    }

    IEnumerator SpinObject(float time)
    {
        yield return new WaitForSeconds(time);
        while(true)
        {
            ThingToRotate.transform.Rotate(0,RotateSpeed*Time.deltaTime,0);
            yield return new WaitForEndOfFrame();
        }
    }
}
