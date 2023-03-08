using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject[] children;

    private void Start()
    {
        HideChildren();
    }
    public void ShowChildren()
    {
        foreach(GameObject obj in children)
        {
            obj.SetActive(true);
        }
    }
    public void HideChildren()
    {
        foreach (GameObject obj in children)
        {
            obj.SetActive(false);
        }
    }
}
