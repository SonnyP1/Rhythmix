using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] Transform[] SpawnTrans;
    [SerializeField] GameObject[] EnvironmentToSpawn;

    [SerializeField] bool HasAccentEnvironment = false;
    [SerializeField] Transform[] AccentSpawnTrans;
    [SerializeField] GameObject[] AccentEnvironmentToSpawn;
    Transform _playerTransform;
    [SerializeField] float DistanceToCheckToSpawn = 150f;


    private void Start()
    {
        _playerTransform = FindObjectOfType<BasicPlayer>().GetComponent<Transform>();
        StartCoroutine(CheckIfPlayerCloseEnough());
    }

    private void SpawnEnvironment()
    {
        foreach(Transform trans in SpawnTrans)
        {
            GameObject randEnviroment = Instantiate(PickRandomEnvironment(),trans);
        }

        if(HasAccentEnvironment)
        {
            //Random.value return a random value between 0-1 -- so 50% chance to spawn accentEnvironment
            if (Random.value > 0.5)
            {
                foreach (Transform trans in AccentSpawnTrans)
                {
                    GameObject randAccentEnviroment = Instantiate(PickRandomAccentEnvironment(), trans);
                }
            }
        }
    }
    private GameObject PickRandomEnvironment()
    {
        int rand = Random.Range(0,EnvironmentToSpawn.Length);
        return EnvironmentToSpawn[rand];
    }
    private GameObject PickRandomAccentEnvironment()
    {
        int rand = Random.Range(0, AccentEnvironmentToSpawn.Length);
        return AccentEnvironmentToSpawn[rand];
    }

    IEnumerator CheckIfPlayerCloseEnough()
    {
        while(true)
        {
            if(Vector3.Distance(_playerTransform.position,transform.position) < DistanceToCheckToSpawn)
            {
                SpawnEnvironment();
                StartCoroutine(DeleteSelfWhenBehindPlayer());
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DeleteSelfWhenBehindPlayer()
    {
        while (true)
        {
            if (Vector3.Distance(_playerTransform.position, transform.position) > DistanceToCheckToSpawn)
            {
                Destroy(gameObject);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
