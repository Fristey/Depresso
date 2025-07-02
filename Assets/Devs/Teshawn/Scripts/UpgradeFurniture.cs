using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UpgradeFurniture : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private TabletcamObjectSelector tabletcamObjectSelector;
    private Inventory inventory;
    [SerializeField] private Material highLight;


    [SerializeField] private int price;

    public GameObject selectMenu;

    [SerializeField] private GameObject purchaseBtn,placeBtn,exitpu,exitpl;
    [Header("shop")]
    [SerializeField] private GameObject normalPu, fancyPu, cyberPu, asianPu;

    [Header("placement")]
    [SerializeField] private GameObject normalPul, fancyPl, cyberPl, asianPl;

    [Header("confirm")]
    [SerializeField] private GameObject confirmPu, denyPu, confirmPl, denyPl;

    [SerializeField] private GameObject normalObject, fancyObject, CyberObject, asianObject;

    [SerializeField] private GameObject currentObject;
    public GameObject previousObject;
    [SerializeField] private GameObject PurchaseObject;
    [SerializeField] private GameObject placedObject;

    void Start()
    {
        previousObject = normalObject;
        currencyManager = FindFirstObjectByType<CurrencyManager>();
        tabletcamObjectSelector = FindAnyObjectByType<TabletcamObjectSelector>();
        inventory = FindFirstObjectByType<Inventory>();

        normalPu.SetActive(false);
        fancyPu.SetActive(false);
        cyberPu.SetActive(false);
        asianPu.SetActive(false);
    }

    private void Update()
    {
        if(PurchaseObject != null)
        {
            confirmPu.SetActive(true);
            denyPu.SetActive(true);
        }
        else
        {
            confirmPu.SetActive(false);
            denyPu.SetActive(false);
        }

        if (placedObject != null)
        {
            confirmPl.SetActive(true);
            denyPl.SetActive(true);
        }
        else
        {
            confirmPl.SetActive(false);
            denyPl.SetActive(false);
        }

        if(tabletcamObjectSelector.selectedFurniture != this.gameObject)
        {
            selectMenu.SetActive(false);
            Highlight(previousObject.GetComponent<MeshRenderer>() , false);
        }
        else
        {
            selectMenu.SetActive(true);
            Highlight(previousObject.GetComponent<MeshRenderer>(), true);

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

    public void Purchase()
    {
        if (currencyManager.playerCurrency > price)
        {
            currencyManager.playerCurrency -= price;
            inventory.furniture.Add(PurchaseObject);
            PurchaseObject = null;

            placeBtn.SetActive(true);
            purchaseBtn.SetActive(true);
        }
        else
        {
            Debug.Log("cant buy");
        }
    }

    public void PlaceObject()
    {
        if (placedObject != null)
        {
            for(int i = 0; i < inventory.furniture.Count; i++)
            {
                if (inventory.furniture[i].name == placedObject.name)
                {
                    currentObject = placedObject;
                    if (currentObject != previousObject)
                    {
                        currentObject.SetActive(true);
                    }
                    if (previousObject != null)
                    {
                        previousObject.SetActive(false);
                        currentObject.SetActive(true);
                        previousObject = currentObject;
                        inventory.furniture.Remove(placedObject);
                    }
                    else
                    {
                        previousObject = currentObject;
                    }
                }
            }
           
            placeBtn.SetActive(true);
            purchaseBtn.SetActive(true);

            placedObject = null;
        }
    }
    public void SelectPlace()
    {
        purchaseBtn.SetActive(false);
        placeBtn.SetActive(false);

        exitpl.SetActive(true);
        fancyPl.SetActive(true);
        cyberPl.SetActive(true);
        asianPl.SetActive(true);
        normalPul.SetActive(true);
    }
    #region purchase Functions
    public void FancyPurchase()
    {
        price = 20;
        PurchaseObject = fancyObject;
        normalPu.SetActive(false);
        fancyPu.SetActive(false);
        cyberPu.SetActive(false);
        asianPu.SetActive(false);
        exitpu.SetActive(false);

    }

    public void CyberPurchase()
    {
        price = 30;
        PurchaseObject = CyberObject;
        normalPu.SetActive(false);
        fancyPu.SetActive(false);
        cyberPu.SetActive(false);
        asianPu.SetActive(false);
        exitpu.SetActive(false);

    }

    public void NormalObjectPurchase()
    {
        price = 10;
        PurchaseObject = normalObject;
        normalPu.SetActive(false);
        fancyPu.SetActive(false);
        cyberPu.SetActive(false);
        asianPu.SetActive(false);
        exitpu.SetActive(false);

    }

    public void AsianObjectPurchase()
    {
        price = 50;
        PurchaseObject = asianObject;
        normalPu.SetActive(false);
        fancyPu.SetActive(false);
        cyberPu.SetActive(false);
        asianPu.SetActive(false);
        exitpu.SetActive(false);


    }

    public void SelectShop()
    {
        normalPu.SetActive(true);
        fancyPu.SetActive(true);
        cyberPu.SetActive(true);
        asianPu.SetActive(true);
        exitpu.SetActive(true);

        purchaseBtn.SetActive(false);
        placeBtn.SetActive(false);
    }

    public void DenyPurchase()
    {
        PurchaseObject = null;
        normalPu.SetActive(true);
        fancyPu.SetActive(true);
        cyberPu.SetActive(true);
        asianPu.SetActive(true);
        exitpu.SetActive(true);
    }

    public void ExitPurchase()
    {
        normalPu.SetActive(false);
        fancyPu.SetActive(false);
        cyberPu.SetActive(false);
        asianPu.SetActive(false);
        exitpu.SetActive(false);

        purchaseBtn.SetActive(true);
        placeBtn.SetActive(true);

    }
    #endregion

    #region place Functions
    public void PlaceFancy()
    {
        placedObject = fancyObject;
        normalPul.SetActive(false);
        fancyPl.SetActive(false);
        cyberPl.SetActive(false);
        asianPl.SetActive(false);

        exitpl.SetActive(false);
    }

    public void PlaceCyber()
    {
        placedObject = CyberObject;
        fancyPl.SetActive(false);
        cyberPl.SetActive(false);
        asianPl.SetActive(false);
        normalPul.SetActive(false);

        exitpl.SetActive(false);
    }

    public void placeAsionObject()
    {
        placedObject = asianObject;
        fancyPl.SetActive(false);
        cyberPl.SetActive(false);
        asianPl.SetActive(false);
        normalPul.SetActive(false);

        exitpl.SetActive(false);
    }
    public void PlaceNormal()
    {
        placedObject = normalObject;
        fancyPl.SetActive(false);
        cyberPl.SetActive(false);
        asianPl.SetActive(false);
        normalPul.SetActive(false);

        exitpl.SetActive(false);
    }

    public void DenyPlace()
    {


        fancyPl.SetActive(true);
        cyberPl.SetActive(true);
        asianPl.SetActive(true);
        normalPul.SetActive(true);

        confirmPl.SetActive(false);
        denyPl.SetActive(false);
    }

    public void ExitPlace()
    {
        fancyPl.SetActive(false);
        cyberPl.SetActive(false);
        asianPl.SetActive(false);
        normalPul.SetActive(false);
        exitpl.SetActive(false);

        placeBtn.SetActive(true);
        purchaseBtn.SetActive(true);
    }
    #endregion

    public void ExitUpgradeMenus()
    {
        selectMenu.SetActive(false);
    }
}
