using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject spawnPoint;
    private float spawnInterval = 2f;
    [SerializeField] private int maxCustomers = 3; 

    public static int currentCustomerCount = 0;

    private float spawnTimer = 0f;

    private void Update()
    {
        if(currentCustomerCount < maxCustomers)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnCustomer();
                spawnTimer = 0f;
            }
        }
    }

    private void SpawnCustomer()
    {
        GameObject customer = Instantiate(customerPrefab, spawnPoint.transform.position, Quaternion.identity);
        currentCustomerCount+= 1;
    }

}
