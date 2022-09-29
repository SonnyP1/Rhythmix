using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum AttackType
{Tap,SwipeRight,SwipeLeft,SwipeUp,Hold,EndHold };


public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text debugText;
    [SerializeField] LayerMask Clickable;
    [Header("Debugging Inputs")]
    [SerializeField] KeyCode leftInput;
    [SerializeField] Lane leftLane;
    [SerializeField] KeyCode middleInput;
    [SerializeField] Lane middleLane;
    [SerializeField] KeyCode rightInput;
    [SerializeField] Lane rightLane;


    Lane lane;
    Vector3 start;
    float[] startTime = { 0,0,0 };
    bool[] isHolding = { false ,false,false};
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }

    private void LogCallback(string condition, string stackTrace, LogType type)
    {
        if (debugText != null)
        {
            debugText.text += "\n" + condition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            MouseInput();
            PhoneInput();
            KeyboardInput();
        }
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(leftInput))
        {
            startTime[0] = 0;
            isHolding[0] = false;
        }
        else if(Input.GetKey(leftInput))
        {
            startTime[0] += Time.deltaTime;
            if(startTime[0] > .09 && !isHolding[0])
            {
                isHolding[0] = true;
                leftLane.HitNote(AttackType.Hold);
            }
        }
        else if(Input.GetKeyUp(leftInput))
        {
            if(isHolding[0])
            {
                isHolding[0]= false;
                leftLane.HitNote(AttackType.Hold);
            }
            else
            {
                leftLane.HitNote(AttackType.Tap);
            }
        }


        if (Input.GetKeyDown(middleInput))
        {
            startTime[1] = 0;
            isHolding[1] = false;
        }
        else if (Input.GetKey(middleInput))
        {
            startTime[1] += Time.deltaTime;
            if (startTime[1] > .09 && !isHolding[1])
            {
                isHolding[1] = true;
                middleLane.HitNote(AttackType.Hold);
            }
        }
        else if (Input.GetKeyUp(middleInput))
        {
            if (isHolding[1])
            {
                isHolding[1] = false;
                middleLane.HitNote(AttackType.Hold);
            }
            else
            {
                middleLane.HitNote(AttackType.Tap);
            }
        }




        if (Input.GetKeyDown(rightInput))
        {
            startTime[2] = 0;
            isHolding[2] = false;
        }
        else if (Input.GetKey(rightInput))
        {
            startTime[2] += Time.deltaTime;
            if (startTime[2] > .09 && !isHolding[2])
            {
                isHolding[2] = true;
                rightLane.HitNote(AttackType.Hold);
            }
        }
        else if (Input.GetKeyUp(rightInput))
        {
            if (isHolding[2])
            {
                isHolding[2] = false;
                rightLane.HitNote(AttackType.Hold);
            }
            else
            {
                rightLane.HitNote(AttackType.Tap);
            }
        }
    }

    private void PhoneInput()
    {
        //how many fingers are on the screen at a time
        if (Input.touchCount > 0)
        {
            //print(Input.touchCount);
            Tap(Input.GetTouch(0));
            //Tap(Input.GetTouch(1));
            //Tap(Input.GetTouch(3));
        }
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTime[0] = 0;
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
            startTime[0] += Time.deltaTime;
            if (startTime[0] > .5)
            {
                lane.HitNote(AttackType.Hold);
            }
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
            Debug.Log("Begin Touch");
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
        //holding mechanics
        if(touch.phase == TouchPhase.Moved)
        {

        }

        if(touch.phase == TouchPhase.Ended)
        {
            Debug.Log("End Touch");
            if(!lane)
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

                lane = null;
            }
        }
    }
}
