using System;
using System.Collections;
using UnityEngine;

enum YarnSpawnStates
{
    In,
    Out,
    Ready
}
public class YarnSpawner : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] private YarnSpawnStates state = YarnSpawnStates.In;

    [Header("Perm Refrences")]
    [SerializeField] private GameObject yarnPrefab;

    [SerializeField] private GameObject spawn;

    [SerializeField] private CatScript catScript;

    [Header("Temp Refrences")]
    public YarnBall yarnScript;
    public Rigidbody yarnRb;
    private GameObject curYarn;
    private Collider yarnCol;

    private void Start()
    {
        SpawnYarn();
    }

    public void SpawnYarn()
    {
        state = YarnSpawnStates.In;
        if (curYarn == null)
        {
            curYarn = Instantiate(yarnPrefab);

            yarnScript = curYarn.GetComponent<YarnBall>();
            yarnRb = curYarn.GetComponent<Rigidbody>();
            curYarn.GetComponent<Collider>().enabled = true;
            yarnCol = curYarn.GetComponent<Collider>();
            yarnScript.spawner = this;
        }
        curYarn.transform.position = spawn.transform.position;
        yarnRb.isKinematic = true;

        yarnScript.StartRestoration();
    }

    public bool GrabYarn()
    {
        switch (state)
        {
            case YarnSpawnStates.In:
                state = YarnSpawnStates.Out;
                yarnCol.enabled = true;
                yarnScript.Grabbed();
                yarnRb.isKinematic = false;
                return true;
            case YarnSpawnStates.Out:
                ReturnYarn();
                return false;
        }
        return false;
    }

    public void ReturnYarn()
    {
        curYarn.transform.position = spawn.transform.position;
        yarnRb.isKinematic = true;
        yarnCol.enabled = false;

        state = YarnSpawnStates.In;

        catScript.EndDistraction();

        yarnScript.StartRestoration();
    }
}
