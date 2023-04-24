using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Linq;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("TitleScreen")]
    [SerializeField] GameObject _titleScreen;

    [Header("Level Selection")]
    [SerializeField] GameObject[] _levelSelections;

    [Header("Buttons")]
    [SerializeField] Button _rightBtn;
    [SerializeField] Button _leftBtn;
    [SerializeField] Button _startBtn;

    [SerializeField] Button _easyBtn;
    [SerializeField] Button _medBtn;
    [SerializeField] Button _hardBtn;

    [Header("Canvas Groups")]
    [SerializeField] CanvasGroup _titleGroup;
    [SerializeField] CanvasGroup _alleyRatsGroup;
    [SerializeField] CanvasGroup _tutorialGroup;

    [Header("Audio Sources")]
    [SerializeField] AudioSource _alleyRatsSong;
    [SerializeField] AudioSource _tutorialSong;

    private List<Vector3> _objPosition = new List<Vector3>();
    private List<Vector3> _objScale = new List<Vector3>();

    private bool _tutorialSelected = true;
    public void PressToStartBtn()
    {
        _titleScreen.SetActive(false);
        _levelSelections[0].SetActive(true);
    }

    private void Start()
    {
        foreach(GameObject obj in _levelSelections)
        {
            _objPosition.Add(obj.transform.localPosition);
            _objScale.Add(obj.transform.localScale);
        }

        _rightBtn.onClick.AddListener(() => Selection(1));
        _leftBtn.onClick.AddListener(() => Selection(-1));

        _easyBtn.onClick.AddListener(LoadAlleyRatsEasy);
        _medBtn.onClick.AddListener(LoadAlleyRatsMedium);
        _hardBtn.onClick.AddListener(LoadAlleyRatsHard);

        _startBtn.onClick.AddListener(StartSelectedScreen);
    }

    private void StartSelectedScreen()
    {
        _titleScreen.SetActive(false);
        _
    }


    private void Selection(int shift)
    {
        if(_tutorialSelected)
        {
            _tutorialSelected = false;
        }
        else
        {
            _tutorialSelected = true;
        }


        _levelSelections = ShiftIndex(_levelSelections,shift).ToArray();
        StopAllCoroutines();
        StartCoroutine(LerpToLoc());
    }

    IEnumerator LerpToLoc()
    {
        List<Color> orginalColor = new List<Color>();

        foreach(GameObject selection in _levelSelections)
        {
            selection.SetActive(true);
            orginalColor.Add(selection.GetComponent<RawImage>().color);
        }
        _levelSelections[2].SetActive(false);
        _levelSelections[3].SetActive(false);

        List<Vector3> positionToLerp = new List<Vector3>();
        List<Vector3> startPos = new List<Vector3>();

        List<Vector3> scaleToLerp = new List<Vector3>();
        List<Vector3> startScale = new List<Vector3>();


        for (int i = 0; i < _levelSelections.Length; i++)
        {
            startPos.Add(_levelSelections[i].transform.localPosition);
            positionToLerp.Add(_objPosition[i]);

            startScale.Add(_levelSelections[i].transform.localScale);
            scaleToLerp.Add(_objScale[i]);
        }

        float time = 0f;
        float maxTime = 0.5f;
        while(true)
        {
            time += Time.deltaTime;
            float percent = time / maxTime;

            //Lerp Position-Color-Scale
            for(int i = 0; i < _levelSelections.Length; i++)
            {
                _levelSelections[i].transform.localPosition = Vector3.Lerp(startPos[i],positionToLerp[i],percent);
                _levelSelections[i].transform.localScale = Vector3.Lerp(startScale[i],scaleToLerp[i],percent);

                if(i == 0)
                    _levelSelections[i].GetComponent<RawImage>().color = Color.Lerp(orginalColor[i],Color.white,percent);
                else
                    _levelSelections[i].GetComponent<RawImage>().color = Color.Lerp(orginalColor[i],Color.gray,percent);
            }

            float oppositePercent = 1.0f - percent;
            _titleGroup.alpha = oppositePercent;

            if (_tutorialSelected)
            {
                _alleyRatsGroup.alpha = oppositePercent;
                _alleyRatsSong.volume = oppositePercent;
                _tutorialGroup.alpha = 0f;
            }
            else
            {
                _tutorialGroup.alpha = oppositePercent;
                _tutorialSong.volume = oppositePercent;
                _alleyRatsGroup.alpha = 0f;
            }


            if (percent >= 1f)
            {
                if(_tutorialSelected)
                    _titleGroup.GetComponent<TextMeshProUGUI>().text = "TUTORIAL";
                else
                    _titleGroup.GetComponent<TextMeshProUGUI>().text = "ALLEY RATS";

                break;
            }
            yield return new WaitForEndOfFrame();
        }

        time = 0f;
        while(true)
        {
            time += Time.deltaTime;
            float percent = time / maxTime;

            _titleGroup.alpha = percent;

            if(_tutorialSelected)
            {
                _tutorialGroup.alpha = percent;
                _tutorialSong.volume = percent;
            }
            else
            {
                _alleyRatsGroup.alpha = percent;
                _alleyRatsSong.volume = percent;
            }

            if (percent >= 1)
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
