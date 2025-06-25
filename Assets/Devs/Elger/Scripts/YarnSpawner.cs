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

    [Header("Misc")]
    [SerializeField] private GameObject spawn;

    [SerializeField] private CatScript catScript;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject yarnCoil;

    [Header("Yarn")]
    public YarnBall yarnScript;
    public Rigidbody yarnRb;
    [SerializeField] private GameObject yarnPrefab;
    private GameObject curYarn;
    private Collider yarnCol;
    private MeshRenderer yarnMesh;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
            yarnScript.yarnCoil = yarnCoil;
            yarnMesh = curYarn.GetComponentInChildren<MeshRenderer>();
        }
        curYarn.transform.position = spawn.transform.position;
        yarnRb.isKinematic = true;

        yarnScript.StartRestoration();

        animator.SetBool("Regenerating", true);
        yarnMesh.enabled = false;
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

                animator.SetBool("Regenerating", false);
                yarnMesh.enabled = true;
                return true;
            case YarnSpawnStates.Out:
                ReturnYarn();
                return false;
        }
        return false;
    }

    public void ReturnYarn()
    {
        Debug.Log("Return yarn");

        curYarn.transform.position = spawn.transform.position;
        yarnRb.isKinematic = true;
        yarnCol.enabled = false;

        state = YarnSpawnStates.In;

        catScript.EndDistraction();

        yarnScript.StartRestoration();

        animator.SetBool("Regenerating", true);
        yarnMesh.enabled = false;
    }
}
