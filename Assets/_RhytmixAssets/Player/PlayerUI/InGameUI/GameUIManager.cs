using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject InGameUI;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject GameOverMenu;
    [SerializeField] GameObject WinMenu;
    private bool _isGamePause = false;

    [Header("Score UI")]
    [SerializeField] TextMeshProUGUI[] ScoreText;
    [SerializeField] TextMeshProUGUI[] AccuracyText;
    [SerializeField] TextMeshProUGUI MultiplierText;
    [SerializeField] Slider SongSlider;
    [SerializeField] TextMeshProUGUI SongTitle;

    [Header("Player UI")]
    [SerializeField] Image PlayerHealthBar;
    [SerializeField] VideoPlayer _videoPlayer;

    private ScoreKeeper _scoreKeeper;
    private AudioSource _music;
    private void Start()
    {
        Time.timeScale = 1;
        CoreGameDataHolder coreGameData = FindObjectOfType<CoreGameDataHolder>();


        _scoreKeeper = coreGameData.GetScoreKeeper();
        _music = coreGameData.GetMusic();
        SongTitle.text = coreGameData.GetSongTitle();

        WinMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        PauseMenu.SetActive(false);

        if(_scoreKeeper != null)
        {
            UpdateScore();
            UpdateMultiplier();
            UpdateAccuracy();
        }
    }

    private void Update()
    {
        SongSlider.value = _music.time/ _music.clip.length;

        if(SongSlider.value >= 0.99)
        {
            InGameUI.SetActive(false);
            WinMenu.SetActive(true);
            StopMusic();
            Time.timeScale = 0;
        }
    }

    public void Dead()
    {
        InGameUI.SetActive(false);
        GameOverMenu.SetActive(true);

        StopMusic();
    }

    private void StopMusic()
    {
        _music.Pause();
    }
    public void UpdateAccuracy()
    {
        foreach(TextMeshProUGUI textPro in AccuracyText)
        {
            textPro.text =  ((_scoreKeeper.GetAccuracy()*100)).ToString("F0") + "%";
        }
    }
    public void UpdateScore()
    {
        foreach (TextMeshProUGUI textPro in ScoreText)
        {
            textPro.text = _scoreKeeper.GetScore().ToString();
        }
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
        SceneManager.LoadScene("MainMenuScene",LoadSceneMode.Single);
    }

    public void SkipCutScene()
    {
        _videoPlayer.Stop();
        _music.GetComponent<LevelAudioManager>().Skip();
    }
    public void TryAgainBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
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
        StopMusic();
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        _isGamePause = true;
    }
}
