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
    void Update()
    {
        if(Time.timeScale != 0)
        {
            PhoneInput();
            KeyboardInput();
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
    }
    private void InputKey(int index)
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
            if (startTime[index] > .1f && !isHolding[index])
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
    private void PhoneInput()
    {
        if (Input.touchCount > 0)
        {
            Tap(0);
            Tap(1);
            Tap(2);
        }
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
                Lanes[index].HitNote(AttackType.Tap);
            }
        }
        //holding mechanics
        if(Input.GetTouch(index).phase == TouchPhase.Moved || Input.GetTouch(index).phase == TouchPhase.Stationary)
        {

            if (!isHolding[index])
            {
                isHolding[index] = true;
                Lanes[index].HitNote(AttackType.Hold);
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
            else if (Mathf.Abs(end.x - start.x) > 30)
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
        }
    }


}
