using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenuCanvas;
    [SerializeField] GameObject FantasySongSelectionCanvas;
    [SerializeField] GameObject CyberpunkSongSelectionCanvas;
    private void Start()
    {
        FantasySongSelectionCanvas.SetActive(false);
        CyberpunkSongSelectionCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    public void FantasyWorldBtnClick()
    {
        MainMenuCanvas.SetActive(false);
        FantasySongSelectionCanvas.SetActive(true);
    }
    public void CyberpunkWorldBtnClick()
    {
        MainMenuCanvas.SetActive(false);
        CyberpunkSongSelectionCanvas.SetActive(true);
    }

    public void BackBtnClick()
    {
        FantasySongSelectionCanvas.SetActive(false);
        CyberpunkSongSelectionCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }
    public void SongHeroJourneyBtnClick()
    {
        SceneManager.LoadScene("Lush_DarkForest_Scene",LoadSceneMode.Single);
    }
    public void SongAlleyRatsBtnClick()
    {
        SceneManager.LoadScene("Utopia_Slums_Scene",LoadSceneMode.Single);
    }
}
