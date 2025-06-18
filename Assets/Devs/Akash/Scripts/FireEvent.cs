using UnityEngine;
using System.Collections.Generic;
public class FireEvent : TempEvent
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] private Vector3 center;
    [SerializeField] private float spawnRadius = 2f;

    private void Start()
    {
        TriggerFireEvent();
    }


    public void TriggerFireEvent()
    {

        Vector3 randomPos = center + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));

        GameObject fire = Instantiate(firePrefab, randomPos + Vector3.up * 0.5f, Quaternion.identity);
        FireSource fireSource = fire.GetComponent<FireSource>();
        if (fireSource != null)
        {
            fireSource.StartFire();
        }
    }
}

