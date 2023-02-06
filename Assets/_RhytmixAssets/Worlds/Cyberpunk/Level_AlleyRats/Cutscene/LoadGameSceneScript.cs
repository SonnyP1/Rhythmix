using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadGameSceneScript : MonoBehaviour
{
    public void LoadGameScene()
    {
        Debug.Log("LOAD GAME");
        SceneManager.LoadScene("AlleyRats_Scene",LoadSceneMode.Single);
    }
}
