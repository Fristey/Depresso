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
    YarnSpawnStates state = YarnSpawnStates.In;

    [SerializeField] private GameObject yarn;
    [SerializeField] private GameObject spawn;

    private GameObject curYarn;
    public YarnBall yarnScript;
    public Rigidbody yarnRb;

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
    }

    private void ReturnYarn()
    {
        curYarn.transform.position = spawn.transform.position;
    }

    private void SpawnYarn()
    {
        state = YarnSpawnStates.In;
        curYarn = Instantiate(yarn, spawn.transform.position, Quaternion.identity);
        yarnScript = curYarn.GetComponent<YarnBall>();
        yarnRb = curYarn.GetComponent<Rigidbody>();
        curYarn.GetComponent<Collider>().enabled = true;

        yarnRb.isKinematic = true;
    }

    public void GrabYarn()
    {
        yarnRb.isKinematic = false;

        if (state == YarnSpawnStates.In)
        {
            state = YarnSpawnStates.Out;
        }
        else if (state == YarnSpawnStates.Out)
        {
            ReturnYarn();
        }
    }

    public IEnumerator YarnRespawnTimer()
    {
        yield return new WaitForSeconds(YarnRespawnTime);
        SpawnYarn();
    }
}
