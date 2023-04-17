using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{

    [Header("Looks")]
    [SerializeField] Material SamplerMat;
    [SerializeField] GameObject SampleGameObject;
    [SerializeField] GameObject[] _fireEffects;

    [Header("Values")]
    [SerializeField] [Range(0, 512)] int SampleSpawner;
    [SerializeField] float _maxScale;
    [SerializeField] bool _useBuffer;

    [Header("Sampler")]
    [SerializeField] AudioSampler _audioSampler;

    private ScoreKeeper _scoreKeeper;
    private GameObject[] _sampleObjects = new GameObject[512];

    private void Start()
    {
        _scoreKeeper = FindObjectOfType<CoreGameDataHolder>().GetScoreKeeper();
        ChangeSampleObjectColor(Color.red);

        for (int i = 0; i < SampleSpawner; i++)
        {
            GameObject instance = (GameObject)Instantiate(SampleGameObject);
            instance.transform.position = this.transform.position;
            instance.transform.parent = this.transform;
            instance.name = "sampleObject " + i;
            //this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instance.transform.position = new Vector3(instance.transform.position.x,0,-i);
            _sampleObjects[i] = instance;
        }
    }

    private void Update()
    {
        for (int i = 0; i < SampleSpawner; i++)
        {
            if (_sampleObjects != null)
            {
                if(_useBuffer)
                {
                    _sampleObjects[i].transform.localScale = new Vector3(1, (_audioSampler.GetSampleBuffer()[i] * _maxScale)+ 2, 1);
                }
                else
                {
                    _sampleObjects[i].transform.localScale = new Vector3(1, (_audioSampler.GetSampler()[i] * _maxScale) + 2, 1);
                }
            }
        }
    }

    public void ChangeSampleObjectColor(Color color)
    {
        float factor = Mathf.Pow(2, 1.5f);

        Color colorToApply = new Color(color.r *factor, color.g*factor, color.b*factor, SamplerMat.color.a);
        SamplerMat.color = colorToApply;
        SamplerMat.SetColor("_EmissionColor", colorToApply);
    }

    public void TurnOnFireEffect()
    {
        foreach(GameObject obj in _fireEffects)
        {
            obj.SetActive(true);
        }
    }
    public void TurnOffFireEffect()
    {
        foreach (GameObject obj in _fireEffects)
        {
            obj.SetActive(false);
        }
    }

}
