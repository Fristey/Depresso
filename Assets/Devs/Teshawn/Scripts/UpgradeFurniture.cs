using UnityEngine;

public class UpgradeFurniture : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private TabletcamObjectSelector tabletcamObjectSelector;
    private Inventory inventory;

    [SerializeField] private int price;

    public GameObject selectMenu;
    [Header("shop")]
    public GameObject purchaseConfermMenu;
    public GameObject purchaseMenu;
    [Header("placement")]
    public GameObject placeConfermMenu;
    public GameObject placeMenu;

    [SerializeField] private GameObject normalObject, fancyObject, CyberObject;

    [SerializeField] private GameObject currentObject;
    [SerializeField] private GameObject previousObject;
    [SerializeField] private GameObject PurchaseObject;
    [SerializeField] private GameObject placedObject;

    public bool isInMenu;
    void Start()
    {
        isInMenu = false;
        previousObject = normalObject;
        currencyManager = FindFirstObjectByType<CurrencyManager>();
        tabletcamObjectSelector = FindAnyObjectByType<TabletcamObjectSelector>();
        inventory = FindFirstObjectByType<Inventory>();
        tabletcamObjectSelector.upgradeFurnitureList.Add(this);
    }

    public void Purchase()
    {
        if (currencyManager.playerCurrency > price)
        {
            currencyManager.playerCurrency -= price;
            inventory.furniture.Add(PurchaseObject);
            purchaseConfermMenu.SetActive(false);
            isInMenu = false;
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
            if (inventory.furniture.Contains(placedObject))
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
            placeConfermMenu.SetActive(false);
            isInMenu = false;
        }
    }
    public void SelectPlace()
    {
        selectMenu.SetActive(false);
        placeMenu.SetActive(true);
    }
    #region purchase Functions
    public void FancyPurchase()
    {
        price = 20;
        PurchaseObject = fancyObject;
        purchaseMenu.SetActive(false);
        purchaseConfermMenu.SetActive(true);
    }

    public void CyberPurchase()
    {
        price = 30;
        PurchaseObject = CyberObject;
        purchaseMenu.SetActive(false);
        purchaseConfermMenu.SetActive(true);
    }

    public void NormalObjectPurchase()
    {
        price = 10;
        PurchaseObject = normalObject;
        purchaseMenu.SetActive(false);
        purchaseConfermMenu.SetActive(true);
    }

    public void SelectShop()
    {
        selectMenu.SetActive(false);
        purchaseMenu.SetActive(true);
    }

    public void DenyPurchase()
    {
        purchaseConfermMenu.SetActive(false);
        purchaseMenu.SetActive(true);
    }

    public void PurchaseFurniture()
    {
        purchaseConfermMenu.SetActive(true);
        purchaseMenu.SetActive(false);
    }

    public void ExitPurchase()
    {
        purchaseMenu.SetActive(false);
        selectMenu.SetActive(true);
    }
    #endregion

    public void PlaceFancy()
    {
        placedObject = fancyObject;
        placeMenu.SetActive(false);
        placeConfermMenu.SetActive(true);
    }
    public void PlaceCyber()
    {
        placedObject = CyberObject;
        placeMenu.SetActive(false);
        placeConfermMenu.SetActive(true);
    }

    public void PlaceNormal()
    {
        placedObject = normalObject;
        placeMenu.SetActive(false);
        placeConfermMenu.SetActive(true);
    }

    public void DenyPlace()
    {
        placeConfermMenu.SetActive(false);
        placeMenu.SetActive(true);
    }

    public void ExitPlace()
    {
        placeMenu.SetActive(false);
        selectMenu.SetActive(true);
    }
}
