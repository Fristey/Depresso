using System.Collections.Generic;
using UnityEngine;

public class TabletcamObjectSelector : MonoBehaviour
{
    private CamSwapManager camSwap;

    private Camera tabletCam;
    public LayerMask mask;

    public GameObject selectedFurniture;

    [SerializeField] private MeshRenderer mainRenderer;

    private void Start()
    {
        tabletCam = FindFirstObjectByType<Camera>();
        camSwap = FindFirstObjectByType<CamSwapManager>();
    }

    private void Update()
    {
        if (camSwap.isLookingAtTablet)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = tabletCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {
                    if (hit.collider.gameObject.CompareTag("Furniture"))
                    {
                        if (hit.collider.gameObject.GetComponent<UpgradeFurniture>() != null)
                        {
                            selectedFurniture = hit.collider.gameObject;
                        }
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            camSwap.isLookingAtTablet = false;
            selectedFurniture = null;
        }
    }

   
}
