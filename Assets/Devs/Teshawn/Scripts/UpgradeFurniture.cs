using UnityEngine;

public class UpgradeFurniture : MonoBehaviour
{
    private CurrencyManager currencyManager;
    [SerializeField] private int price;

    public GameObject upgradeMenu;
    public GameObject confermMenu;

    [SerializeField] private GameObject normalObject, fancyObject;

    public GameObject upgradeObject;
    void Start()
    {
        currencyManager = FindFirstObjectByType<CurrencyManager>();
    }

    public void Upgrade()
    {
        if (upgradeObject != null)
        {
            if (currencyManager.playerCurrency > price)
            {
                currencyManager.playerCurrency -= price;
                if (upgradeObject != null)
                {
                    upgradeObject.SetActive(true);
                    normalObject.SetActive(false);
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

    public void FancyChairUpgrade()
    {
        upgradeObject = fancyObject;
    }
}
