using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] AudioSource Music;
    [SerializeField] GameObject PauseMenu;
    private bool _isGamePause = false;

    private void Start()
    {
        PauseMenu.SetActive(false);
    }
    public void ReturnToMainMenuBtn()
    {
        UnPauseGame();
        SceneManager.LoadScene("MainMenuScene",LoadSceneMode.Single);
    }
    public void PauseBtnClicked()
    {
        Debug.Log("Pause Button CLICKED");
        if(!_isGamePause)
        {
            PauseGame();
        }
        else
        {
            UnPauseGame();
        }
    }
    private void UnPauseGame()
    {
        Music.Play();
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        _isGamePause = false;
    }
    private void PauseGame()
    {
        Music.Pause();
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        _isGamePause = true;
    }
}
