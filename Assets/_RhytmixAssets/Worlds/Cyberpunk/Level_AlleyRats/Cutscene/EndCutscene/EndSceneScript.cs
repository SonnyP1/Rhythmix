using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour
{
    //animators
    public Animator _policeAnimator;
    public Animator _cameraAnimator;

    //buttons
    public Button _continueButton;
    public Button _returnToMenu;

    //other UI elements
    public RectTransform _creditTextTransform;
    public CanvasGroup _canvasGroup;
    private void Start()
    {
        _returnToMenu.enabled = false;

        _continueButton.onClick.AddListener(ContinueScene);
        _returnToMenu.onClick.AddListener(ReturnToMenu);
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

        //make sure alpah fully 1
        _canvasGroup.alpha = 1;


        Vector3 endPos = new Vector3(0,100,0);
        Vector3 startPos = new Vector3(0,-1500,0);
        maxTime = 10;
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
    }
}
