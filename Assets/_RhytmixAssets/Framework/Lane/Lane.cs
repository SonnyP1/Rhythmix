using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.UI;
using System;

public class Lane : MonoBehaviour
{
    [SerializeField] Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    [SerializeField] GameObject[] notePrefab;

    [Header("Effects")]
    [SerializeField] Transform EffectSpawn;
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
    public List<Melanchall.DryWetMidi.Interaction.Note> melanchallMidiNotes = new List<Melanchall.DryWetMidi.Interaction.Note>();
    [SerializeField] LevelAudioManager _levelAudioManager;

    double timeStamp;
    double marginOfError;
    double audioTime;
    int spawnIndex = 0;
    int inputIndex = 0;


    public void HitNote(AttackType attackType)
    {
        print(attackType);
        PlayAttackAnimation(attackType);
        if (AbsValueDouble(audioTime - timeStamp) < marginOfError && notes[inputIndex].GetNoteType() == attackType)
        {
            Hit();
            if(notes[inputIndex].GetNoteType() == AttackType.Hold)
            {
                notes[inputIndex].SetIsHoldingNote(true);
            }
            else
            {
                Destroy(notes[inputIndex].gameObject);
            }
            inputIndex++;
        }
        else
        {
            //print($"Hit inaccurate on {inputIndex} note with {AbsValueDouble(audioTime - timeStamp)} delay");
        }

        if (timeStamp + marginOfError <= audioTime && attackType != AttackType.Hold)
        {
            Miss();
            inputIndex++;
            //print($"Missed {inputIndex} note");
        }
    }

    private void PlayAttackAnimation(AttackType attackType)
    {
        if (PlayerAnimator != null)
        {
            if(attackType == AttackType.Tap)
            {
                if (hasMultipleAttackAnimation)
                {
                    System.Random rand = new System.Random();
                    int randomNum = rand.Next(1, attackAnimationCount + 1);
                    //print(randomNum);
                    switch (randomNum)
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
            else if(attackType == AttackType.SwipeUp)
            {
                PlayerAnimator.SetLayerWeight(2,1);
            }
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
                melanchallMidiNotes.Add(note);
            }
        }
    }
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (_levelAudioManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - _levelAudioManager.GetNoteTime())
            {
                GameObject noteObject = null;
                if (melanchallMidiNotes[spawnIndex].Length > 32)
                {
                    noteObject = Instantiate(notePrefab[1], transform);
                }
                else
                {
                    noteObject = Instantiate(notePrefab[0], transform);
                }


                notes.Add(noteObject.GetComponent<Note>());
                noteObject.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            timeStamp = timeStamps[inputIndex];
            marginOfError = _levelAudioManager.GetMarginOfError();
            audioTime = _levelAudioManager.GetAudioSourceTime() - (_levelAudioManager.GetInputDelayInMillieseconds() / 1000.0);

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
        if(accuracy > 0.06f)
        {
            //Debug.Log("Early");
            Instantiate(EarlyEffect, EffectSpawn);
        }
        else if(accuracy < 0.05f)
        {
            //Debug.Log("Perfect");
            Instantiate(PerfectEffect, EffectSpawn);
        }
        else
        {
            //Debug.Log("Late");
            Instantiate(LateEffect, EffectSpawn);
        }
    }
    private void Miss()
    {
        Instantiate(missEffect, EffectSpawn);
        if(HealthComp != null && HealthComp.GetHealth() != 0)
        {
            HealthComp.TakeDmg(1);
            if (PlayerAnimator != null)
            {
                PlayerAnimator.SetTrigger("HitTrigger");
            }
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
