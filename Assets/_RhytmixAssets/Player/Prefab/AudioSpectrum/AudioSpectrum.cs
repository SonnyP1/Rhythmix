using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    [SerializeField] Material newMaterialChange;
    [SerializeField] GameObject _sampleGameObject;
    [SerializeField] float _maxScale;
    [SerializeField] AudioSampler _audioSampler;
    GameObject[] _sampleObjects = new GameObject[64];
    [SerializeField] ScoreKeeper _scoreKeeper;

    private void Start()
    {
        ChangeSampleObjectColor(Color.red);
        for (int i = 0; i < 64; i++)
        {
            GameObject instance = (GameObject)Instantiate(_sampleGameObject);
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
        for (int i = 0; i < 64; i++)
        {
            if (_sampleObjects != null)
            {
                _sampleObjects[i].transform.localScale = new Vector3(1, (_audioSampler.Samples[i] * _maxScale)+2, 1);
            }
        }
    }

    public void ChangeSampleObjectColor(Color color)
    {
        Color colorToApply = new Color(color.r, color.g, color.b, newMaterialChange.color.a);
        newMaterialChange.color = colorToApply;
        newMaterialChange.SetColor("_EmissionColor", colorToApply);
    }

}
