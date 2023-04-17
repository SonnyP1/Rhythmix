using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] Animator _policeAnimator;
    [SerializeField] Animator _cameraAnimator;

    [Header("Buttons")]
    [SerializeField] Button _continueButton;
    [SerializeField] Button _returnToMenu;

    [Header("Other UI Elements")]
    [SerializeField] RectTransform _creditTextTransform;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Canvas _scoreCanvas;
    [SerializeField] TextMeshProUGUI _scoreTxt;
    [SerializeField] TextMeshProUGUI _accuracyTxt;

    [Header("Tablet Geo")]
    [SerializeField] GameObject TabletInHand;
    [SerializeField] GameObject TabletOnHip;
    [SerializeField] GameObject TabletSmash;


    public void HideHipTablet()
    {
        TabletOnHip.SetActive(false);
        TabletInHand.SetActive(true);
    }

    public void HideInHandTablet()
    {
        TabletInHand.SetActive(false);
        TabletSmash.SetActive(true);
    }
    private void Start()
    {
        _scoreTxt.text = PlayerPrefs.GetFloat("Score").ToString();
        _accuracyTxt.text = (PlayerPrefs.GetFloat("Accuracy") * 100).ToString("F0") + "%";

        _returnToMenu.enabled = false;
        _scoreCanvas.gameObject.SetActive(false);

        _continueButton.onClick.AddListener(ContinueScene);
        _returnToMenu.onClick.AddListener(ReturnToMenu);
    }

    public void ShowScore()
    {
        _scoreCanvas.gameObject.SetActive(true);
        StartCoroutine(ShowScoreCanvas());
    }

    IEnumerator ShowScoreCanvas()
    {
        float time = 0;
        float maxTime = 0.1f;
        RectTransform scoreTransform = _scoreCanvas.GetComponent<RectTransform>();

        while (true)
        {
            time += Time.deltaTime;
            float percent = time / maxTime;
            scoreTransform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(0.01f, 0.008f, 0.008f),percent);

            if (percent >= 1)
                break;
            yield return new WaitForEndOfFrame();
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuScene",LoadSceneMode.Single);
    }

    private void ContinueScene()
    {
        _continueButton.gameObject.SetActive(false);
        _policeAnimator.SetTrigger("PlayerClick");
        _cameraAnimator.SetTrigger("PlayerClick");
        StartCoroutine(StartCreditRoll());
    }

    IEnumerator StartCreditRoll()
    {
        yield return new WaitForSeconds(3f);

        float time = 0;
        float maxTime = 2;

        while (true)
        {
            time += Time.deltaTime;
            float percent = time / maxTime;
            _canvasGroup.alpha = percent;

            if (percent >= 1)
                break;
            yield return new WaitForEndOfFrame();
        }

        //make sure alpah is fully 1
        _canvasGroup.alpha = 1;


        Vector3 endPos = new Vector3(0,1000,0);
        Vector3 startPos = new Vector3(0,-1000,0);
        maxTime = 25;
        while(true)
        {
            time += Time.deltaTime;
            float percent = time / maxTime;
            _creditTextTransform.localPosition = Vector3.Lerp(startPos, endPos, percent);

            if (percent >= 1)
                break;
            yield return new WaitForEndOfFrame();
        }

        _returnToMenu.enabled = true;

        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("MainMenuScene",LoadSceneMode.Single);
    }
}
