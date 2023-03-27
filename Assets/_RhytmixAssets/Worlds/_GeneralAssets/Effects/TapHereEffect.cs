using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapHereEffect : MissEffectScript
{


    public override void Update()
    {
        transform.position += transform.forward * -0.1f * Time.deltaTime;
        if (GetText().color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
