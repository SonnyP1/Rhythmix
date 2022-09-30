using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] AudioSpectrum[] audioSpectrum;
    [SerializeField] AudioSource _levelMusic;
    int score = 0;
    int comboMeter = 0;
    int multiplier = 1;

    public int GetComboMeter()
    {
        return comboMeter;
    }

    public void ChangeScore(int val)
    {
        if(val == 0)
        {
            //combo drop
            comboMeter = 0;
            audioSpectrum[0].ChangeSampleObjectColor(Color.red);
            StopAllCoroutines();
            StartCoroutine(ChangeVolume(.2f,false));
        }
        else
        {
            comboMeter++;
            if(comboMeter > 10)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVolume(.5f, true)); 
                multiplier = 8;
                audioSpectrum[0].ChangeSampleObjectColor(Color.green);
            }
            else if(comboMeter > 5)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVolume(.45f, true));
                multiplier = 4;
                audioSpectrum[0].ChangeSampleObjectColor(Color.yellow);
            }
            else if(comboMeter > 2)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVolume(.4f, true));
                multiplier = 2;
                audioSpectrum[0].ChangeSampleObjectColor(Color.blue);
            }

            score += val * multiplier;

            //print("The score is " + score + " The Multiplier is " + multiplier + " Combo Meter at " + comboMeter);
        }
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
