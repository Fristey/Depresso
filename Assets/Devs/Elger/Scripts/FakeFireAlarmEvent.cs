using UnityEngine;

public class FakeFireAlarmEvent : TempEvent
{
    private void Start()
    {
        Object.FindFirstObjectByType<FireAlarmScript>().StartAlarm(duration);
    }
}
