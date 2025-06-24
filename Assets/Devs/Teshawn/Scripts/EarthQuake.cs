using System.Collections;
using UnityEngine;

public class EarthQuake : TempEvent
{
    public bool isShaking;
    [SerializeField] private AnimationCurve curve;
    public GameObject map;

    private void Start()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 OGpos = map.transform.localPosition;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float intensity = curve.Evaluate(elapsedTime/ duration);
            map.transform.localPosition = OGpos + Random.insideUnitSphere * intensity;
            yield return null;
        }
        transform.position = OGpos;
    }
}
