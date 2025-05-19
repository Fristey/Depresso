using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSelector : MonoBehaviour
{
    private GraphicRaycaster raycaster;
    private PointerEventData pointer;
    private EventSystem eventSystem;

    private void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointer = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointer, results);

            for (int i = 0; i < results.Count; i++)
            {
                GameObject castObject = results[i].gameObject;
                Button upgradeButton = castObject.GetComponent<Button>();
                if (upgradeButton != null)
                {
                    upgradeButton.onClick.Invoke();
                    break;
                }
            }
        }
    }
}
