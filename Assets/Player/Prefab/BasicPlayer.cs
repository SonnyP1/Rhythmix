using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BasicPlayer : MonoBehaviour
{
    [SerializeField] float MovementSpeed;
    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        characterController.Move(new Vector3(-MovementSpeed*Time.deltaTime, 0,0));
    }
}
