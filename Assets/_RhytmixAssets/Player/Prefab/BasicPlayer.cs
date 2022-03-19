using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BasicPlayer : MonoBehaviour
{
    [SerializeField] float MovementSpeed;
    float _gravity = -9.8f;
    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        Vector3 playerVelocity = new Vector3(-MovementSpeed * Time.deltaTime, 0, 0);

        if (characterController.isGrounded == false)
        {
            playerVelocity.y = Time.deltaTime * _gravity;
        }

        characterController.Move(playerVelocity);
    }
}
