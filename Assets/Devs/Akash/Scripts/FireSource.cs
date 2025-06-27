using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireSource : MonoBehaviour
{
    private float spreadDelay = 5f;
    private float spreadRadius = 1f;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject visualEffect;

    public LayerMask burnableLayer;

    private bool isSpreading = false;
    private bool isExtinguished = false;

    private float gameOverTime = 15f;
    private float elapsedTime = 0f;
    public void StartFire()
    {
        if(isExtinguished)
        {
            return;
        }

        if (!isSpreading)
        {
            isSpreading = true;
            if(visualEffect != null)
            {
                visualEffect.SetActive(true);
            }
            StartCoroutine(Spread());
        }
    }

    private void Update()
    {
        Debug.Log("Update on");
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= gameOverTime)
        {
            Debug.Log("MEOW");
            SceneManager.LoadScene("StartScene");
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

    public void ExtinguishFire()
    {
        if(isExtinguished)
        {
            return;
        }

        isExtinguished = true;
        isSpreading = false;
        StopAllCoroutines();

        if(visualEffect != null)
        {
            visualEffect.SetActive(false);
        }
        Destroy(gameObject, 1f); // Delay before destruction for visual effect
    }
}
