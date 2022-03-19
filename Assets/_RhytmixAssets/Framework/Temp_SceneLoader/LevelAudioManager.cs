using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAudioManager : MonoBehaviour
{
    [SerializeField] float SongFiftyPercentDoneInSecounds;
    bool isAlreadyFiftyPercent = false;
    AudioSource _songAudioSource;
    private void Start()
    {
        _songAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_songAudioSource.time >= _songAudioSource.clip.length)
        {
            //temp for testing
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("LOAD NEXT LEVEL");
        }

        if(_songAudioSource.time > SongFiftyPercentDoneInSecounds && !isAlreadyFiftyPercent)
        {
            Debug.Log("HALF WAY THERE");
            isAlreadyFiftyPercent = true;
        }
    }
}
