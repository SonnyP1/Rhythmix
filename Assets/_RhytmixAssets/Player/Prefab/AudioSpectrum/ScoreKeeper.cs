using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] AudioSpectrum[] audioSpectrums;
    public bool bOnFire = false;
    private AudioSource _levelMusic;
    private GameUIManager _UI;
    int score = 0;
    int comboMeter = 0;
    int multiplier = 1;
    float accuracy = 0;
    float notesHit = 0;
    float allNotes = 0;


    public void Start()
    {
        _levelMusic = FindObjectOfType<CoreGameDataHolder>().GetMusic();
        _UI = FindObjectOfType<GameUIManager>();
        audioSpectrums[0].TurnOffFireEffect();
        audioSpectrums[1].TurnOffFireEffect();
    }
    public void SetNoteCount(float val)
    {
        allNotes = val;
    }
    public int GetComboMeter()
    {
        return comboMeter;
    }
    public float GetAccuracy()
    {
        return accuracy;
    }
    public int GetMultiplier()
    {
        return multiplier;
    }
    public int GetScore()
    {
        return score;
    }

    public void ChangeScore(int val)
    {
        if(val > 1000)
        {
            notesHit++;
        }
        else if(val > 500)
        {
            notesHit += 0.5f;
        }

        accuracy = notesHit / allNotes;

        if(val == 0)
        {
            //combo drop
            comboMeter = 0;
            multiplier = 1; 
            audioSpectrums[0].ChangeSampleObjectColor(Color.red);
            StopAllCoroutines();
            StartCoroutine(ChangeVolume(.4f,false));
            audioSpectrums[0].TurnOffFireEffect();
            audioSpectrums[1].TurnOffFireEffect();
            bOnFire = false;
            _UI.OnFireUI(false);
        }
        else
        {
            comboMeter++;
            if(comboMeter > 10)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVolume(.5f, true)); 
                multiplier = 4;
                audioSpectrums[0].ChangeSampleObjectColor(Color.green);
                bOnFire = true;


                audioSpectrums[0].TurnOnFireEffect();
                audioSpectrums[1].TurnOnFireEffect();
                _UI.OnFireUI(true);
            }
            else if(comboMeter > 5)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVolume(.48f, true));
                multiplier = 3;
                audioSpectrums[0].ChangeSampleObjectColor(Color.magenta);
            }
            else if(comboMeter > 2)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVolume(.45f, true));
                multiplier = 2;
                audioSpectrums[0].ChangeSampleObjectColor(Color.blue);
            }

            score += val * multiplier;
        }
        PlayerPrefs.SetFloat("Score",score);
        PlayerPrefs.SetFloat("Accuracy",accuracy);

        _UI.UpdateMultiplier();
        _UI.UpdateScore();
        _UI.UpdateAccuracy();
    }

    IEnumerator ChangeVolume(float val , bool increase)
    {
        if(increase)
        {
            while(_levelMusic.volume < val)
            {
                _levelMusic.volume += 0.001f;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (_levelMusic.volume > val)
            {
                _levelMusic.volume -= 0.005f;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
