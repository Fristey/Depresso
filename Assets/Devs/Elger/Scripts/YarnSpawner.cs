using System;
using System.Collections;
using UnityEngine;

enum YarnSpawnStates
{
    Waiting,
    In,
    Out
}
public class YarnSpawner : MonoBehaviour
{
    [SerializeField] private YarnSpawnStates state = YarnSpawnStates.In;

    [SerializeField] private GameObject yarnPrefab;
    private GameObject curYarn;
    private Rigidbody curYarnRb;

    [SerializeField] private GameObject spawn;
    private YarnBall yarnScript;
    [SerializeField] private float YarnRespawnTime;

    [Header("Behaviour")]
    [SerializeField] private bool trigger;

    private void Start()
    {
        SpawnYarn();
    }

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            GrabYarn();
        }

        switch (state)
        {
            case YarnSpawnStates.Waiting:
                break;
            case YarnSpawnStates.In:
                break;
            case YarnSpawnStates.Out:
                break;
        }
    }

    private void ReturnYarn()
    {
        curYarnRb.isKinematic = true;
        curYarn.transform.position = spawn.transform.position;
    }

    private void ReleaseYarn()
    {
        curYarnRb.isKinematic = false;
    }

    private void SpawnYarn()
    {
        state = YarnSpawnStates.In;
        curYarn = Instantiate(yarnPrefab, spawn.transform.position, Quaternion.identity);
        curYarnRb = curYarn.GetComponent<Rigidbody>();
        yarnScript = curYarn.GetComponent<YarnBall>();

        yarnScript.spawner = this;
    }

    public void GrabYarn()
    {
        if (state == YarnSpawnStates.In)
        {
            ReleaseYarn();
            state = YarnSpawnStates.Out; 
        }
        else if (state == YarnSpawnStates.Out)
        {
            ReturnYarn();
            state = YarnSpawnStates.In;
        }
    }

    public void StartRespawn()
    {
        StartCoroutine(YarnRespawnTimer());
    }

    private IEnumerator YarnRespawnTimer()
    {
        yield return new WaitForSeconds(YarnRespawnTime);
        SpawnYarn();
    }
}
