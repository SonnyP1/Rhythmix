using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreGameDataHolder : MonoBehaviour
{
    public AudioSource GetMusic()
    {
        return _music;
    }
    [SerializeField] AudioSource _music;

    public ScoreKeeper GetScoreKeeper()
    {
        return _scoreKeeper;
    }
    [SerializeField] ScoreKeeper _scoreKeeper;

    public GameUIManager GetGameUIManager()
    {
        return _UIGameManager;
    }
    [SerializeField] GameUIManager _UIGameManager;

    public string GetSongTitle()
    {
        return _songTitle;
    }
    [SerializeField] string _songTitle;

    public GameObject GetPlayerGroup()
    {
        return _playerGroup;
    }
    [SerializeField] GameObject _playerGroup;

    //Game Related Functions
    public void PauseGame()
    {
        PauseMusic();
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        ContinueMusic();
        Time.timeScale = 1f;
    }


    //Music Related Functions
    public void ContinueMusic()
    {
        _music.Play();
    }


    public void PauseMusic()
    {
        _music.Pause();
    }
}
