using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering.UI;

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

    [Header("Icon")]
    [SerializeField] IconUI Icon;

    [Header("Animation For Player")]
    private AnimationHandler _animationHandler;
    public List<double> GetTimeStampsList() { return timeStamps;}

    [Header("TutorialStuff")]
    [SerializeField] bool isTutorial = false;
    [SerializeField] TutorialWorld tutorialWorld;

    //private variables
    private HeathComponent HealthComp;
    private ScoreKeeper _scoreKeeper;
    private List<Note> notes = new List<Note>();
    private List<Melanchall.DryWetMidi.MusicTheory.NoteName> notesRestriction = new List<Melanchall.DryWetMidi.MusicTheory.NoteName>();
    private List<double> timeStamps = new List<double>();
    private List<Melanchall.DryWetMidi.Interaction.Note> melanchallMidiNotes = new List<Melanchall.DryWetMidi.Interaction.Note>();
    private AudioSource _hitSoundAudioSource;
    private CoreGameDataHolder _gameDataHolder;
    private TempoMap _tempoMap;

    double timeStamp;
    double marginOfError;
    double audioTime;
    int spawnIndex = 0;
    int inputIndex = 0;
    bool _holdingNote = false;

    public bool GetIsHoldingNote() { return _holdingNote; }
    public void Start()
    {
        _animationHandler = GetComponent<AnimationHandler>();
        HealthComp = GetComponentInParent<HeathComponent>();
        _scoreKeeper = GetComponentInParent<ScoreKeeper>();
        _hitSoundAudioSource = GetComponent<AudioSource>();
        _gameDataHolder= FindObjectOfType<CoreGameDataHolder>();

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
        if(attackType == AttackType.Tap)
        {
            if (inputIndex >= 0 && inputIndex < notes.Count && notes[inputIndex] != null)
            {
                if (notes[inputIndex].GetNoteType() == AttackType.Tap)
                {
                    _animationHandler.PlayAttackAnimation(attackType);
                }
                else if(notes[inputIndex].GetNoteType() == AttackType.Hold)
                {
                    _animationHandler.PlayAttackAnimation(AttackType.Hold);
                }
            }
            else
            {
                _animationHandler.PlayAttackAnimation(attackType);
            }
        }
        else
        {
            _animationHandler.PlayAttackAnimation(attackType);
        }

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
            return;
        }

        if (AbsValueDouble(audioTime - timeStamp) < marginOfError)
        {
            _hitSoundAudioSource.Play();

            if(attackType == AttackType.SwipeUp)
            {
                if(notes[inputIndex].GetNoteType() == AttackType.SwipeUp)
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
                if (notes[inputIndex].GetNoteType() == AttackType.SwipeUp)
                {
                    return;
                }
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


    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction || note.NoteName == noteRestrictionSwipeUp)
            {
                _tempoMap = _levelAudioManager.GetMidiFile().GetTempoMap();
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, _tempoMap);
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
                var metricTimeEnd = TimeConverter.ConvertTo<MetricTimeSpan>(melanchallMidiNotes[spawnIndex].Length, _tempoMap);
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
            //Icon.HurtIcon();
            HealthComp.TakeDmg(1);
            _animationHandler.PlayHitAnimation();
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

    internal void CheckHoldingNote()
    {
        if(inputIndex-1 >= 0 && inputIndex-1 < notes.Count && notes[inputIndex-1] != null)
        {
            _holdingNote = true;
            if (AbsValueDouble(audioTime - notes[inputIndex-1].GetEndTime()) < 0.1f)
            {
                _holdingNote = false;
                _animationHandler.PlayAttackAnimation(AttackType.EndHold);
            }
        }
    }
}
