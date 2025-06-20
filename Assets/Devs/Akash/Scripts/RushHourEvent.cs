using UnityEngine;

public class RushHourEvent : MonoBehaviour
{
    [SerializeField] private float rushHourDuration = 60f; // Duration of the rush hour event in seconds
    [SerializeField] private int customerIncrease = 5; // Number of additional customers during rush hour
    private float rushHourTimer;
    private bool isRushHourActive = false;
    private void Start()
    {
        rushHourTimer = rushHourDuration;
    }
    private void Update()
    {
        if (isRushHourActive)
        {
            rushHourTimer -= Time.deltaTime;
            if (rushHourTimer <= 0)
            {
                EndRushHour();
            }
        }
    }
    public void StartRushHour()
    {
        isRushHourActive = true;
        rushHourTimer = rushHourDuration;
        CustomerManager.Instance.counterStools.AddRange(new GameObject[customerIncrease]);
        Debug.Log("Rush Hour started! Increased customer count by " + customerIncrease);
    }
    private void EndRushHour()
    {
        isRushHourActive = false;
        CustomerManager.Instance.counterStools.RemoveRange(CustomerManager.Instance.counterStools.Count - customerIncrease, customerIncrease);
        Debug.Log("Rush Hour ended!");
    }
}
