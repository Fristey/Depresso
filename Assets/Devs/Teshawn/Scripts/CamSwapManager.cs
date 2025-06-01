using Unity.Cinemachine;
using UnityEngine;

public class CamSwapManager : MonoBehaviour
{
    public CinemachineCamera playerCam;

    [SerializeField] private bool isLookingAtTabblet;

    private void Update()
    {
        if (isLookingAtTabblet)
        {
            playerCam.Priority = 1;
        }
        else
        {
            playerCam.Priority = 3;
        }
    }
}
