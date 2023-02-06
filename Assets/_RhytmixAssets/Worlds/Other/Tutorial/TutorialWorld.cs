using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
using System.IO;
using System.Text;
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

    StringBuilder tutorialStringBuilder = new StringBuilder();

    private void Start()
    {
        Debug.Log("Start Tutorial");
        gameDataHolder = FindObjectOfType<CoreGameDataHolder>();
        gameDataHolder.PauseGame();
        gameDataHolder.PauseMusic();

        AssignStringBuilder();
    }

    public void SetTutorialTxt(string txt)
    {
        terminalTxt.SetText(txt);
    }
    public void Touch()
    {
        tutorialStep++;
        string txt = "";
        for(int i = 0; i < tutorialStringBuilder.Length; i++)
        {
            if(tutorialStringBuilder[i] == '~')
            {
                tutorialStringBuilder.Remove(0,i+1);
                break;
            }
            txt += tutorialStringBuilder[i];
        }

        SetTutorialTxt(txt);
        switch (tutorialStep)
        {
            case 1:
                terminalAnimator.SetBool("isFade", true);
                break;
            case 2:
                rightPlaceHolder.SetActive(true);
                leftPlaceHolder.SetActive(true);
                break;
            case 3:
                healthUI.SetActive(true);
                break;
            case 4:
                pauseUI.SetActive(true);
                break;
            case 5:
                percentageUI.SetActive(true);
                percentageSliderUI.SetActive(true);
                break;
            case 6:
                scoreUI.SetActive(true);
                break;
            case 7:
                songNameUI.SetActive(true);
                break;
            case 8:
                comboUI.SetActive(true);
                break;
            case 9:
                Time.timeScale = 1;
                break;
        }
    }

    public void AssignStringBuilder()
    {
        string path = "Assets/_RhytmixAssets/Worlds/Other/Tutorial/TutorialTxt.txt";

        var lines = File.ReadAllLines(path);
        foreach(var line in lines)
        {
            tutorialStringBuilder.Append(line);
        }

    }
}
