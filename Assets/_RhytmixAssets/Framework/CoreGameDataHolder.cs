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
}
