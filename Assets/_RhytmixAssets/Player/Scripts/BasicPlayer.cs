using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BasicPlayer : MonoBehaviour
{
    float _gravity = -9.8f;
    CharacterController characterController;
    HeathComponent healthComp;
    bool isDead = false;
    [SerializeField] GameObject _cameraTransform;
    [SerializeField] Animator[] playerAnimators;
    [SerializeField] GameObject[] ClickHereEffect;
    private GameUIManager _UI;

    public void ActivateClickEffect()
    {
        foreach(var obj in ClickHereEffect)
        {
            obj.SetActive(true);
        }
    }
    private void Start()
    {
        CoreGameDataHolder data = FindObjectOfType<CoreGameDataHolder>();
        _UI = data.GetGameUIManager();
        characterController = GetComponent<CharacterController>();
        healthComp = GetComponent<HeathComponent>();
        healthComp.onDeath += Death;
    }
    public void StartMovement()
    {
        _cameraTransform.transform.parent = null;
        StartCoroutine(Movement());
    }

    IEnumerator Movement()
    {
        while(true)
        {
            Debug.Log("MOVE");
            characterController.Move(characterController.transform.forward * Time.deltaTime*10f);
            characterController.Move(-characterController.transform.up * Time.deltaTime * 10f);
            yield return new WaitForEndOfFrame();
        }
    }
    private void Death()
    {
        if(!isDead)
        {
            isDead = false;
            if(playerAnimators != null)
            {
                foreach(Animator animator in playerAnimators)
                {
                    Debug.Log(animator.name);
                    animator.SetTrigger("DeathTrigger");
                    animator.SetBool("DeathBool",true);
                }
            }
            _UI.Dead();
        }
    }

    void Update()
    {
        Vector3 playerVelocity = Vector3.zero;
        playerVelocity.y = Time.deltaTime * _gravity;
        characterController.Move(playerVelocity);
    }
}
