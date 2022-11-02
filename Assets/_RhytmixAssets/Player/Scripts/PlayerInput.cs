using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    private Vector3[] start = { Vector3.zero, Vector3.zero , Vector3.zero };
    private float[] startTime = { 0,0,0 };
    private bool[] isHolding = { false ,false,false};

    private void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }
    void Update()
    {
        if(Time.timeScale != 0)
        {
            PhoneInput();
            KeyboardInput();
            Click();
        }
    }




    private void LogCallback(string condition, string stackTrace, LogType type)
    {
        if (debugText != null)
        {
            debugText.text += "\n" + condition;
        }
    }
    private void KeyboardInput()
    {
        InputKey(0);
        InputKey(1);
        InputKey(2);

        //debuging
        if(Input.GetKeyDown(KeyCode.F))
        {
            Lanes[0].HitNote(AttackType.SwipeUp);
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            Lanes[1].HitNote(AttackType.SwipeUp);
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            Lanes[2].HitNote(AttackType.SwipeUp);
        }
    }
    private void InputKey(int index)
    {

        if (Input.GetKeyDown(KeysCodes[index]))
        {
            startTime[index] = 0;
            isHolding[index] = false;
            Lanes[index].HitNote(AttackType.Tap);

            isHolding[index] = true;
            Lanes[index].HitNote(AttackType.Hold);
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

    private void PhoneInput()
    {
        if(Input.touchCount != null)
        {
            if(Input.touchCount == 1)
            {
                Tap(0);
            }
            else if(Input.touchCount == 2)
            {
                Tap(0);
                Tap(1);
            }
            else
            {
                Tap(0);
                Tap(1);
                Tap(2);
            }
        }
    }
    private void Tap(int index)
    {
        if (Input.GetTouch(index).phase == TouchPhase.Began)
        {
            Debug.Log("Begin Touch");
            start[index] = Input.GetTouch(index).position;
            Vector3 touchPosFar = new Vector3(Input.GetTouch(index).position.x, Input.GetTouch(index).position.y, Camera.main.farClipPlane);
            Vector3 touchPosClose = new Vector3(Input.GetTouch(index).position.x, Input.GetTouch(index).position.y, Camera.main.nearClipPlane);

            Vector3 touchPosFarPos = Camera.main.ScreenToWorldPoint(touchPosFar);
            Vector3 touchPosClosePos = Camera.main.ScreenToWorldPoint(touchPosClose);

            RaycastHit hit;
            if (Physics.Raycast(touchPosClosePos, touchPosFarPos - touchPosClosePos, out hit, 100f, Clickable))
            {
                Debug.Log("I hit something!");
                Lanes[index] = hit.collider.gameObject.GetComponent<Lane>();
                Lanes[index].HitNote(AttackType.Tap);
            }
        }

        if(Input.GetTouch(index).phase == TouchPhase.Stationary)
        {
            if (!isHolding[index])
            {
                Debug.Log("Starting Holding");
                isHolding[index] = true;
                Lanes[index].HitNote(AttackType.Hold);
                Debug.Log(Lanes[index].name);
            }
        }

        if(Input.GetTouch(index).phase == TouchPhase.Ended)
        {
            Debug.Log("End Touch");
            Vector3 end = Input.GetTouch(index).position;

            if (isHolding[index])
            {
                isHolding[index] = false;
                Lanes[index].HitNote(AttackType.Hold);
            }
            else if(Mathf.Abs(end.y - start[index].y) > 60)
            {
                Lanes[index].HitNote(AttackType.SwipeUp);
            }
            else if (Mathf.Abs(end.x - start[index].x) > 30)
            {
                if (end.x > start[index].x)
                {
                    Lanes[index].HitNote(AttackType.SwipeRight);
                }
                else
                {
                    Lanes[index].HitNote(AttackType.SwipeLeft);
                }
            }
        }
    }




    private void Click()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Begin Click");
            Vector3 touchPosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
            Vector3 touchPosClose = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

            Vector3 touchPosFarPos = Camera.main.ScreenToWorldPoint(touchPosFar);
            Vector3 touchPosClosePos = Camera.main.ScreenToWorldPoint(touchPosClose);

            RaycastHit hit;
            Debug.DrawRay(touchPosClosePos, touchPosFarPos - touchPosClosePos,Color.red,10f);
            if (Physics.Raycast(touchPosClosePos, touchPosFarPos - touchPosClosePos, out hit, 100f, Clickable))
            {
                Debug.Log(hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<Lane>().HitNote(AttackType.Tap);
            }
        }
        */
    }
}
