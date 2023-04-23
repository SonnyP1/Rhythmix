using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchShot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localPosition = new Vector3(2, 1.185f, 1);

        StartCoroutine(ShotSwitch());
    }

    IEnumerator ShotSwitch()
    {
        yield return new WaitForSeconds(2f);
        gameObject.transform.localPosition = new Vector3(-0.015f, 1.185f, 1.109f);

        yield return new WaitForSeconds(1f);
        gameObject.transform.localPosition = new Vector3(-1.761f,1.185f,1.145f);

        yield return new WaitForSeconds(1f);
        gameObject.transform.localPosition = new Vector3(0,.588f,3.124f);
    }
}
