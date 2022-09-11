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
    Animator[] playerAnimators;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        healthComp = GetComponent<HeathComponent>();
        healthComp.onDeath += Death;
    }

    private void Death()
    {
        if(!isDead)
        {
            print("I Die");
            isDead = false;
            foreach(Animator animator in playerAnimators)
            {
                animator.SetTrigger("DeathTrigger");
            }
        }
    }

    void Update()
    {
        Vector3 playerVelocity = Vector3.zero;

        if (!IsOnGround())
        {
            playerVelocity.y = Time.deltaTime * _gravity;
            //fix this
            playerVelocity = AdjustVelocityToSlope(playerVelocity);
        }
        characterController.Move(playerVelocity);
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);
        if ((Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f)))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up,hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;
            Debug.Log(adjustedVelocity);
            return adjustedVelocity;
        }
        return velocity;

    }


    private bool IsOnGround()
    {
        var ray = new Ray(transform.position, Vector3.down);
        if ((Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f)))
        {
            //Debug.Log(hitInfo.collider.gameObject.name);
            return true;
        }
        return false;
    }
}
