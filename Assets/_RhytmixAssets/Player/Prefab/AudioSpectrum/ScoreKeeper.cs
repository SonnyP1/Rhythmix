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

        Color comboColor = Color.red;
        if(val == 0)
        {
            //combo drop
            comboMeter = 0;
            multiplier = 1; 
            StopAllCoroutines();
            StartCoroutine(ChangeVolume(.4f,false));

            //Change Colors
            comboColor = Color.red;
            audioSpectrums[0].ChangeSampleObjectColor(comboColor);

            //Turn Off OnFire Effects
            audioSpectrums[0].TurnOffFireEffect();
            audioSpectrums[1].TurnOffFireEffect();
            bOnFire = false;
        }
        else
        {
            comboMeter++;
            if(comboMeter > 10)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVolume(.5f, true)); 
                multiplier = 4;

                //Change Colors
                comboColor= Color.green;
                audioSpectrums[0].ChangeSampleObjectColor(comboColor);

                //Turn On OnFire Effects
                bOnFire = true;
                audioSpectrums[0].TurnOnFireEffect();
                audioSpectrums[1].TurnOnFireEffect();
            }
            else if(comboMeter > 5)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVolume(.48f, true));
                multiplier = 3;

                //Change Colors
                comboColor = Color.magenta;
                audioSpectrums[0].ChangeSampleObjectColor(comboColor);
            }
            else if(comboMeter > 2)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVolume(.45f, true));
                multiplier = 2;

                //Change Colors
                comboColor = Color.blue;
                audioSpectrums[0].ChangeSampleObjectColor(comboColor);
            }

            score += val * multiplier;
        }
        PlayerPrefs.SetFloat("Score",score);
        PlayerPrefs.SetFloat("Accuracy",accuracy);

        _UI.UpdateMultiplier(multiplier,comboColor);
        _UI.UpdateScore(comboColor);
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
