using Unity.Cinemachine;
using UnityEngine;

public class CamSwapManager : MonoBehaviour
{
    public CinemachineCamera tabletCam;
    public CinemachineCamera bookCam;
    private PlayerMovement movement;
    private GrabCup grabcup;
    private LookAround lookAround;

    public GameObject crosshair;
    public GameObject ceiling;
    public bool isLookingAtTablet;
    public bool isLookingAtBook;

    private void Start()
    {
        movement = FindFirstObjectByType<PlayerMovement>();
        grabcup = FindFirstObjectByType<GrabCup>();
        lookAround = FindFirstObjectByType<LookAround>();
    }

    private void Update()
    {
        if (isLookingAtTablet)
        {
            tabletCam.Priority = 10;

            movement.enabled = false;
            grabcup.enabled = false;
            lookAround.enabled = false;
            crosshair.SetActive(false);
            ceiling.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if (tabletCam != null)
            {
                tabletCam.Priority = 1;
                movement.enabled = true;
                grabcup.enabled = true;
                lookAround.enabled = true;
                ceiling.SetActive(true);
                crosshair.SetActive(true);
            }
        }

        if (isLookingAtBook)
        {
            bookCam.Priority = 10;
            movement.enabled = false;
            grabcup.enabled = false;
            lookAround.enabled = false;
            crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            bookCam.Priority = 1;
            movement.enabled = true;
            grabcup.enabled = true;
            lookAround.enabled = true;
            crosshair.SetActive(true);
        }
    }
}
