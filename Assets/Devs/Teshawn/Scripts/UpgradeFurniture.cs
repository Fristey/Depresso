using UnityEngine;

public class UpgradeFurniture : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private TabletcamObjectSelector tabletcamObjectSelector;
    private Inventory inventory;

    [SerializeField] private int price;

    public GameObject PurchaseConfermMenu;
    public GameObject placeConfermMenu;
    public GameObject selectMenu;
    public GameObject placeMenu;
    public GameObject purchaseMenu;

    [SerializeField] private GameObject normalObject, fancyObject, CyberObject;

    [SerializeField] private GameObject currentObject;
    [SerializeField] private GameObject previousObject;
    public GameObject upgradeObject;
    void Start()
    {
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
            inventory.furniture.Add(upgradeObject);
        }
    }

    public void Upgrade()
    {
        if (upgradeObject != null)
        {
            if (inventory.furniture.Contains(upgradeObject))
            {
                currentObject = upgradeObject;
                if (currentObject != previousObject)
                {
                    currentObject.SetActive(true);
                }
                if (previousObject != null)
                {
                    previousObject.SetActive(false);
                    currentObject.SetActive(true);
                    previousObject = currentObject;
                }
                else
                {
                    previousObject = currentObject;
                }
            }
        }
    }

    public void Deny()
    {
    }

    public void SelectUpgrade()
    {
    }

    public void FancyUpgrade()
    {
        price = 20;
        upgradeObject = fancyObject;
    }

    public void CyberUpgrade()
    {
        price = 30;
        upgradeObject = CyberObject;

    }

    public void NormalObject()
    {
        price = 10;
        upgradeObject = normalObject;
    }

    public void ExitUpgrade()
    {
    }

    public void PurchaseFurniture()
    {
        purchaseMenu.SetActive(true);
    }

    public void PlaceFurniture()
    {
        placeMenu.SetActive(true);
    }
}
