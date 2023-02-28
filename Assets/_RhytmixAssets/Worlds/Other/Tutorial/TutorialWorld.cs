using System.Collections;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialWorld : MonoBehaviour
{
    [Header("Tutorial UI")]
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] TextMeshProUGUI terminalTxt;
    [SerializeField] Animator terminalAnimator;
    [SerializeField] Image dot;

    [Header("Color")]
    [SerializeField] Color uiColor;
    [SerializeField] Color greenColor;

    [Header("In Game UI")]
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject healthUI;
    [SerializeField] Image[] HealthImages;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject rightPlaceHolder;
    [SerializeField] GameObject leftPlaceHolder;
    [SerializeField] GameObject percentageUI;
    [SerializeField] TextMeshProUGUI percentageNumber;
    [SerializeField] GameObject percentageSliderUI;
    [SerializeField] Image percentageSliderFillImage;
    [SerializeField] GameObject scoreUI;
    [SerializeField] GameObject songNameUI;
    [SerializeField] TextMeshProUGUI songNameTxt;
    [SerializeField] GameObject comboUI;
    [SerializeField] TextMeshProUGUI comboXTxt;
    [SerializeField] TextMeshProUGUI comboNumberTxt;
    [SerializeField] GameObject hitLoc;
    [SerializeField] GameObject characterIcons;

    [Header("Tutorial Notes")]
    [SerializeField] GameObject tapNote;
    [SerializeField] GameObject holdNote;
    [SerializeField] GameObject swipeUpNote;


    //Private variables
    CoreGameDataHolder gameDataHolder;
    StringBuilder tutorialStringBuilder = new StringBuilder();
    int tutorialStep = 0;


    //Private Coroutine
    List<Coroutine> songNameCores = new List<Coroutine>();
    Coroutine scoreCore;
    Coroutine flashingImageCore;
    Coroutine textWritingCore;
    List<Coroutine> healthCores= new List<Coroutine>();
    Coroutine pauseCore;
    List<Coroutine> percentageCores = new List<Coroutine>();
    List<Coroutine> comboCores = new List<Coroutine>();
    Coroutine hitLocCore;
    private void Start()
    {
        Debug.Log("Start Tutorial");
        gameDataHolder = FindObjectOfType<CoreGameDataHolder>();
        gameDataHolder.PauseGame();
        gameDataHolder.PauseMusic();

        flashingImageCore = StartCoroutine(FlashColor(dot,dot.color,new Color(0,0,0,0)));
        AssignStringBuilder();
    }
    public void Touch()
    {
        tutorialStep++;
        string txt = "";
        dot.color = greenColor;

        for (int i = 0; i < tutorialStringBuilder.Length; i++)
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
                characterIcons.SetActive(true);
                rightPlaceHolder.SetActive(true);
                leftPlaceHolder.SetActive(true);
                break;
            case 3:
                healthUI.SetActive(true);
                foreach(Image image in HealthImages)
                {
                    healthCores.Add(StartCoroutine(FlashColor(image, uiColor, greenColor)));
                }
                break;
            case 4:
                StopCores(healthCores);
                ResetColor(HealthImages);

                pauseUI.SetActive(true);
                pauseCore = StartCoroutine(FlashColor(pauseUI.GetComponent<Image>(), uiColor, greenColor));
                break;
            case 5:
                StopCoroutine(pauseCore);
                ResetColor(pauseUI.GetComponent<Image>());

                AddCoroutinesFlashColor(percentageCores,new MaskableGraphic[] 
                {
                    percentageUI.GetComponent<TextMeshProUGUI>(),
                    percentageNumber,
                    percentageSliderFillImage
                });
                percentageUI.SetActive(true);
                percentageSliderUI.SetActive(true);
                break;
            case 6:
                StopCores(percentageCores);
                ResetColor(new MaskableGraphic[] 
                {                    
                    percentageUI.GetComponent<TextMeshProUGUI>(),
                    percentageNumber,
                    percentageSliderFillImage
                });

                scoreCore = StartCoroutine(FlashColor(scoreUI.GetComponent<TextMeshProUGUI>(), uiColor, greenColor));
                scoreUI.SetActive(true);
                break;
            case 7:
                StopCoroutine(scoreCore);
                ResetColor(scoreUI.GetComponent<TextMeshProUGUI>());

                AddCoroutinesFlashColor(songNameCores, new MaskableGraphic[]
                {
                    songNameUI.GetComponent<Image>(),
                    songNameTxt
                });
                songNameUI.SetActive(true);
                break;
            case 8:
                StopCores(songNameCores);
                ResetColor(new MaskableGraphic[]
                {
                    songNameUI.GetComponent<Image>(),
                    songNameTxt
                });

                AddCoroutinesFlashColor(comboCores,new MaskableGraphic[]
                {
                    comboXTxt,
                    comboNumberTxt
                });
                comboUI.SetActive(true);
                break;
            case 9:
                StopCores(comboCores);
                ResetColor(new MaskableGraphic[]
                {
                    comboXTxt,
                    comboNumberTxt
                });

                hitLoc.SetActive(true);
                hitLocCore = StartCoroutine(FlashColor(hitLoc.GetComponent<LineRenderer>(),hitLoc.GetComponent<LineRenderer>().startColor,greenColor));
                break;
            case 10:
                StopCoroutine(hitLocCore);
                hitLoc.GetComponent<LineRenderer>().startColor = Color.blue;
                hitLoc.GetComponent<LineRenderer>().endColor = Color.blue;
                Time.timeScale = 1;
                break;
            case 15:
                //show tap
                tapNote.SetActive(true);
                break;
            case 17:
                Destroy(tapNote);
                //show hold
                holdNote.SetActive(true);
                break;
            case 19:
                Destroy(holdNote);
                //show swipe up
                swipeUpNote.SetActive(true);
                break;
            case 22:
                Destroy(swipeUpNote);
                Destroy(tutorialCanvas);
                FindObjectOfType<LevelAudioManager>().Skip();
                pauseUI.GetComponent<Button>().interactable = true;
                Destroy(gameObject);
                break;
        }
    }
    private void AddCoroutinesFlashColor(List<Coroutine> cores,MaskableGraphic[] objectsToAdd)
    {
        foreach(MaskableGraphic obj in objectsToAdd)
        {
            cores.Add(StartCoroutine(FlashColor(obj,uiColor,greenColor)));
        }
    }
    private void ResetColor(MaskableGraphic objRef)
    {
        objRef.color = uiColor;
    }
    private void ResetColor(MaskableGraphic[] objRef)
    {
        foreach(MaskableGraphic obj in objRef)
        {
            obj.color = uiColor;
        }
    }
    private void StopCores(List<Coroutine> cores)
    {
        foreach(Coroutine core in cores)
        {
            StopCoroutine(core);
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
        foreach (char letter in txt)
        {
            terminalTxt.text += letter;
            terminalTxt.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(terminalTxt.bounds.size.x,50);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        flashingImageCore = StartCoroutine(FlashColor(dot,greenColor,new Color(0,0,0,0)));
    }
    IEnumerator FlashColor(Object objectRef,Color one, Color two)
    {
        if(objectRef as MaskableGraphic)
        {
            MaskableGraphic objectAsMaskableGraphic = (MaskableGraphic)objectRef;
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                objectAsMaskableGraphic.color = one;
                yield return new WaitForSecondsRealtime(0.5f);
                objectAsMaskableGraphic.color = two;
            }
        }
        else if(objectRef as LineRenderer)
        {
            LineRenderer objectAsLineRenderer = (LineRenderer)objectRef;
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                objectAsLineRenderer.startColor = one;
                objectAsLineRenderer.endColor = one;
                yield return new WaitForSecondsRealtime(0.5f);
                objectAsLineRenderer.startColor = two;
                objectAsLineRenderer.endColor = two;
            }
        }
    }
}
