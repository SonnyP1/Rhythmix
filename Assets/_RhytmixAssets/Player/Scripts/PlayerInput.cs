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
    [Header("Mobile Inputs")]
    [SerializeField] LayerMask Clickable;

    [Header("Keyboard Inputs")]
    [SerializeField] KeyCode[] KeysCodes;

    [Header("Lanes")]
    [SerializeField] Lane[] Lanes;

    [Header("Debugs")]
    [SerializeField] Text debugText;

    //private variables
    private Vector3 start;
    private float[] startTime = { 0,0,0 };
    private bool[] isHolding = { false ,false,false};

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

    void Update()
    {
        if(Time.timeScale != 0)
        {
            //MouseInput();
            PhoneInput();
            KeyboardInput();
        }
    }

    private void KeyboardInput()
    {
        InputKey(0);
        InputKey(1);
        InputKey(2);
    }

    private void InputKey(int index)
    {
        if (Lanes[index].GetTimeStampsList().Count != 0)
        {
            if (Input.GetKeyDown(KeysCodes[index]))
            {
                startTime[index] = 0;
                isHolding[index] = false;
                Lanes[index].HitNote(AttackType.Tap);
            }
            else if (Input.GetKey(KeysCodes[index]))
            {
                startTime[index] += Time.deltaTime;
                if (startTime[index] > .09 && !isHolding[index])
                {
                    isHolding[index] = true;
                    Lanes[index].HitNote(AttackType.Hold);
                }
            }
            else if (Input.GetKeyUp(KeysCodes[index]))
            {
                if (isHolding[index])
                {
                    isHolding[index] = false;
                    Lanes[index].HitNote(AttackType.Hold);
                }
            }
        }
    }

    private void PhoneInput()
    {
        if (Input.touchCount > 0)
        {
            Tap(0);
            Tap(1);
            Tap(2);
        }
    }

    private void MouseInput()
    {
        /*
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
        */
    }

    private void Tap(int index)
    {
        if (Input.GetTouch(index).phase == TouchPhase.Began)
        {
            Debug.Log("Begin Touch");
            start = Input.GetTouch(index).position; 
            Vector3 touchPosFar = new Vector3(Input.GetTouch(index).position.x, Input.GetTouch(index).position.y, Camera.main.farClipPlane);
            Vector3 touchPosClose = new Vector3(Input.GetTouch(index).position.x, Input.GetTouch(index).position.y, Camera.main.nearClipPlane);

            Vector3 touchPosFarPos = Camera.main.ScreenToWorldPoint(touchPosFar);
            Vector3 touchPosClosePos = Camera.main.ScreenToWorldPoint(touchPosClose);

            RaycastHit hit;
            if (Physics.Raycast(touchPosClosePos, touchPosFarPos - touchPosClosePos, out hit, 100f, Clickable))
            {
                print(hit.collider.gameObject.name);
                Lanes[index] = hit.collider.gameObject.GetComponent<Lane>();
            }
        }
        //holding mechanics
        if(Input.GetTouch(index).phase == TouchPhase.Moved)
        {

        }

        if(Input.GetTouch(index).phase == TouchPhase.Ended)
        {
            Debug.Log("End Touch");
            if(!Lanes[index])
            {
                Vector3 end = Input.GetTouch(index).position;
                if (Mathf.Abs(end.x - start.x) > 30)
                {
                    if (end.x > start.x)
                    {
                        Lanes[index].HitNote(AttackType.SwipeRight);
                    }
                    else
                    {
                        Lanes[index].HitNote(AttackType.SwipeLeft);
                    }

                }
                else
                {
                    Lanes[index].HitNote(AttackType.Tap);
                }
            }
        }
    }
}
