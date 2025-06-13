using UnityEngine;
using System;
using System.Collections;

public class YarnBall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;

    [SerializeField] private bool trigger = false;
    public YarnSpawner spawner;

    [SerializeField] private CatScript cat;

    private bool isReleased = false;

    [Header("Stats")]
    [SerializeField] private float integrity;
    [SerializeField] private float degredationSpeed;
    private float baseScale;
    private float scaleChunk;

    private void Awake()
    {
        cat = FindAnyObjectByType<CatScript>();
        baseScale = transform.localScale.x;
        scaleChunk = baseScale / integrity;
    }

    private void FixedUpdate()
    {
        if (isReleased)
        {
            integrity -= rb.linearVelocity.magnitude * degredationSpeed;

            float newScale = scaleChunk * integrity;

            transform.localScale = new Vector3(newScale, newScale, newScale);

            if (transform.localScale.x <= 0)
            {
                DestroyBall();
            }
        }
    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }

    public void StartDistraction()
    {
        isReleased = true;
        cat.StartDistraction(gameObject);
    }
}
