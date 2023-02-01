using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadGameSceneScript : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene(2,LoadSceneMode.Single);
    }
}
