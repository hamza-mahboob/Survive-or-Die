using System.IO;
using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    private Transform[] spawnPoints, objectiveSpawnPoints;
    public GameObject objectiveSpawnPointsParent;

    //singleton
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    private void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        objectiveSpawnPoints = objectiveSpawnPointsParent.GetComponentsInChildren<Transform>();

        SpawnObjectives();
    }

    public Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    Transform GetRandomObjectiveSpawnPoint()
    {
        return objectiveSpawnPoints[Random.Range(0, objectiveSpawnPoints.Length)];
    }

    void SpawnObjectives()
    {
        Transform objectiveSpawnPosition = GetRandomObjectiveSpawnPoint();
        //instantiate objective prefab and add to list of objectives in game manager
        GameManager.instance.AddObjectivePhotonPrefab(PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Objective"), objectiveSpawnPosition.position,
            objectiveSpawnPosition.rotation));
    }
}