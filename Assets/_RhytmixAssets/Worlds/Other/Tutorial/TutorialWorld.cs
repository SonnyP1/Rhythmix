using System.Collections;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
public class TutorialWorld : MonoBehaviour
{
    [Header("Tutorial UI")]
    [SerializeField] TextMeshProUGUI terminalTxt;
    [SerializeField] Animator terminalAnimator;
    [SerializeField] Image dot;

    [Header("In Game UI")]
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject healthUI;
    [SerializeField] Image[] HealthImages;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject rightPlaceHolder;
    [SerializeField] GameObject leftPlaceHolder;
    [SerializeField] GameObject percentageUI;
    [SerializeField] GameObject percentageSliderUI;
    [SerializeField] GameObject scoreUI;
    [SerializeField] GameObject songNameUI;
    [SerializeField] GameObject comboUI;
    [SerializeField] GameObject hitLoc;

    CoreGameDataHolder gameDataHolder;
    StringBuilder tutorialStringBuilder = new StringBuilder();
    Coroutine textWritingCore;
    Coroutine flashingImageCore;
    int tutorialStep = 0;
    private void Start()
    {
        Debug.Log("Start Tutorial");
        gameDataHolder = FindObjectOfType<CoreGameDataHolder>();
        gameDataHolder.PauseGame();
        gameDataHolder.PauseMusic();

        flashingImageCore = StartCoroutine(FlashImage(dot,dot.color,new Color(0,0,0,0)));
        AssignStringBuilder();
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

        if(textWritingCore != null)
            StopCoroutine(textWritingCore);
        textWritingCore = StartCoroutine(SetTutorialTxtConsole(txt));

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
                foreach(Image image in HealthImages)
                {
                    StartCoroutine(FlashImage(image,image.color,Color.green));
                }
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
                hitLoc.SetActive(true);
                break;
            case 10:
                Time.timeScale = 1;
                break;
        }
    }

    public void AssignStringBuilder()
    {
        string path = Application.streamingAssetsPath + "/TutorialTxt.txt";
#if UNITY_STANDALONE_WIN
        path = Application.streamingAssetsPath + "/TutorialTxtWindow.txt";
#endif

        var lines = File.ReadAllLines(path);
        foreach(var line in lines)
        {
            tutorialStringBuilder.Append(line);
        }

    }


    IEnumerator SetTutorialTxtConsole(string txt)
    {
        StopCoroutine(flashingImageCore);
        Color color = dot.color;
        color.a = 255;
        dot.color = color;


        terminalTxt.text = "";

        foreach(char letter in txt)
        {
            terminalTxt.text += letter;
            terminalTxt.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(terminalTxt.bounds.size.x,50);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        flashingImageCore = StartCoroutine(FlashImage(dot,Color.green,new Color(0,0,0,0)));
    }

    IEnumerator FlashImage(Image flash,Color one, Color two)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            flash.color = one;
            yield return new WaitForSecondsRealtime(0.5f);
            flash.color = two;
        }
    }
}
