using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] BasicPlayer Player;
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
    [SerializeField] TextMeshProUGUI PercentageSong;
    [SerializeField] TextMeshProUGUI SongTitle;

    [Header("Player UI")]
    [SerializeField] GameObject[] PlayerHealthBars;

    [Header("OnFire UI")]
    [SerializeField] GameObject[] onFireUIObjs;

    private ScoreKeeper _scoreKeeper;
    private AudioSource _music;
    private CoreGameDataHolder coreGameData;
    private void Start()
    {
        if(FindObjectOfType<TutorialWorld>() == null)
        {
            Time.timeScale = 1;
        }

        coreGameData = FindObjectOfType<CoreGameDataHolder>();


        _scoreKeeper = coreGameData.GetScoreKeeper();
        _music = coreGameData.GetMusic();
        SongTitle.text = coreGameData.GetSongTitle();

        WinMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OnFireUI(false);

        if(_scoreKeeper != null)
        {
            UpdateScore();
            UpdateMultiplier();
            UpdateAccuracy();
        }
        StartCoroutine(CheckTime());
    }

    public void OnFireUI(bool val)
    {
        foreach(GameObject obj in onFireUIObjs)
        {
            obj.SetActive(val);
        }
    }
    IEnumerator CheckTime()
    {
        while(true)
        {
            SongSlider.value = _music.time / _music.clip.length;
            PercentageSong.text = ((SongSlider.value / SongSlider.maxValue)*100).ToString("f0");
            if (SongSlider.value >= 0.99f)
            {
                Debug.Log("End Game!");
                InGameUI.SetActive(false);
                WinMenu.SetActive(true);
                Time.timeScale = 1;
                Player.StartMovement();
                coreGameData.PauseMusic();
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }


    public void Dead()
    {
        InGameUI.SetActive(false);
        GameOverMenu.SetActive(true);

        coreGameData.PauseMusic();
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
        MultiplierText.text =  _scoreKeeper.GetMultiplier().ToString();
    }
    public void UpdatePlayerHealthBar(int healthRemaining , int maxHealth)
    {
        if (PlayerHealthBars.Length == 0)
        {
            return;
        }

        for(int i = maxHealth; i > 0;i--)
        {
            if(healthRemaining < i)
            {
                PlayerHealthBars[i - 1].GetComponent<Animator>().SetBool("HasHealth",false);
            }
            else
            {
                PlayerHealthBars[i - 1].GetComponent<Animator>().SetBool("HasHealth",true);
            }
        }
    }

    public void ReturnToMainMenuBtn()
    {
        SceneManager.LoadScene("MainMenuScene",LoadSceneMode.Single);
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
            PauseGameUI();
        }
        else
        {
            UnPauseGameUI();
        }
    }
    private void UnPauseGameUI()
    {
        coreGameData.ContinueGame();
        PauseMenu.SetActive(false);
        _isGamePause = false;
    }
    private void PauseGameUI()
    {
        coreGameData.PauseGame();
        PauseMenu.SetActive(true);
        _isGamePause = true;
    }
}
