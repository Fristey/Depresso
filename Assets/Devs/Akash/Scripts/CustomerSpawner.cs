using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;

    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject spawnPoint;
    public float spawnInterval = 2f;
    public int maxCustomers = 3; 

    public  int currentCustomerCount = 0;

    private float spawnTimer = 0f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (!GameManager.Instance.hasDayStarted)
        {
            return;
        }

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

    public void SetSpawnSettings(float interval, int max)
    {
        spawnInterval = interval;
        maxCustomers = max;
    }

}
