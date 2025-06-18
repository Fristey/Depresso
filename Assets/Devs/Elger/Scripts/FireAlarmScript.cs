using System.Collections;
using UnityEngine;

public class FireAlarmScript : MonoBehaviour
{
    private float alarmDur;
    [SerializeField] private AudioSource alarmSound;
    public void StartAlarm(float time)
    {
        alarmDur = time;
        StopAllCoroutines();
        StartCoroutine(AlarmTimer());
    }

    private IEnumerator AlarmTimer()
    {
        alarmSound.Play();
        yield return new WaitForSeconds(alarmDur);
        alarmSound.Stop();
    }
}
