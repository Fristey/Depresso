using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int playerCurrency;

    public TextMeshProUGUI currencyText;
    public void AddCurrency(int amout)
    {
        playerCurrency += amout;
    }

    private void Update()
    {
        if (currencyText != null)
        {

            currencyText.text = playerCurrency.ToString();
        }
    }
}
