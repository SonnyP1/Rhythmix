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

    [Header("Multiplier UI")]
    [SerializeField] TextMeshProUGUI MultiplierText;
    [SerializeField] TextMeshProUGUI MultiplierXText;
    [SerializeField] float GlowIntensity;

    [SerializeField] Slider SongSlider;
    [SerializeField] TextMeshProUGUI PercentageSong;
    [SerializeField] TextMeshProUGUI SongTitle;

    [Header("Player UI")]
    [SerializeField] GameObject[] PlayerHealthBars;



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


        if(_scoreKeeper != null)
        {
            UpdateScore(Color.red);
            UpdateMultiplier(1,Color.red);
            UpdateAccuracy();
        }
        StartCoroutine(CheckTime());
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
    public void UpdateScore(Color newColor)
    {
        foreach (TextMeshProUGUI textPro in ScoreText)
        {
            textPro.color = newColor;
            textPro.text = _scoreKeeper.GetScore().ToString();
        }
    }

    public void UpdateMultiplier(int multiplier , Color color)
    {
        MultiplierText.color = color;
        MultiplierXText.color = color;
        if(MultiplierXText.materialForRendering.HasProperty("_GlowColor"))
        {
            float factor = Mathf.Pow(2, GlowIntensity);
            Color glowColor = new Color(color.r * factor, color.g * factor, color.b * factor,color.a);
            MultiplierXText.materialForRendering.SetColor("_GlowColor",glowColor);
        }

        MultiplierText.text = multiplier.ToString();
    }

    private void UpdateHealthBarColor(Color newColor)
    {
        foreach(GameObject healthObj in PlayerHealthBars)
        {
            healthObj.GetComponent<HealthBarUIComponentData>().FullHealthBarSegment.color = newColor;
        }
    }
    public void UpdatePlayerHealthBar(int healthRemaining , int maxHealth)
    {
        if (PlayerHealthBars.Length == 0)
        {
            return;
        }

        switch (healthRemaining)
        {
            case 5:
                UpdateHealthBarColor(Color.cyan);
                break;
            case 4:
                UpdateHealthBarColor(Color.green);
                break;
            case 3:
                UpdateHealthBarColor(Color.yellow);
                break;
            case 2:
                UpdateHealthBarColor(new Color(139f,64f,0));
                break;
            case 1:
                UpdateHealthBarColor(Color.clear);
                break;
            default:
                UpdateHealthBarColor(Color.clear);
                break;

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
