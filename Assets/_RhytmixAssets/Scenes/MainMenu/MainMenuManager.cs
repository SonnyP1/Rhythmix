using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject WorldSelectionPanel;
    [SerializeField] GameObject[] Worlds;
    List<GameObject> _worldList = new List<GameObject>();

    private void Start()
    {
        foreach(GameObject world in Worlds)
        {
            _worldList.Add(world);
        }
    }
    public void OnValueChangeWorldSelectionPanel(Vector2 valueChange)
    {
        //temp need to find a better way to do this
        Debug.Log(valueChange);
        if(valueChange.x > 500 &&_worldList.Count > 0)
        {
            GameObject lastObj = _worldList[0];
            lastObj.transform.parent = null;
            lastObj.transform.parent = WorldSelectionPanel.transform;
            _worldList.Remove(lastObj);
            _worldList.Add(lastObj);
        }
    }

    public void FantasyWorldBtnClick()
    {
        SceneManager.LoadScene("Lush_DarkForest_Scene",LoadSceneMode.Single);
    }
    public void CyberpunkWorldBtnClick()
    {
        SceneManager.LoadScene("Utopia_Slums_Scene",LoadSceneMode.Single);
    }
}
