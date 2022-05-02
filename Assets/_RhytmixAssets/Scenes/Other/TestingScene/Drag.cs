using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] GameObject ObjectToDrag;

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch screenTouch = Input.GetTouch(0);
            if (screenTouch.phase == TouchPhase.Moved)
            {
                ObjectToDrag.transform.Rotate(0f, screenTouch.deltaPosition.x, 0f);
            }
        }
    }
}
