using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] AudioSpectrum[] audioSpectrum;
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
        }
        else
        {
            comboMeter++;
            if(comboMeter > 10)
            {
                multiplier = 8;
                audioSpectrum[0].ChangeSampleObjectColor(Color.green);
            }
            else if(comboMeter > 5)
            {
                multiplier = 4;
                audioSpectrum[0].ChangeSampleObjectColor(Color.yellow);
            }
            else if(comboMeter > 2)
            {
                multiplier = 2;
                audioSpectrum[0].ChangeSampleObjectColor(Color.blue);
            }

            score += val * multiplier;

            print("The score is " + score + " The Multiplier is " + multiplier + " Combo Meter at " + comboMeter);
        }
    }
}
