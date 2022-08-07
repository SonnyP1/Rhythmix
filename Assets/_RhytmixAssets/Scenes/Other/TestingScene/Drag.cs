using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject ObjectToDrag;
    public bool isActive = false;
    public bool isRotate = false;
    private void Start()
    {
        if(ObjectToDrag == null)
        {
            ObjectToDrag = this.gameObject;
        }
    }
    private void Update()
    {
        if(isActive)
        {
            if (Input.touchCount == 1)
            {
                Touch screenTouch = Input.GetTouch(0);
                if (screenTouch.phase == TouchPhase.Moved)
                {
                    if (isRotate)
                    {
                        ObjectToDrag.transform.Rotate(0f, -screenTouch.deltaPosition.x * Time.deltaTime* speed, 0f);
                    }
                    else
                    {
                        ObjectToDrag.transform.Translate(-screenTouch.deltaPosition.x * Time.deltaTime*speed, 0f, 0f);
                    }
                }
                else if (screenTouch.phase == TouchPhase.Ended)
                {
                    isActive = false;
                }
            }
        }
    }
}
