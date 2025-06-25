using System.Runtime.CompilerServices;
using UnityEngine;

public class RushHourEvent : TempEvent
{
    public float rushHourSpawnTime = 0.5f;
    [SerializeField] private int maxCustomersDuringRushHour = 7; // Maximum customers during rush hour
    private int originalMaxCustomers; // Original maximum customers   

    private float originalSpawnTime;

    private void Start()
    {
        duration = 10f; // Set the duration of the rush hour event
        if (CustomerSpawner.Instance != null)
        {
            originalSpawnTime = CustomerSpawner.Instance.spawnInterval;
            originalMaxCustomers = CustomerSpawner.Instance.maxCustomers;
            CustomerSpawner.Instance.SetSpawnSettings(rushHourSpawnTime, maxCustomersDuringRushHour); // Increase max customers to 10 during rush hour
        }

        Invoke(nameof(EndRushHour), duration);

        Debug.Log("Rush Hour Event Started! Customers will spawn more frequently.");
    }

    private void EndRushHour()
    {
        Debug.Log("Rush Hour Event Ended! Customers will return to normal spawning.");
        if (CustomerSpawner.Instance != null)
        {
            CustomerSpawner.Instance.SetSpawnSettings(originalSpawnTime, originalMaxCustomers); // Reset to original settings
        }
        Destroy(gameObject); // Destroy the event object after it ends
     
    }

  private void OnDestroy()
    {
        if(CustomerSpawner.Instance != null)
        {
            CustomerSpawner.Instance.SetSpawnSettings(originalSpawnTime, originalMaxCustomers); // Reset to original settings
        }
    }
}
