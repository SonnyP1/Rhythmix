using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] AudioSource Music;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI MultiplierText;
    private bool _isGamePause = false;

    [SerializeField] Image PlayerHealthBar;
    ScoreKeeper _scoreKeeper;

    private void Start()
    {
        PauseMenu.SetActive(false);
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(_scoreKeeper != null)
        {
            UpdateScore();
            UpdateMultiplier();
        }
    }

    public void UpdateScore()
    {
        ScoreText.text = _scoreKeeper.GetScore().ToString();
    }

    public void UpdateMultiplier()
    {
        MultiplierText.text =  "X" +_scoreKeeper.GetMultiplier().ToString();
    }
    public void UpdatePlayerHealthBar(float percent)
    {
        if (PlayerHealthBar == null)
        {
            return;
        }
        PlayerHealthBar.fillAmount = percent;
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
