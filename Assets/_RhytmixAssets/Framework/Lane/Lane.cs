using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.UI;
using System;

public class Lane : MonoBehaviour
{
    [Header("Midi Files")]
    [SerializeField] Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    [SerializeField] Melanchall.DryWetMidi.MusicTheory.NoteName noteRestrictionSwipeUp;
    [SerializeField] GameObject[] notePrefab;
    [SerializeField] LevelAudioManager _levelAudioManager;

    [Header("Hit Note")]
    [SerializeField] AudioClip HitSound;

    [Header("Effects")]
    [SerializeField] Transform EffectSpawn;
    [SerializeField] Transform HitEffectSpawn;
    [SerializeField] GameObject HitEffect;
    [SerializeField] GameObject MissEffect;
    [SerializeField] GameObject BadHitEffect;
    [SerializeField] GameObject PerfectEffect;

    [Header("Animation For Player")]
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] bool hasMultipleAttackAnimation;
    [SerializeField][Range(1,4)] int attackAnimationCount;

    public List<double> GetTimeStampsList() { return timeStamps;}

    //private variables
    private HeathComponent HealthComp;
    private ScoreKeeper _scoreKeeper;
    private List<Note> notes = new List<Note>();
    private List<Melanchall.DryWetMidi.MusicTheory.NoteName> notesRestriction = new List<Melanchall.DryWetMidi.MusicTheory.NoteName>();
    private List<double> timeStamps = new List<double>();
    private List<Melanchall.DryWetMidi.Interaction.Note> melanchallMidiNotes = new List<Melanchall.DryWetMidi.Interaction.Note>();
    private AudioSource _hitSoundAudioSource;

    double timeStamp;
    double marginOfError;
    double audioTime;
    int spawnIndex = 0;
    int inputIndex = 0;
    public void Start()
    {
        HealthComp = GetComponentInParent<HeathComponent>();
        _scoreKeeper = GetComponentInParent<ScoreKeeper>();
        _hitSoundAudioSource = GetComponent<AudioSource>();

        if(_levelAudioManager != null)
        {
            _levelAudioManager = FindObjectOfType<LevelAudioManager>();
        }
        if(_hitSoundAudioSource != null)
        {
            _hitSoundAudioSource.clip = HitSound;
        }
    }

    public void HitNote(AttackType attackType)
    {
        PlayAttackAnimation(attackType);

        if(attackType == AttackType.EndHold)
        {
            if(notes.Count > 0)
            {
                if(notes[inputIndex-1] != null)
                {
                    Miss();
                    return;
                }
            }
        }

        if (AbsValueDouble(audioTime - timeStamp) < marginOfError)
        {
            _hitSoundAudioSource.Play();

            if(notes[inputIndex].GetNoteType() == AttackType.SwipeUp)
            {
                if(attackType == AttackType.SwipeUp)
                {
                    Hit();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Hit();
            }


            if (notes[inputIndex].GetNoteType() == AttackType.Hold)
            {
                if(!notes[inputIndex].GetHasStartedHolding())
                {
                    notes[inputIndex].SetIsHoldingNote(true);
                    inputIndex++;
                }
            }
            else
            {
                Destroy(notes[inputIndex].gameObject);
                inputIndex++;
            }
        }
    }

    private void PlayAttackAnimation(AttackType attackType)
    {
        if (PlayerAnimator != null)
        {
            if(!PlayerAnimator.GetBool("isDead"))
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
                    PlayerAnimator.SetTrigger("JumpAttack");
                }
            }
        }
    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction || note.NoteName == noteRestrictionSwipeUp)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, _levelAudioManager.GetMidiFile().GetTempoMap());
                timeStamps.Add( + ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f));
                melanchallMidiNotes.Add(note);
                notesRestriction.Add(note.NoteName);
            }
        }
    }
    void Update()
    {
        SpawnNotes();

        if (inputIndex < timeStamps.Count)
        {
            timeStamp = timeStamps[inputIndex];
            marginOfError = _levelAudioManager.GetMarginOfError();
            audioTime = _levelAudioManager.GetAudioSourceTime() - (_levelAudioManager.GetInputDelayInMillieseconds() / 1000.0);

            if (timeStamp + marginOfError <= audioTime && notes[inputIndex].GetHasStartedHolding() == false)
            {
                Miss();
                inputIndex++;
            }

        }

    }
    private void SpawnNotes()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (_levelAudioManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - _levelAudioManager.GetNoteTime())
            {
                GameObject noteObject = null;
                var metricTimeEnd = TimeConverter.ConvertTo<MetricTimeSpan>(melanchallMidiNotes[spawnIndex].Length, _levelAudioManager.GetMidiFile().GetTempoMap());
                if (notesRestriction[spawnIndex] == noteRestrictionSwipeUp)
                {
                    noteObject = Instantiate(notePrefab[2], transform);
                }
                else if (metricTimeEnd.Seconds > 0)
                {
                    noteObject = Instantiate(notePrefab[1], transform);
                    noteObject.GetComponent<Note>().SetNoteDuration(((double)metricTimeEnd.Minutes * 60f + metricTimeEnd.Seconds + (double)metricTimeEnd.Milliseconds / 1000f));
                }
                else
                {
                    noteObject = Instantiate(notePrefab[0], transform);
                }
                noteObject.name += spawnIndex.ToString();
                notes.Add(noteObject.GetComponent<Note>());
                noteObject.GetComponent<Note>().SetAssignedTime((float)timeStamps[spawnIndex]);
                spawnIndex++;
            }
        }
    }
    private void Hit()
    {
        double accuracy = AbsValueDouble(audioTime - timeStamp);
        if(accuracy > 0.06f)
        {
            Instantiate(BadHitEffect, EffectSpawn);
            _scoreKeeper.ChangeScore(551);
        }
        else if(accuracy < 0.05f)
        {
            Instantiate(PerfectEffect, EffectSpawn);
            _scoreKeeper.ChangeScore(1501);
        }
        else
        {
            Instantiate(BadHitEffect, EffectSpawn);
            _scoreKeeper.ChangeScore(551);
        }
        Instantiate(HitEffect,HitEffectSpawn);
    }
    private void Miss()
    {
        Instantiate(MissEffect, EffectSpawn);
        if(HealthComp != null && HealthComp.GetHealth() != 0)
        {
            HealthComp.TakeDmg(1);
            if (PlayerAnimator != null)
            {
                PlayerAnimator.SetTrigger("HitTrigger");
            }
        }

        _scoreKeeper.ChangeScore(0);
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
