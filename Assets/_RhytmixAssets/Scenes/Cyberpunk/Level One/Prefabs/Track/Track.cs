using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] Transform[] SpawnTrans;
    [SerializeField] GameObject[] EnvironmentToSpawn;
    Transform _playerTransform;
    float _distanceToCheck = 150f;


    private void Start()
    {
        _playerTransform = FindObjectOfType<BasicPlayer>().GetComponent<Transform>();
        StartCoroutine(CheckIfPlayerCloseEnough());
    }

    private void SpawnEnvironment()
    {
        foreach(Transform trans in SpawnTrans)
        {
            GameObject randEnviroment = Instantiate(PickRandomEnvionment(),trans);
        }
    }
    private GameObject PickRandomEnvionment()
    {
        int rand = Random.Range(0,EnvironmentToSpawn.Length);
        return EnvironmentToSpawn[rand];
    }
    IEnumerator CheckIfPlayerCloseEnough()
    {
        while(true)
        {
            if(Vector3.Distance(_playerTransform.position,transform.position) < _distanceToCheck)
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
            if (Vector3.Distance(_playerTransform.position, transform.position) > _distanceToCheck)
            {
                Destroy(gameObject);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
