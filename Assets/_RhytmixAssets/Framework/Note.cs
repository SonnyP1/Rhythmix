using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    LevelAudioManager _levelAudioManager;
    void Start()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        _levelAudioManager = FindObjectOfType<LevelAudioManager>();
        Debug.Log("Note Started");
        timeInstantiated = _levelAudioManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = _levelAudioManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (_levelAudioManager.GetNoteTime() * 2));


        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.forward * _levelAudioManager.GetNoteSpawnZ(), Vector3.forward * _levelAudioManager.NoteDespawnY(), t);
            GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }
}
