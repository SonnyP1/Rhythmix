using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Linq;

public class MainMenuManager : MonoBehaviour
{
    [Header("TitleScreen")]
    [SerializeField] GameObject TitleScreen;

    [Header("Level Selection")]
    [SerializeField] GameObject[] LevelSelections;

    [Header("Buttons")]
    [SerializeField] Button _rightBtn;
    [SerializeField] Button _leftBtn;

    private List<Vector3> _objPosition = new List<Vector3>();
    private List<Vector3> _objScale = new List<Vector3>();
    public void PressToStartBtn()
    {
        TitleScreen.SetActive(false);
        LevelSelections[0].SetActive(true);
    }

    private void Start()
    {
        foreach(GameObject obj in LevelSelections)
        {
            _objPosition.Add(obj.transform.localPosition);
            _objScale.Add(obj.transform.localScale);
        }

        _rightBtn.onClick.AddListener(() => Selection(1));
        _leftBtn.onClick.AddListener(() => Selection(-1));
    }

    private void Selection(int shift)
    {


        LevelSelections = ShiftIndex(LevelSelections,shift).ToArray();
        StopAllCoroutines();
        StartCoroutine(LerpToLoc());
    }

    IEnumerator LerpToLoc()
    {
        List<Color> orginalColor = new List<Color>();

        foreach(GameObject selection in LevelSelections)
        {
            selection.SetActive(true);
            orginalColor.Add(selection.GetComponent<Image>().color);
        }
        LevelSelections[2].SetActive(false);
        LevelSelections[3].SetActive(false);

        List<Vector3> positionToLerp = new List<Vector3>();
        List<Vector3> startPos = new List<Vector3>();

        List<Vector3> scaleToLerp = new List<Vector3>();
        List<Vector3> startScale = new List<Vector3>();


        for (int i = 0; i < LevelSelections.Length; i++)
        {
            startPos.Add(LevelSelections[i].transform.localPosition);
            positionToLerp.Add(_objPosition[i]);

            startScale.Add(LevelSelections[i].transform.localScale);
            scaleToLerp.Add(_objScale[i]);
        }

        float time = 0f;
        float maxTime = 0.5f;
        while(true)
        {
            time += Time.deltaTime;
            float percent = time / maxTime;

            //Lerp Position-Color-Scale
            for(int i = 0; i < LevelSelections.Length; i++)
            {
                LevelSelections[i].transform.localPosition = Vector3.Lerp(startPos[i],positionToLerp[i],percent);
                LevelSelections[i].transform.localScale = Vector3.Lerp(startScale[i],scaleToLerp[i],percent);

                if(i == 0)
                    LevelSelections[i].GetComponent<Image>().color = Color.Lerp(orginalColor[i],Color.white,percent);
                else
                    LevelSelections[i].GetComponent<Image>().color = Color.Lerp(orginalColor[i],Color.gray,percent);
            }

            if(percent >= 1f)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private static int MathMod(int a, int b)
    {
        int c = ((a % b) + b) % b;
        return c;
    }
    public static IEnumerable<T> ShiftIndex<T>(IList<T> values, int shift)
    {
        for (int index = 0; index < values.Count; index++)
        {
            yield return values[MathMod(index - shift, values.Count)];
        }
    }

    //**********************************************************************Load Levels
    public void LoadTutorialLevel()
    {
        PlayerPrefs.SetString("ChartDirPath","Tutorial.mid");
        SceneManager.LoadScene("TutorialWorld",LoadSceneMode.Single);
    }
    public void LoadAlleyRatsEasy()
    {
        PlayerPrefs.SetString("ChartDirPath", "AlleyRats_Easy.mid");
        SceneManager.LoadScene("CinematicAlleyRats_Scene", LoadSceneMode.Single);
    }
    public void LoadAlleyRatsMedium()
    {
        PlayerPrefs.SetString("ChartDirPath", "AlleyRats_Medium.mid");
        SceneManager.LoadScene("CinematicAlleyRats_Scene", LoadSceneMode.Single);
    }
    public void LoadAlleyRatsHard()
    {
        PlayerPrefs.SetString("ChartDirPath", "AlleyRats_Hard.mid");
        SceneManager.LoadScene("CinematicAlleyRats_Scene", LoadSceneMode.Single);
    }
    //**********************************************************************Others
    public void FeedbackBtnClick()
    {
        Application.OpenURL("https://forms.gle/jY27T1Rf6UGFVZw86");
    }

    public void ExitBtnClick()
    {
        Debug.Log("Quit Button!");
        Application.Quit();
    }
}
