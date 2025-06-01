using UnityEngine;

public class UpgradeFurniture : MonoBehaviour
{
    private CurrencyManager currencyManager;
    [SerializeField] private int price;

    public GameObject upgradeMenu;
    public GameObject confermMenu;

    public Material redUpgrade;
    public Material blueUpgrade;
    public Material greenUpgrade;

    public Material upgradeMaterial;
    void Start()
    {
        currencyManager = FindFirstObjectByType<CurrencyManager>();
    }

    public void Upgrade()
    {
        if (upgradeMaterial != null)
        {
            if (currencyManager.playerCurrency > price)
            {
                currencyManager.playerCurrency -= price;
                if (upgradeMaterial != null)
                {
                    this.gameObject.GetComponent<Renderer>().material = upgradeMaterial;
                    confermMenu.SetActive(false);
                }
            }
        }
    }

    public void Deny()
    {
        upgradeMenu.SetActive(true);
        confermMenu.SetActive(false);
    }

    public void SelectUpgrade()
    {
        upgradeMenu.SetActive(false);
        confermMenu.SetActive(true);
    }

    public void Red()
    {
        upgradeMaterial = redUpgrade;
    }

    public void Green()
    {
        upgradeMaterial = greenUpgrade;
    }

    public void Blue()
    {
        upgradeMaterial = blueUpgrade;
    }
}
