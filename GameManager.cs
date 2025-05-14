using UnityEngine;

private class Daycycle : MonoBehaviour
{
    public float dayDuration = 120f; // Duration of a day in seconds
    public float timeScale = 1f; // Speed of time passing

    private float currentTime;

    void Update()
    {
        currentTime += Time.deltaTime * timeScale;

        if (currentTime >= dayDuration)
        {
            currentTime = 0f;
            // Trigger end of day events here
        }
    }
}



public class GameManager : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
