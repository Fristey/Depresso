using Unity.Cinemachine;
using UnityEngine;

public class CamSwapManager : MonoBehaviour
{
    public CinemachineCamera playerCam;
    private PlayerMovement movement;
    private GrabCup grabcup;
    private LookAround lookAround;

    public bool isLookingAtTabblet;

    private void Start()
    {
        movement = FindFirstObjectByType<PlayerMovement>();
        grabcup = FindFirstObjectByType<GrabCup>();
        lookAround = FindFirstObjectByType<LookAround>();
    }

    private void Update()
    {
        if (isLookingAtTabblet)
        {

            playerCam.Priority = 1;
            movement.enabled = false;
            grabcup.enabled = false;
            lookAround.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            playerCam.Priority = 3;
            movement.enabled = true;
            grabcup.enabled = true;
            lookAround.enabled = true;
        }
    }
}
