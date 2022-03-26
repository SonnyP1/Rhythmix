using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BasicPlayer : MonoBehaviour
{
    float _gravity = -9.8f;
    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        Vector3 playerVelocity = Vector3.zero;

        if (characterController.isGrounded == false)
        {
            playerVelocity.y = Time.deltaTime * _gravity;
        }

        characterController.Move(playerVelocity);
    }
}
