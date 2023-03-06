using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    [Header("TitleScreen")]
    [SerializeField] GameObject TitleScreen;
    public void PressToStartBtn()
    {
        TitleScreen.SetActive(false);
        LevelSelections[0].SetActive(true);
        MenuAudioSource.clip = Songs[1];
        MenuAudioSource.Play();
    }

    [Header("Level Selection")]
    [SerializeField] GameObject[] LevelSelections;
    private int cycleIndex = 0;
    public void CycleThroughLevels()
    {
        if(cycleIndex == 0)
        {
            LevelSelections[cycleIndex].SetActive(false);
            cycleIndex = 1;
            LevelSelections[cycleIndex].SetActive(true);
            MenuAudioSource.clip = Songs[2];
        }
        else
        {
            LevelSelections[cycleIndex].SetActive(false);
            cycleIndex = 0;
            LevelSelections[cycleIndex].SetActive(true);
            MenuAudioSource.clip = Songs[1];
        }

        MenuAudioSource.Play();
    }


    [Header("AlleyRatsSelection")]
    [SerializeField] VideoPlayer AlleyRatVideoPlayer;
    [SerializeField] VideoClip EasyClip;
    [SerializeField] VideoClip MediumClip;
    [SerializeField] VideoClip HardClip;

    [Header("Song")]
    [SerializeField] AudioSource MenuAudioSource;
    [SerializeField] AudioClip[] Songs;

    public void AlleyRatsEasyOver()
    {
        AlleyRatVideoPlayer.clip = EasyClip;
    }
    public void AlleyRatsMediumOver()
    {
        AlleyRatVideoPlayer.clip = MediumClip;
    }
    public void AlleyRatsHardOver()
    {
        AlleyRatVideoPlayer.clip = HardClip; 
    }

    //**********************************************************************Load Levels
    public void LoadTutorialLevel()
    {
        PlayerPrefs.SetString("ChartDirPath","Tutorial.mid");
        SceneManager.LoadScene("TutorialWorld",LoadSceneMode.Single);
    }
    public void LoadAlleyRatsEasy()
    {
        PlayerPrefs.SetString("ChartDirPath", "AlleyRats_Easy.mid");
        SceneManager.LoadScene("CinematicAlleyRats_Scene", LoadSceneMode.Single);
    }
    public void LoadAlleyRatsMedium()
    {
        PlayerPrefs.SetString("ChartDirPath", "AlleyRats_Medium.mid");
        SceneManager.LoadScene("CinematicAlleyRats_Scene", LoadSceneMode.Single);
    }
    public void LoadAlleyRatsHard()
    {
        PlayerPrefs.SetString("ChartDirPath", "AlleyRats_Hard.mid");
        SceneManager.LoadScene("CinematicAlleyRats_Scene", LoadSceneMode.Single);
    }
    //**********************************************************************Others
    public void FeedbackBtnClick()
    {
        Application.OpenURL("https://forms.gle/jY27T1Rf6UGFVZw86");
    }
}
