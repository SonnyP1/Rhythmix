using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrackSpawner : MonoBehaviour
{
    [SerializeField] GameObject TrackToSpawn;
    [SerializeField] float TrackSpeed;
    [SerializeField] float TrackSizeOffset;
    float _trackSize;
    void Start()
    {
        _trackSize = TrackToSpawn.GetComponent<Track>().GetMeshRenderedOfRoad() + TrackSizeOffset;
        GenerateBeginningTiles();
    }

    void GenerateBeginningTiles()
    {
        Vector3 CameraLoc = Camera.main.transform.position;
        float distanceToCamera = CameraLoc.z - transform.position.z;
        int amountOfTilesNeeded = (int)(distanceToCamera / _trackSize) + 1;



        for (int i = 1; i <= amountOfTilesNeeded; i++)
        {
            float SpawnLocZ = transform.position.z + i * _trackSize;
            Vector3 SpawnLoc = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, SpawnLocZ);
            GameObject newTrack = Instantiate(TrackToSpawn, SpawnLoc, Quaternion.identity);
            newTrack.GetComponent<Track>().SetTrackMovementSpeed(TrackSpeed);
        }

        StartCoroutine(StartSpawnNewTrack());
    }

    IEnumerator StartSpawnNewTrack()
    {
        GameObject previousTrack = null;
        while (true)
        {
            Vector3 SpawnLoc = transform.position;

            if (previousTrack != null)
            {
                SpawnLoc = previousTrack.transform.position - new Vector3(0f, 0f, _trackSize);
            }

            GameObject newTile = Instantiate(TrackToSpawn, SpawnLoc, Quaternion.identity);
            newTile.GetComponent<Track>().SetTrackMovementSpeed(TrackSpeed);

            previousTrack = newTile;
            float timeToGeneratorTheNextTile = _trackSize / TrackSpeed;
            yield return new WaitForSeconds(timeToGeneratorTheNextTile);
        }
    }
}
