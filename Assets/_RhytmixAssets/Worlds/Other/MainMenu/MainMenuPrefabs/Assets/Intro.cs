using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    [SerializeField] CanvasGroup _UIWLOGOGroup;
    [SerializeField] VideoManager[] _players;


    private void Start()
    {
        StartCoroutine(FadeToBlack());
    }
    IEnumerator FadeToBlack()
    {
        yield return new WaitForSeconds(1f);
        float time = 0f;
        float maxTime = 2f;
        while(time <= maxTime)
        {
            time += Time.deltaTime;
            float percent = time / maxTime;

            _UIWLOGOGroup.alpha = 1.0f - percent;
            yield return new WaitForFixedUpdate();
        }

        foreach(VideoManager player in _players)
        {
            player.StartVideo();
        }

    }
}
