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
    YarnSpawnStates state = YarnSpawnStates.In;

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
