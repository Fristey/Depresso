using System;
using System.Collections;
using UnityEngine;

enum YarnSpawnStates
{
    In,
    Out,
    Full
}
public class YarnSpawner : MonoBehaviour
{
    [SerializeField] private YarnSpawnStates state = YarnSpawnStates.In;

    [SerializeField] private GameObject yarnPrefab;

    [SerializeField] private GameObject spawn;

    private GameObject curYarn;
    public YarnBall yarnScript;
    public Rigidbody yarnRb;
    private Collider yarnCol;

    [SerializeField] private float YarnRespawnTime;

    private void Start()
    {
        SpawnYarn();
    }

    private void SpawnYarn()
    {
        state = YarnSpawnStates.In;
        curYarn = Instantiate(yarnPrefab, spawn.transform.position, Quaternion.identity);

        yarnScript = curYarn.GetComponent<YarnBall>();
        yarnRb = curYarn.GetComponent<Rigidbody>();
        curYarn.GetComponent<Collider>().enabled = true;
        yarnCol = curYarn.GetComponent<Collider>();

        yarnRb.isKinematic = true;
    }

    public void GrabYarn()
    {
        yarnRb.isKinematic = false;

        if (state == YarnSpawnStates.In)
        {
            state = YarnSpawnStates.Out; 
            yarnCol.enabled = true;
        }
        else if (state == YarnSpawnStates.Out)
        {
            curYarn.transform.position = spawn.transform.position;
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
