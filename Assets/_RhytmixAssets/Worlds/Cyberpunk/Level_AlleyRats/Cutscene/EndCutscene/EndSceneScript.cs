using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour
{
    public Animator _policeAnimator;
    public Animator _cameraAnimator;
    public Button _continueButton;

    private void Start()
    {
        _continueButton.onClick.AddListener(ContinueScene);
    }


    private void ContinueScene()
    {
        _policeAnimator.SetTrigger("PlayerClick");
        _cameraAnimator.SetTrigger("PlayerClick");
    }
}
