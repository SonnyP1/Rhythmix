using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    private bool _isGamePause = false;

    [Header("Score UI")]
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI MultiplierText;

    [Header("Player UI")]
    [SerializeField] Image PlayerHealthBar;


    private ScoreKeeper _scoreKeeper;
    private AudioSource _music;

    private void Start()
    {
        CoreGameDataHolder coreGameData = FindObjectOfType<CoreGameDataHolder>();
        _scoreKeeper = coreGameData.GetScoreKeeper();
        _music = coreGameData.GetMusic();

        PauseMenu.SetActive(false);
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
        _music.Play();
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        _isGamePause = false;
    }
    private void PauseGame()
    {
        _music.Pause();
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        _isGamePause = true;
    }
}
