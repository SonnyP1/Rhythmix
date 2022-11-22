using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("NoteType")]
    [SerializeField] AttackType NoteType;
    [SerializeField] GameObject EffectPrefab;

    [Header("Models")]
    [SerializeField] Transform EnemyModel;
    [SerializeField] GameObject[] Meshrenderers;


    [Header("Hit Time Indicator")]
    [SerializeField] RectTransform InnerRing;
    [SerializeField] RectTransform OuterRing;



    //=============================================================Getters & Setters
    public bool GetHasStartedHolding(){ return hasStartedHolding; }
    public AttackType GetNoteType(){return NoteType;}
    public bool GetIsFinishHolding() {return isFinishHolding; }
    public void SetNoteDuration(double val){noteDuration = val; }
    public void SetAssignedTime(float val) { assignedTime = val; }
    internal void SetIsHoldingNote(bool val){ isHoldingNote = val; hasStartedHolding = true;}


    //=============================================================public variables
    public bool bIsAttacking = false;
    public bool bIsOnFire = false;

    //=============================================================private variables
    private LevelAudioManager _levelAudioManager;
    private AudioSource _music;
    private Vector3 startScale;
    private Vector3 enemyStartPos;
    private bool isHoldingNote = false;
    private bool isFinishHolding = true;
    private double timeInstantiated;
    private float assignedTime;
    private float endTime;
    private double noteDuration;
    private bool hasStartedHolding = false;

    //=====================Unity Functions
    void Start()
    {
        _levelAudioManager = FindObjectOfType<LevelAudioManager>();
        timeInstantiated = _levelAudioManager.GetAudioSourceTime();
        _music = FindObjectOfType<CoreGameDataHolder>().GetMusic();
        endTime = (float)(assignedTime + noteDuration);

        if (NoteType == AttackType.Hold)
        {
            EnemyModel.localPosition = new Vector3(0, EnemyModel.localPosition.y, 32 + (float)(noteDuration * 100));
            enemyStartPos = EnemyModel.localPosition;
        }

        HideNote();
    }
    void Update()
    {        
        if(NoteType == AttackType.Hold)
        {
            HandleHoldingNoteMovement();
        }
        else
        {
            HandleNoteMovement();
        }

        OuterRingLerping();
        UnHideNote();
    }

    //======================Note Functions
    private void HandleNoteMovement()
    {
        double timeSinceInstantiated = _levelAudioManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (_levelAudioManager.GetNoteTime() * 2));

        transform.localPosition = Vector3.Lerp(Vector3.forward * _levelAudioManager.GetNoteSpawnZ(), Vector3.forward * _levelAudioManager.NoteDespawnY(), t);

        if(t > .9f)
        {
            PlayAttackAnimation();
        }
        if(t > 1)
        {
            Destroy(gameObject);
        }
    }
    private void HandleHoldingNoteMovement()
    {
        double timeSinceInstantiated = _levelAudioManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (_levelAudioManager.GetNoteTime() * 2));

        float timeUntilEnd = _music.time / endTime;
        EnemyModel.localPosition = Vector3.Lerp(enemyStartPos, new Vector3(0, EnemyModel.localPosition.y, transform.localPosition.z), timeUntilEnd);

        if (isHoldingNote)
        {
            transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,4);
            Meshrenderers[0].SetActive(false);

            if (timeUntilEnd > .98f)
            {
                PlayAttackAnimation();
            }

            if (timeUntilEnd > 1)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.forward * _levelAudioManager.GetNoteSpawnZ(), Vector3.forward * _levelAudioManager.NoteDespawnY(), t);
        }

        if (t > 1 && !isHoldingNote)
        {
            Destroy(gameObject);
        }

        GetComponent<LineRenderer>().SetPosition(0, new Vector3(transform.position.x, 0.5f, transform.position.z));
        GetComponent<LineRenderer>().SetPosition(1, new Vector3(EnemyModel.position.x, 0.6f, EnemyModel.position.z));
    }
    private void OuterRingLerping()
    {
        if (OuterRing != null)
        {
            float percentDistance = (transform.position.z - (-35)) / ((-3) - (-35));
            OuterRing.localScale = Vector3.Lerp(startScale, InnerRing.localScale, percentDistance);
        }
    }

    //=====================Visual Functions
    private void UnHideNote()
    {
        foreach (GameObject meshRender in Meshrenderers)
        {
            meshRender.SetActive(true);
        }
        OuterRing.gameObject.SetActive(true);
        InnerRing.gameObject.SetActive(true);
    }
    private void HideNote()
    {
        if (OuterRing != null)
        {
            OuterRing.gameObject.SetActive(false);
            InnerRing.gameObject.SetActive(false);
            startScale = OuterRing.localScale;
        }

        foreach (GameObject meshRender in Meshrenderers)
        {
            meshRender.SetActive(false);
        }
    }

    //======================Animation
    public void PlayAttackAnimation()
    {
        Animator animator = EnemyModel.GetComponent<Animator>();
        
        if(animator != null)
            animator.SetTrigger("Attack");
    }

    private void OnDestroy()
    {
        ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(scoreKeeper != null)
        {
            if(scoreKeeper.bOnFire)
            {
                if(EffectPrefab != null)
                {
                    GameObject effectObj = Instantiate(EffectPrefab,this.transform);
                    effectObj.transform.parent = null;
                }
            }
        }
    }
}
