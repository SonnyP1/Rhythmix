using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissEffectScript : MonoBehaviour
{
    [SerializeField] float FadeTime;
    [SerializeField] Text text;

    public Text GetText()
    {
        return text;
    }
    private void Start()
    {
        StartCoroutine(FadeTimer());
    }
    public virtual void Update()
    {
        transform.position += transform.up * 1 * Time.deltaTime;

        if (GetText().color.a <= 0)
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
            GetText().color = newColor;
            yield return new WaitForEndOfFrame();
        }
    }
}
