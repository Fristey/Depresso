using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int playerCurrency;
    [SerializeField] private TMP_Text currencyText;

    private void Awake()
    {
        currencyText.text = playerCurrency.ToString();
    }
    public void AddCurrency(int amout)
    {
        playerCurrency += amout;
        currencyText.text = playerCurrency.ToString();
    }

    public void RemoveCurrency(int amount)
    {
        playerCurrency -= amount;
        currencyText.text = playerCurrency.ToString();
    }
}
