using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadGameSceneScript : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 1.0f;
    }
    public void LoadGameScene()
    {
        Debug.Log("LOAD GAME");
        SceneManager.LoadScene("AlleyRats_Scene",LoadSceneMode.Single);
    }
}
