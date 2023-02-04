using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenuCanvas;
    [SerializeField] GameObject CyberpunkSongSelectionCanvas;
    private void Start()
    {
        CyberpunkSongSelectionCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    public void FantasyWorldBtnClick()
    {
        MainMenuCanvas.SetActive(false);
    }
    public void CyberpunkWorldBtnClick()
    {
        MainMenuCanvas.SetActive(false);
        CyberpunkSongSelectionCanvas.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackBtnClick()
    {
        CyberpunkSongSelectionCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    public void SongAlleyRatsBtnClick()
    {
        SceneManager.LoadScene("CinematicAlleyRats_Scene", LoadSceneMode.Single);
    }

    public void TutorialBtnClick()
    {
        SceneManager.LoadScene("TutorialWorld",LoadSceneMode.Single);
    }

    public void TestingSceneBtnClick()
    {
        SceneManager.LoadScene("TestingScene", LoadSceneMode.Single);
    }

    public void FeedbackBtnClick()
    {
        Application.OpenURL("https://forms.gle/jY27T1Rf6UGFVZw86");
    }
}
