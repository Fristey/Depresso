using System.Collections;
using UnityEngine;

public class EarthQuake : TempEvent
{
    public bool isShaking;
    [SerializeField] private AnimationCurve curve;
    public Map map;

    private void Start()
    {
        map = FindFirstObjectByType<Map>();
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 OGpos = map.map.transform.localPosition;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float intensity = curve.Evaluate(elapsedTime/ duration);
            map.map.transform.localPosition = OGpos + Random.insideUnitSphere * intensity;
            yield return null;
        }
        transform.position = OGpos;
    }
}
