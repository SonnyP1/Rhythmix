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

    HeathComponent _playerHPComp;

    //private variables
    private Vector3[] start = { Vector3.zero, Vector3.zero , Vector3.zero };
    private float[] startTime = { 0,0,0 };
    private bool[] isHolding = { false ,false,false};

    private void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }
    private void Start()
    {
        _playerHPComp = GetComponent<HeathComponent>();
    }
    void Update()
    {
        if(_playerHPComp.GetHealth() >= 0)
        {
            PhoneInput();
            KeyboardInput();
            //Click();
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
        if(Input.touchCount > 0)
        {
            Tap(0);
            if(Input.touchCount > 1)
            {
                Tap(1);
                if (Input.touchCount > 2)
                {
                    Tap(2);
                }
            }
        }
    }
    private void Tap(int index)
    {
        if (Input.GetTouch(index).phase == TouchPhase.Began)
        {
            start[index] = Input.GetTouch(index).position;
            Vector3 touchPosFar = new Vector3(Input.GetTouch(index).position.x, Input.GetTouch(index).position.y, Camera.main.farClipPlane);
            Vector3 touchPosClose = new Vector3(Input.GetTouch(index).position.x, Input.GetTouch(index).position.y, Camera.main.nearClipPlane);

            Vector3 touchPosFarPos = Camera.main.ScreenToWorldPoint(touchPosFar);
            Vector3 touchPosClosePos = Camera.main.ScreenToWorldPoint(touchPosClose);

            RaycastHit hit;
            if (Physics.Raycast(touchPosClosePos, touchPosFarPos - touchPosClosePos, out hit, 100f, Clickable))
            {
                Lanes[index] = hit.collider.gameObject.GetComponent<Lane>();
                Lanes[index].HitNote(AttackType.Tap);
                Lanes[index].HitNote(AttackType.Hold);
            }
        }

        if(Input.GetTouch(index).phase == TouchPhase.Stationary)
        {
            if (!isHolding[index])
            {
                isHolding[index] = true;
            }
        }

        if(Input.GetTouch(index).phase == TouchPhase.Ended)
        {
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
        }
    }
}
