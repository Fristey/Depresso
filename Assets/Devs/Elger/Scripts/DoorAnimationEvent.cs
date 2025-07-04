using UnityEngine;

public class DoorAnimationEvent : MonoBehaviour
{
    Transform camTransform;
    private void Awake()
    {
        camTransform = FindFirstObjectByType<Camera>().transform;
    }

    private void Update()
    {
        if(Vector3.Distance(camTransform.position,transform.position) < 0.1)
        {
            GameManager.Instance.DoorAnimationFinished();
        }
    }
}
