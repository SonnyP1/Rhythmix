using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System;
using UnityEngine.Networking;

public class LevelAudioManager : MonoBehaviour
{
    [Header("Notes Values")]
    [SerializeField] float NoteTime;
    [SerializeField] float NoteSpawnZ;
    [SerializeField] float NoteTapZ;

    [Header("Player Inputs")]
    [SerializeField] float InputDelayInMilliseconds;
    [SerializeField] double MarginOfError; // in seconds

    [Header("Other")]
    [SerializeField] float SongDelayInSecounds;
    [SerializeField] string FileLoc;
    [SerializeField] Lane[] Lanes;



    //============================Private Variables==================
    private static MidiFile _midiFile;
    private AudioSource _songAudioSource;
    private ScoreKeeper _scoreKeeper;
    private bool _isSongHalfWayDone = false;

    //=============================Getters=========================
    public float GetNoteTime()
    {
        return NoteTime;
    }
    public float GetNoteSpawnZ()
    {
        return NoteSpawnZ;
    }
    public double GetMarginOfError()
    {
        return MarginOfError;
    }
    public MidiFile GetMidiFile()
    {
        return _midiFile;
    }
    public bool IsSongHalfWayDone()
    {
        return _isSongHalfWayDone;
    }
    public float NoteDespawnY()
    {
        return NoteTapZ - (NoteSpawnZ - NoteTapZ);
    }
    public float GetInputDelayInMillieseconds()
    {
         return InputDelayInMilliseconds;
    }

    //=====================Unity Functions=================
    private void Start()
    {
        CoreGameDataHolder data = FindObjectOfType<CoreGameDataHolder>();
        _songAudioSource = data.GetMusic();
        _scoreKeeper = data.GetScoreKeeper();
        StartCoroutine(WaitToStartGame());
    }
    private void Update()
    {
        if(_isSongHalfWayDone == false && _songAudioSource.time/(_songAudioSource.clip.length/10) >= 0.5)
        {
            _isSongHalfWayDone = true;
        }
    }


    //========================Custom Functions===================
    public void GetDataFromMidi()
    {
        var notes = _midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);
        _scoreKeeper.SetNoteCount(array.Length);

        foreach (var lane in Lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), SongDelayInSecounds);
    }
    public double GetAudioSourceTime()
    {
        return _songAudioSource.time;
    }
    public void StartSong()
    {
        _songAudioSource.Play();
    }
    private string LoadStreamingAssets(string fileLoc)
    {
        string results;
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(ReadFromWebsite(fileLoc));
            return null;
        }
        results = Application.streamingAssetsPath + "/" + fileLoc;

        return results;
    }



    //====================IEnumerators============================
    private IEnumerator ReadFromWebsite(string fileLoc)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLoc))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    _midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }
    IEnumerator WaitToStartGame()
    {
        yield return new WaitForSeconds(5);
        string readFile = LoadStreamingAssets(FileLoc);
        if (readFile != null)
        {
            _midiFile = MidiFile.Read(readFile);
            GetDataFromMidi();
        }

    }

}
