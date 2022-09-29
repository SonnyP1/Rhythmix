using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    public float endTime;
    LevelAudioManager _levelAudioManager;
    [SerializeField] Transform EnemyModel;
    [SerializeField] MeshRenderer[] Meshrenderers;
    [SerializeField] AttackType NoteType;
    bool isHoldingNote = false;
    bool isFinishHolding = true;
    public bool hasStartedHolding = false;
    public bool GetIsFinishHolding() {
        return isFinishHolding; }
    public AttackType GetNoteType(){
        return NoteType;}
    void Start()
    {
        if(NoteType == AttackType.Hold)
        {
            //do work
            EnemyModel.localPosition = new Vector3(0, 2, 10);

        }

        foreach (MeshRenderer meshRender in Meshrenderers)
        {
            meshRender.enabled = false;
        }
        _levelAudioManager = FindObjectOfType<LevelAudioManager>();
        //Debug.Log("Note Started");
        timeInstantiated = _levelAudioManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = _levelAudioManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (_levelAudioManager.GetNoteTime() * 2));


        if (t > 1 && !isHoldingNote)
        {
            Destroy(gameObject);
        }
        else
        {
            if(!isHoldingNote)
            {
                transform.localPosition = Vector3.Lerp(Vector3.forward * _levelAudioManager.GetNoteSpawnZ(), Vector3.forward * _levelAudioManager.NoteDespawnY(), t);
            }
            if (NoteType == AttackType.Hold)
            {
                EnemyModel.localPosition = Vector3.Lerp(new Vector3(0, EnemyModel.localPosition.y, 1 * _levelAudioManager.GetNoteSpawnZ()), new Vector3(0, EnemyModel.localPosition.y, transform.localPosition.z), t);
                if (isHoldingNote == true)
                {
                    print("HOLD IN PLACE");
                    Meshrenderers[0].enabled = false;
                    transform.localPosition = transform.localPosition;
                    if (t > 1)
                    {
                        Destroy(gameObject);
                    }
                }

                GetComponent<LineRenderer>().SetPosition(0, new Vector3(transform.position.x, 0.5f, transform.position.z));
                GetComponent<LineRenderer>().SetPosition(1, new Vector3(EnemyModel.position.x, 0.6f, EnemyModel.position.z));
            }

            if(!isHoldingNote)
            {
                foreach (MeshRenderer meshRender in Meshrenderers)
                {
                    meshRender.enabled = true;
                }
            }
        }
    }

    internal void SetIsHoldingNote(bool val)
    {
        isHoldingNote = val;
        hasStartedHolding = true;
    }
}
