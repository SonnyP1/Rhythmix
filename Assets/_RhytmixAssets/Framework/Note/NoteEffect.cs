using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEffect : MonoBehaviour
{
    private float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        if(time > 2.0f)
        {
            Destroy(gameObject);
        }
    }

}
