using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrackSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] TrackToSpawn;
    [SerializeField] float TrackSpeed;
    [SerializeField] float TrackSizeOffsetZ;
    [SerializeField] LevelAudioManager LevelAudioManager;
    float _trackSize;
    [SerializeField] bool HasTransitionTrack;
    [SerializeField] float TrackSizeYOffset;
    [SerializeField] GameObject TransitionTrackToSpawn;
    [SerializeField] GameObject TrackToSpawnAfterTransition;
    bool _isTrackTransitionAlreadySpawn = false;

    void Start()
    {
        _trackSize = TrackToSpawn[0].GetComponent<Track>().GetMeshRenderedOfRoadSizeZ() + TrackSizeOffsetZ;
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
            GameObject newTrack = Instantiate(TrackToSpawn[0], SpawnLoc, Quaternion.identity);

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

            GameObject newTrack = null;

            switch (LevelAudioManager.IsSongHalfWayDone() && HasTransitionTrack)
            {
                case false:
                    newTrack = Instantiate(ChooseRandomTrackToSpawn(), SpawnLoc, Quaternion.identity);
                    break;
                case true:
                    if(HasTransitionTrack)
                    {
                        if(!_isTrackTransitionAlreadySpawn)
                        {
                            _isTrackTransitionAlreadySpawn = true;
                            newTrack = Instantiate(TransitionTrackToSpawn, SpawnLoc, Quaternion.identity);
                            break;
                        }
                        SpawnLoc = previousTrack.transform.position - new Vector3(0f,
                            TransitionTrackToSpawn.GetComponent<Track>().GetMeshRenderedOfRoadSizeY() + TrackSizeYOffset + previousTrack.transform.position.y, 
                            previousTrack.GetComponent<Track>().GetMeshRenderedOfRoadSizeZ()+ TrackSizeOffsetZ);
                    }
                    newTrack = Instantiate(TrackToSpawnAfterTransition, SpawnLoc, Quaternion.identity);
                    break;
            }


            newTrack.GetComponent<Track>().SetTrackMovementSpeed(TrackSpeed);

            previousTrack = newTrack;
            float timeToGeneratorTheNextTile = _trackSize / TrackSpeed;
            yield return new WaitForSeconds(timeToGeneratorTheNextTile);
        }
    }

    private GameObject ChooseRandomTrackToSpawn()
    {
        int rand = Random.Range(0, TrackToSpawn.Length);
        return TrackToSpawn[rand];
    }
}
