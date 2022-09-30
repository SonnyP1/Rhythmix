using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    public float endTime;
    public long noteDuration;
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
            noteDuration = noteDuration / (long)100;
            endTime = (float)(timeInstantiated + noteDuration);
            print("I spawn at this time " + timeInstantiated);
            print("I end at this time " + endTime);
        }

        foreach (MeshRenderer meshRender in Meshrenderers)
        {
            meshRender.enabled = false;
        }
        _levelAudioManager = FindObjectOfType<LevelAudioManager>();
        timeInstantiated = _levelAudioManager.GetAudioSourceTime();

    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = _levelAudioManager.GetAudioSourceTime() - timeInstantiated;
        float  t = (float)(timeSinceInstantiated / (_levelAudioManager.GetNoteTime() * 2));


        if (t > 1 && !isHoldingNote)
        {
            Destroy(gameObject);
        }
        else
        {
            if(!isHoldingNote)
            {
                transform.localPosition = Vector3.Lerp(Vector3.forward * _levelAudioManager.GetNoteSpawnZ(), Vector3.forward * _levelAudioManager.NoteDespawnY(), t);
                if(NoteType == AttackType.Hold)
                {
                    EnemyModel.localPosition = Vector3.Lerp(Vector3.forward * (_levelAudioManager.GetNoteSpawnZ() + endTime), transform.localPosition, t);           
                }
            }
            if (NoteType == AttackType.Hold)
            {
                if (isHoldingNote == true)
                {
                    EnemyModel.localPosition = Vector3.Lerp(Vector3.forward * _levelAudioManager.GetNoteSpawnZ(), transform.localPosition, t);           
                    transform.localPosition = transform.localPosition;
                    Meshrenderers[0].enabled = false;
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
