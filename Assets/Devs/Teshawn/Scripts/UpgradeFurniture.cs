using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeFurniture : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private TabletcamObjectSelector tabletcamObjectSelector;
    private Inventory inventory;
    [SerializeField] private Material highLight;
    [SerializeField] private TextMeshProUGUI priceTag;


    private int price = 300;

    [SerializeField] private int index;
    [SerializeField] private int prevIndex;

    public GameObject selectMenu;
    [SerializeField] private List<GameObject> furnitureObj;
    [SerializeField] private GameObject currentObject;

    void Start()
    {
        currencyManager = FindFirstObjectByType<CurrencyManager>();
        tabletcamObjectSelector = FindAnyObjectByType<TabletcamObjectSelector>();
        inventory = FindFirstObjectByType<Inventory>();

        prevIndex = index;
    }

    private void Update()
    {
        if (currentObject != null)
        {
            if (tabletcamObjectSelector.selectedFurniture != this.gameObject)
            {
                selectMenu.SetActive(false);
                Highlight(currentObject.GetComponent<MeshRenderer>(), false);
            }
            else
            {
                selectMenu.SetActive(true);
                Highlight(currentObject.GetComponent<MeshRenderer>(), true);

            }

            priceTag.text = price.ToString();

        }
    }

    private void Highlight(MeshRenderer mainMesh, bool isSelected)
    {
        List<Material> materials = new List<Material>();
        Material mainMat = null;

        if (mainMesh != null)
        {
            mainMat = mainMesh.material;
        }

        if (mainMat != null && mainMat != highLight)
        {
            materials.Add(mainMat);
        }

        if (isSelected)
        {
            materials.Add(highLight);
        }

        mainMesh.SetMaterials(materials);

    }

    public void IndexUp()
    {
        if (index == furnitureObj.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        furnitureSwap();
    }

    public void IndexDown()
    {
        if (index == 0)
        {
            index = furnitureObj.Count - 1;

        }
        else
        {
            index--;
        }
        furnitureSwap();
    }

    private void furnitureSwap()
    {
        for (int i = 0; i < furnitureObj.Count; i++)
        {
            furnitureObj[i].SetActive(false);
        }
        furnitureObj[index].SetActive(true);
        currentObject = furnitureObj[index];
    }

    public void Purchase()
    {
        if (currentObject != null)
        {
            currencyManager.playerCurrency -= price;
            inventory.furniture.Add(currentObject);
            prevIndex = index;
        }
    }

    public void Place()
    {
        for (int i = 0; i < inventory.furniture.Count; i++)
        {
            if (inventory.furniture[i].name.Equals(currentObject.name))
            {
                inventory.furniture.RemoveAt(i);
            }
            else
            {
                index = prevIndex;
            }
        }
        furnitureSwap();
        tabletcamObjectSelector.selectedFurniture = null;
    }
    public void ExitUpgradeMenus()
    {
        selectMenu.SetActive(false);
    }
}
