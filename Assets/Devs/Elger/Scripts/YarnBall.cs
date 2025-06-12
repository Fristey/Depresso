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

    private void Awake()
    {
        cat = FindAnyObjectByType<CatScript>();
    }
    private void Release()
    {
        cat.StartDistraction(gameObject);
        isReleased = true;
    }

    private void FixedUpdate()
    {
        if (isReleased)
        {
            integrity -= rb.linearVelocity.magnitude * degredationSpeed;

            transform.localScale = new Vector3(integrity, integrity, integrity);

            if (transform.localScale.x <= 0)
            {
                DestroyBall();
            }
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            Release();
        }

        if (trigger)
        {
            trigger = false;
            Destroy(gameObject);
        }
    }

    private void DestroyBall()
    {
        spawner.StartRespawn();
        Destroy(gameObject);
    }

    public void StartDistraction()
    {

    }
}
