using System.Collections;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    [SerializeField] private bool isShaking;
    [SerializeField] private float duration = 4f;
    [SerializeField] private AnimationCurve curve;

    void Start()
    {
        
    }

    void Update()
    {
        if (isShaking)
        {
            isShaking = false;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        Vector3 OGpos = transform.position;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float intensity = curve.Evaluate(elapsedTime/ duration);
            transform.position = OGpos + Random.insideUnitSphere * intensity;
            yield return null;
        }

        transform.position = OGpos;
    }
}
