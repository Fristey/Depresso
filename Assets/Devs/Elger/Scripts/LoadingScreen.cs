using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image LoadSymbol;
    [SerializeField] private Canvas mainUI;
    [SerializeField] private CinemachineCamera DoorCamera;

    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Animator camAnimator;
    private Camera cam;

    private bool startAni;
    private void Awake()
    {
        cam = Camera.main;
    }

    public void StartLoad()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void StartAnimation()
    {
        DoorCamera.Priority = 100;
        startAni = true;

        mainUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Vector3.Distance(cam.transform.position,transform.position) < 0.01 && startAni)
        {
            startAni = false;

            doorAnimator.SetTrigger("InstaOpen");
            camAnimator.SetTrigger("InstaZoom");
        }
    }
}
