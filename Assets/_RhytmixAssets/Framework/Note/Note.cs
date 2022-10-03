using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    public float endTime;
    public double noteDuration;
    public float Speed;
    LevelAudioManager _levelAudioManager;
    [SerializeField] Transform EnemyModel;
    [SerializeField] MeshRenderer[] Meshrenderers;
    [SerializeField] AttackType NoteType;
    Vector3 enemyStartPos;
    bool isHoldingNote = false;
    bool isFinishHolding = true;
    public bool hasStartedHolding = false;
    public bool GetIsFinishHolding() {
        return isFinishHolding; }
    public AttackType GetNoteType(){
        return NoteType;}
    void Start()
    {
        _levelAudioManager = FindObjectOfType<LevelAudioManager>();
        timeInstantiated = _levelAudioManager.GetAudioSourceTime();

        if (NoteType == AttackType.Hold)
        {
            //print(noteDuration);
            endTime = (float)(assignedTime + noteDuration);
            //print("I spawn at this time " + timeInstantiated);
            //print("I end at this time " + endTime);
            EnemyModel.localPosition = new Vector3( 0,EnemyModel.localPosition.y,32 + (float)(noteDuration*100));

            enemyStartPos = EnemyModel.localPosition;

        }

        foreach (MeshRenderer meshRender in Meshrenderers)
        {
            meshRender.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = _levelAudioManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (_levelAudioManager.GetNoteTime() * 2));
        float timeUntilEnd = _levelAudioManager.audioSource.time / endTime;
        
        if(NoteType == AttackType.Hold)
        {
            EnemyModel.localPosition = Vector3.Lerp(enemyStartPos, new Vector3(0, EnemyModel.localPosition.y, transform.localPosition.z), timeUntilEnd);
        }

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
                if (isHoldingNote == true)
                {
    
                    transform.localPosition = transform.localPosition;
                    Meshrenderers[0].enabled = false;
                    if (timeUntilEnd > 1)
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
