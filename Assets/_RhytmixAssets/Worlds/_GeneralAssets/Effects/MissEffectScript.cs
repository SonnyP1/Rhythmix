using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissEffectScript : MonoBehaviour
{
    [SerializeField] float FadeTime;
    [SerializeField] Text text;

    private void Start()
    {
        StartCoroutine(FadeTimer());
    }
    void Update()
    {
        float newYPos = transform.position.y + (Time.deltaTime * 1);
        Vector3 newPos = new Vector3(transform.position.x,newYPos,transform.position.z);
        transform.SetPositionAndRotation(newPos,transform.rotation);

        if (text.color.a <= 0)
        {
            Destroy(gameObject);
        }

    }


    IEnumerator FadeTimer()
    {
        float Timer = 0;
        while(Timer < FadeTime)
        {
            Timer += Time.deltaTime;
            float percent = Timer / FadeTime;
            Color newColor = text.color;
            newColor.a = 1f-percent;
            text.color = newColor;
            yield return new WaitForEndOfFrame();
        }
    }
}
