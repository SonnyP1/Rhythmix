using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.UI;
using System;

public class Lane : MonoBehaviour
{
    [SerializeField] Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    [SerializeField] KeyCode input;
    [SerializeField] Button ButtonToPress;
    [SerializeField] GameObject notePrefab;

    [Header("Effects")]
    [SerializeField] GameObject missEffect;
    [SerializeField] GameObject EarlyEffect;
    [SerializeField] GameObject LateEffect;
    [SerializeField] GameObject PerfectEffect;

    [Header("AnimationForPlayer")]
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] bool hasMultipleAttackAnimation;
    [SerializeField][Range(1,4)] int attackAnimationCount;

    HeathComponent HealthComp;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    [SerializeField] LevelAudioManager _levelAudioManager;

    double timeStamp;
    double marginOfError;
    double audioTime;
    int spawnIndex = 0;
    int inputIndex = 0;

    public void Awake()
    {
        if(ButtonToPress != null)
        {
            Debug.Log("Added listener");
            ButtonToPress.onClick.AddListener(HitNote);
        }
    }

    private void HitNote()
    {
        if (PlayerAnimator != null)
        {
            if(hasMultipleAttackAnimation)
            {
                System.Random rand = new System.Random();
                int randomNum = rand.Next(1,attackAnimationCount+1);
                print(randomNum);
                switch(randomNum)
                {
                    case 1:
                        PlayerAnimator.SetTrigger("AttackTrigger1");
                        break;
                    case 2:
                        PlayerAnimator.SetTrigger("AttackTrigger2");
                        break;
                    case 3:
                        PlayerAnimator.SetTrigger("AttackTrigger3");
                        break;
                    case 4:
                        PlayerAnimator.SetTrigger("AttackTrigger4");
                        break;
                    default:
                        PlayerAnimator.SetTrigger("AttackTrigger1");
                        break;
                }
            }
            else
            {
                PlayerAnimator.SetTrigger("AttackTrigger1");
            }
        }

        if (AbsValueDouble(audioTime - timeStamp) < marginOfError)
        {
            Hit();
            //print($"Hit on {inputIndex} note");
            Destroy(notes[inputIndex].gameObject);
            inputIndex++;
        }
        else
        {
            //print($"Hit inaccurate on {inputIndex} note with {AbsValueDouble(audioTime - timeStamp)} delay");
        }
        if (timeStamp + marginOfError <= audioTime)
        {
            Miss();
            //print($"Missed {inputIndex} note");
            inputIndex++;
        }
    }

    public void Start()
    {
        if(_levelAudioManager != null)
        {
            _levelAudioManager = FindObjectOfType<LevelAudioManager>();
        }
        HealthComp = GetComponentInParent<HeathComponent>();
    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, _levelAudioManager.GetMidiFile().GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (_levelAudioManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - _levelAudioManager.GetNoteTime())
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            timeStamp = timeStamps[inputIndex];
            marginOfError = _levelAudioManager.GetMarginOfError();
            audioTime = _levelAudioManager.GetAudioSourceTime() - (_levelAudioManager.GetInputDelayInMillieseconds() / 1000.0);

            //Input.GetTouch
            if (Input.GetKeyDown(input))
            {
                HitNote();
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                inputIndex++;
            }
        }

    }
    private void Hit()
    {
        double accuracy = AbsValueDouble(audioTime - timeStamp);
        Debug.Log(accuracy);
        if(accuracy > 0.2f)
        {
            Instantiate(EarlyEffect, ButtonToPress.transform);
        }
        else if(accuracy < 0.05f)
        {
            Instantiate(PerfectEffect, ButtonToPress.transform);
        }
        else
        {
            Instantiate(LateEffect, ButtonToPress.transform);
        }
    }
    private void Miss()
    {
        Instantiate(missEffect,ButtonToPress.transform);
        if(PlayerAnimator != null)
        {
            PlayerAnimator.SetTrigger("HitTrigger");
        }

        if (HealthComp != null)
        {
            HealthComp.TakeDmg(1);
        }
    }

    private static double AbsValueDouble(double number)
    {
        double num = number;
        if(number < 0)
        {
            num = -1 * number;
        }
        return num;
    }
}
