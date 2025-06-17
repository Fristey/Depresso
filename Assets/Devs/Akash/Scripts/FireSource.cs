using System.Collections;
using UnityEngine;

public class FireSource : MonoBehaviour
{
    private float spreadDelay = 5f;
    private float spreadRadius = 1f;
    [SerializeField] private GameObject firePrefab;

    public LayerMask burnableLayer;

    private bool isSpreading = false;

    public void StartFire()
    {
        if (!isSpreading)
        {
            isSpreading = true;
            StartCoroutine(Spread());
        }
    }


    private IEnumerator Spread()
    {
        while (isSpreading)
        {
            yield return new WaitForSeconds(spreadDelay);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, spreadRadius, burnableLayer);
            foreach (var hitCollider in hitColliders)
            {
                FireSource existingFire = hitCollider.GetComponent<FireSource>();
                if(existingFire == null)
                {
                    Vector3 spawnPosition = hitCollider.transform.position + Vector3.up * 0.5f; // Adjust spawn position slightly above the surface
                    GameObject newFire = Instantiate(firePrefab, spawnPosition, Quaternion.identity);

                    FireSource newFireSource = newFire.GetComponent<FireSource>();
                    if(newFireSource != null)
                    {
                        newFireSource.StartFire();
                    }
                }
            }
        }
    }
}
