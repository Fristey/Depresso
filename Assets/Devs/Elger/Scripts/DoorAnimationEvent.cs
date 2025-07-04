using UnityEngine;

public class DoorAnimationEvent : MonoBehaviour
{
    Transform camTransform;
    CamSwapManager swapManager;
    private void Awake()
    {
        camTransform = FindFirstObjectByType<Camera>().transform;
        swapManager = FindFirstObjectByType<CamSwapManager>();
    }

    private void Update()
    {
        if(Vector3.Distance(camTransform.position,transform.position) < 0.1 && swapManager.isLookingAtDoor)
        {
            GameManager.Instance.DoorAnimationFinished();
        }
    }
}
