using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum AttackType
{Tap,SwipeRight,SwipeLeft,SwipeUp };


public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text debugText;
    [SerializeField] LayerMask Clickable;
    Lane lane;
    Vector3 start;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseInput();

        if (Input.touchCount > 0)
        {
            Tap(Input.GetTouch(0));
            Tap(Input.GetTouch(1));
            Tap(Input.GetTouch(3));

            if (debugText != null)
            {
                debugText.text = Input.GetTouch(0).position.ToString();
            }
        }
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            start = Input.mousePosition;
            Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
            Vector3 mousePosClose = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

            Vector3 mousePosFarPos = Camera.main.ScreenToWorldPoint(mousePosFar);
            Vector3 mousePosClosePos = Camera.main.ScreenToWorldPoint(mousePosClose);


            RaycastHit hit;
            if (Physics.Raycast(mousePosClosePos, mousePosFarPos - mousePosClosePos, out hit, 100f, Clickable))
            {
                //print(hit.collider.gameObject.name);
                lane = hit.collider.gameObject.GetComponent<Lane>();
            }
        }

        if(Input.GetMouseButton(0))
        {
            //during the click
        }

        if(Input.GetMouseButtonUp(0))
        {
            //end of click
            Vector3 end = Input.mousePosition;
            if(lane != null)
            {
                if (Mathf.Abs(end.x - start.x) > 30)
                {
                    if(end.x > start.x)
                    {
                        lane.HitNote(AttackType.SwipeRight);
                    }
                    else
                    {
                        lane.HitNote(AttackType.SwipeLeft);
                    }

                }
                else if(Mathf.Abs(end.y - start.y) > 50)
                {
                    lane.HitNote(AttackType.SwipeUp);
                }
                else
                {
                    lane.HitNote(AttackType.Tap);
                }
            }
        }
    }

    private void Tap(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            start = touch.position; 
            Vector3 touchPosFar = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.farClipPlane);
            Vector3 touchPosClose = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.nearClipPlane);

            Vector3 touchPosFarPos = Camera.main.ScreenToWorldPoint(touchPosFar);
            Vector3 touchPosClosePos = Camera.main.ScreenToWorldPoint(touchPosClose);

            RaycastHit hit;
            if (Physics.Raycast(touchPosClosePos, touchPosFarPos - touchPosClosePos, out hit, 100f, Clickable))
            {
                print(hit.collider.gameObject.name);
                lane = hit.collider.gameObject.GetComponent<Lane>();
            }
        }

        if(touch.phase == TouchPhase.Moved)
        {

        }

        if(touch.phase == TouchPhase.Ended)
        {
            Vector3 end = touch.position;
            if (Mathf.Abs(end.x - start.x) > 30)
            {
                if (end.x > start.x)
                {
                    lane.HitNote(AttackType.SwipeRight);
                }
                else
                {
                    lane.HitNote(AttackType.SwipeLeft);
                }

            }
            else
            {
                lane.HitNote(AttackType.Tap);
            }
        }
    }
}
