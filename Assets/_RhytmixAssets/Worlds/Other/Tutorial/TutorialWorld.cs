using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class TutorialWorld : MonoBehaviour
{
    CoreGameDataHolder gameDataHolder;
    int tutorialStep = 0;
    int tutorialTxt = 0;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] TextMeshProUGUI terminalTxt;
    [SerializeField] Animator terminalAnimator;

    [Header("In Game UI")]
    [SerializeField] GameObject healthUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject rightPlaceHolder;
    [SerializeField] GameObject leftPlaceHolder;
    [SerializeField] GameObject percentageUI;
    [SerializeField] GameObject percentageSliderUI;
    [SerializeField] GameObject scoreUI;
    [SerializeField] GameObject songNameUI;
    [SerializeField] GameObject comboUI;

    private void Start()
    {
        Debug.Log("Start Tutorial");
        gameDataHolder = FindObjectOfType<CoreGameDataHolder>();
        gameDataHolder.PauseGame();
        gameDataHolder.PauseMusic();
    }

    public void SetTutorialTxt(string txt)
    {
        terminalTxt.SetText(txt);
    }
    public void Touch()
    {
        tutorialStep++;
        Debug.Log(tutorialStep);
        string txt = "";


        switch (tutorialStep)
        {
            case 1:
                terminalAnimator.SetBool("isFade", true);
                SetTutorialTxt("This is the cyberpunk world!");
                break;
            case 2:
                SetTutorialTxt("Let's take a look at the UI");
                rightPlaceHolder.SetActive(true);
                leftPlaceHolder.SetActive(true);
                break;
            case 3:
                SetTutorialTxt("This is the health! If you take five hits you die");
                healthUI.SetActive(true);
                break;
            case 4:
                SetTutorialTxt("This is the pause button!");
                pauseUI.SetActive(true);
                break;
            case 5:
                SetTutorialTxt("This is progression of the current song your playing.");
                percentageUI.SetActive(true);
                percentageSliderUI.SetActive(true);
                break;
            case 6:
                SetTutorialTxt("This is your score count!");
                scoreUI.SetActive(true);
                break;
            case 7:
                SetTutorialTxt("This the name of the song your currently playing!");
                songNameUI.SetActive(true);
                break;
            case 8:
                SetTutorialTxt("This is your combo meter! The max is 4 times!");
                comboUI.SetActive(true);
                break;
            case 9:
                SetTutorialTxt("Lets start running! To show you the controls");
                break;
            case 10:
                Time.timeScale = 1f;
                txt = "Tap on the respective lane to do a tap attack. Try It!";
#if UNITY_STANDALONE_WIN
                txt = "Pressed left, down, right arrow key to attack in that lane. Try It!";
#endif
                SetTutorialTxt(txt);
                break;
            case 11:
                txt = "Swipe up to do a swipe attack. Try It!";
#if UNITY_STANDALONE_WIN
                txt = "Pressed F, G, and H key to swipe up attack in that lane. Try It!";
#endif
                SetTutorialTxt(txt);
                break;
            case 12:
                SetTutorialTxt("There are three types of enemies!");
                break;
            case 13:
                SetTutorialTxt("Tap Enemies");
                //Show Enemy
                break;
            case 14:
                SetTutorialTxt("Hold Enemies");
                //Show Enemy
                break;
            case 15:
                SetTutorialTxt("Swipe Up Enemies");
                //Show Enemy
                break;
            case 16:
                SetTutorialTxt("Each enemies you need to press the correct attack type");
                break;
            case 17:
                SetTutorialTxt("Tap enemies you need to tap once to defeat them");
                break;
            case 18:
                SetTutorialTxt("Hold enemies you need to tap then hold until their defeated");
                break;
            case 19:
                SetTutorialTxt("Swipe up enemies you need to use a swipe up attack to defeat them");
                break;
            default:
                FindObjectOfType<LevelAudioManager>().Skip();
                break;
        }

    }
}
