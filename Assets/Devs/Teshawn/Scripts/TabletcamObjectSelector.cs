using System.Collections.Generic;
using UnityEngine;

public class TabletcamObjectSelector : MonoBehaviour
{
    private CamSwapManager camSwap;
    public List<UpgradeFurniture> upgradeFurnitureList;

    private Camera tabletCam;
    public LayerMask mask;
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
                            if (!hit.collider.gameObject.GetComponent<UpgradeFurniture>().isInMenu)
                            {
                                hit.collider.gameObject.GetComponent<UpgradeFurniture>().selectMenu.SetActive(true);
                                hit.collider.gameObject.GetComponent<UpgradeFurniture>().isInMenu = true;
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            camSwap.isLookingAtTablet = false;
            foreach (var furniture in upgradeFurnitureList)
            {
                furniture.isInMenu = false;
                furniture.ExitUpgradeMenus();
            }
        }
    }
}
