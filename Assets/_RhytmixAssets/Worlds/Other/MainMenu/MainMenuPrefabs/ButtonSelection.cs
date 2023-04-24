using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI BtnText;

    public void SetBtnText(string txt)
    {
        BtnText.text = txt;
    }

    public Button ReturnButton()
    {
        return GetComponent<Button>();
    }
}
