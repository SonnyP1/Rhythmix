using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System;

public class LevelAudioManager : MonoBehaviour
{
    [SerializeField] Lane[] Lanes;
    [SerializeField] float SongDelayInSecounds;

    public double GetMarginOfError()
    {
        return MarginOfError;
    }
    [SerializeField] double MarginOfError; // in seconds

    public float GetInputDelayInMillieseconds()
    {
         return InputDelayInMilliseconds;
    }
    [SerializeField] float InputDelayInMilliseconds;


    [SerializeField] string FileLoc;
    public float GetNoteTime()
    {
        return NoteTime;
    }
    [SerializeField] float NoteTime;

    public float GetNoteSpawnZ()
    {
        return NoteSpawnZ;
    }
    [SerializeField] float NoteSpawnZ;

    [SerializeField] float NoteTapZ;

    public MidiFile GetMidiFile()
    {
        return _midiFile;
    }
    private static MidiFile _midiFile;
    public float NoteDespawnY()
    {
        return NoteTapZ - (NoteSpawnZ - NoteTapZ);
    }
    AudioSource _songAudioSource;

    public bool IsSongHalfWayDone()
    {
        return _isSongHalfWayDone;
    }
    private bool _isSongHalfWayDone = false;

    private void Start()
    {
        _songAudioSource = GetComponent<AudioSource>();
        _midiFile = MidiFile.Read(LoadStreamingAssets(FileLoc));
        if(_midiFile == null)
        {
            Debug.Log("Data not assign");
            return;
        }
        else
        {
            Debug.Log("Data Assign");
        }
        GetDataFromMidi();
    }


    public void GetDataFromMidi()
    {
        var notes = _midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in Lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), SongDelayInSecounds);
    }

    public double GetAudioSourceTime()
    {
        return (double)_songAudioSource.timeSamples / _songAudioSource.clip.frequency;
    }

    public void StartSong()
    {
        _songAudioSource.Play();
    }

    private void Update()
    {
        if(_isSongHalfWayDone == false && _songAudioSource.time/(_songAudioSource.clip.length/10) >= 0.5)
        {
            _isSongHalfWayDone = true;
        }
    }
    private string LoadStreamingAssets(string fileLoc)
    {
        string results;
#if UNITY_ANDRIOD
        results = "jar:file://" + Application.dataPath + fileLoc;
        return results;
#endif
        results = Application.streamingAssetsPath + "/" + fileLoc;

        return results;
    }

}
