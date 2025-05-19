using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private CurrencyManager currencyManager;
    [SerializeField] private int price;

    public GameObject upgradeMenu;
    void Start()
    {
        currencyManager = FindFirstObjectByType<CurrencyManager>();
    }

    void Update()
    {
       
    }

    public void UpgradeFurniture()
    {
        if(currencyManager != null)
        {
            if(currencyManager.playerCurrency > price)
            {
                //upgrade the object
                currencyManager.playerCurrency -= price;
            }
        }
    }
}
