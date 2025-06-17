using UnityEngine;
using System.Collections.Generic;
public class FireEvent : TempEvent
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] private Vector3 center;
    [SerializeField] private float radius = 5f;


    private void Start()
    {
        TriggerFireEvent();
    }
    public void TriggerFireEvent()
    {
        /*        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
                List<FireSource> fireSources = new List<FireSource>();
                foreach (var hitCollider in hitColliders)
                {
                    FireSource fireSource = hitCollider.GetComponent<FireSource>();
                    if (fireSource != null && !fireSources.Contains(fireSource))
                    {
                        fireSources.Add(fireSource);
                    }
                }
                foreach (var fireSource in fireSources)
                {
                    Vector3 spawnPosition = fireSource.transform.position + Vector3.up * 0.5f; // Adjust spawn position slightly above the surface
                    GameObject newFire = Instantiate(firePrefab, spawnPosition, Quaternion.identity);
                    FireSource newFireSource = newFire.GetComponent<FireSource>();
                    if (newFireSource != null)
                    {
                        newFireSource.StartFire();
                    }
                }*/

        Vector3 randomPos = center + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));

        GameObject fire = Instantiate(firePrefab, randomPos + Vector3.up * 0.5f, Quaternion.identity);
        FireSource fireSource = fire.GetComponent<FireSource>();
        if (fireSource != null)
        {
            fireSource.StartFire();
        }
    }
}

