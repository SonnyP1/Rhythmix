using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    private float _trackMovementSpeed;
    public void SetTrackMovementSpeed(float newSpeed)
    {
        _trackMovementSpeed = newSpeed; ;
    }

    public float GetYSpawnTransform()
    {
        return RoadSpawnerYTransform.position.y;
    }
    [SerializeField] Transform RoadSpawnerYTransform;
    [SerializeField] MeshRenderer _roadMeshRenderer;
    [SerializeField] Transform[] SpawnTrans;
    [SerializeField] GameObject[] EnvironmentToSpawn;

    [SerializeField] bool HasAccentEnvironment = false;
    [SerializeField] Transform[] AccentSpawnTrans;
    [SerializeField] GameObject[] AccentEnvironmentToSpawn;


    public float GetMeshRenderedOfRoadSizeY()
    {
        return _roadMeshRenderer.bounds.size.y;
    }
    public float GetMeshRenderedOfRoadSizeZ()
    {
        return _roadMeshRenderer.bounds.size.z;
    }

    private void Start()
    {
        SpawnEnvironment();
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


    private void Update()
    {
        transform.Translate(0, 0, _trackMovementSpeed * Time.deltaTime);
        if (transform.position.z > Camera.main.transform.position.z+200)
        {
            Destroy(gameObject);
        }
    }

}
