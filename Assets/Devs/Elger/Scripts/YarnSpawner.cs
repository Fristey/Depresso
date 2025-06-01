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

    [SerializeField] private GameObject yarn;
    [SerializeField] private GameObject spawn;
    private YarnBall yarnScript;
    [SerializeField] private float YarnRespawnTime;

    [Header("Behaviour")]
    [SerializeField] private bool trigger;

    private void Awake()
    {

    }

    private void Start()
    {
        SpawnYarn();
    }

    private void Update()
    {
        if (trigger)
        {
            GrabYarn();
            trigger = false;
        }
    }

    private void ReturnYarn()
    {
        yarn.transform.position = spawn.transform.position;
        yarnScript.Release();
    }

    private void SpawnYarn()
    {
        state = YarnSpawnStates.In;
        GameObject go = Instantiate(yarn, spawn.transform.position, Quaternion.identity);
        yarnScript = go.GetComponent<YarnBall>();
    }

    public void GrabYarn()
    {
        if (state == YarnSpawnStates.In)
        {
            yarnScript.Release();
            state = YarnSpawnStates.Out;
        }
        else if (state == YarnSpawnStates.Out)
        {
            ReturnYarn();
            state = YarnSpawnStates.In;
        }
    }

    public IEnumerator YarnRespawnTimer()
    {
        yield return new WaitForSeconds(YarnRespawnTime);
        SpawnYarn();
    }
}
