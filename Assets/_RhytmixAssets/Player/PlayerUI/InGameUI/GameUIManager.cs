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
    private bool _isGamePause = false;

    [Header("GameOverUI")]
    [SerializeField] GameObject GameOverMenu;
    [SerializeField] Animator GameOverAnimator;

    [Header("PauseUI")]
    [SerializeField] GameObject PauseMenu;
    [SerializeField] Animator PauseAnimator;

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

    //CHECK IF GAME IS OVER 
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
                Time.timeScale = 1;
                Player.StartMovement();
                coreGameData.PauseMusic();
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("EndCutscene");
    }


    public void Dead()
    {
        InGameUI.SetActive(false);
        GameOverMenu.SetActive(true);
        GameOverAnimator.SetTrigger("Open");
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
        Debug.Log("TRY AGAIN");
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
        PauseAnimator.SetTrigger("Close");
        PauseAnimator.ResetTrigger("Open");
        coreGameData.ContinueGame();
        _isGamePause = false;
    }
    private void PauseGameUI()
    {
        PauseAnimator.SetTrigger("Open");
        PauseAnimator.ResetTrigger("Close");
        coreGameData.PauseGame();
        _isGamePause = true;
    }
}
