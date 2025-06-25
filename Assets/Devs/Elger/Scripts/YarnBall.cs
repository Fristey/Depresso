using UnityEngine;
using System;
using System.Collections;

enum yarnBallStates
{
    waiting,
    grabbed,
    released
}

public class YarnBall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;

    public YarnSpawner spawner;
    public GameObject yarnCoil;

    [SerializeField] private CatScript cat;

    [SerializeField] private yarnBallStates state = yarnBallStates.waiting;

    [Header("Stats")]
    [SerializeField] private float integrity;
    [SerializeField] private float maxIntegrity;
    [SerializeField] private float minIntegrity;
    [SerializeField] private float degredationSpeed;
    [SerializeField] private float restorationSpeed;
    [SerializeField] private float baseScale;
    private float scaleChunk;

    private void Awake()
    {
        cat = FindAnyObjectByType<CatScript>();

        integrity = minIntegrity;

        baseScale = transform.localScale.x;
        scaleChunk = baseScale / maxIntegrity;
    }

    private void FixedUpdate()
    {
        if (state == yarnBallStates.released)
        {
            integrity -= rb.linearVelocity.magnitude * degredationSpeed;

            float newScale = scaleChunk * integrity;

            transform.localScale = new Vector3(newScale, newScale, newScale);

            if (integrity < minIntegrity)
            {
                spawner.SpawnYarn();
                cat.EndDistraction();
            }
        }
    }

    private void Update()
    {
        if(state == yarnBallStates.waiting && integrity < maxIntegrity)
        {
            integrity += Time.deltaTime;

            float newScale = scaleChunk * integrity;

            transform.localScale = new Vector3(newScale,newScale,newScale);

            float coilScale = 1 / maxIntegrity * integrity;
            yarnCoil.transform.localScale = new Vector3(coilScale,1,coilScale);
        }
    }

    public void StartDistraction()
    {
        state = yarnBallStates.released;
        cat.StartDistraction(gameObject);
    }

    public void Grabbed()
    {
        state = yarnBallStates.grabbed;
    }

    public void StartRestoration()
    {
        state = yarnBallStates.waiting;
    }
}
