using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerSceneScript : MonoBehaviour
{
    [SerializeField] float MaxTime = 30f;
    void Start()
    {
        StartCoroutine(LoadNextLevelBasedOnTimer(MaxTime));
    }
    IEnumerator LoadNextLevelBasedOnTimer(float maxTime)
    {
        float time = 0;
        while (time < maxTime)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}
