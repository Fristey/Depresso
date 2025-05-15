using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int playerCurrency;

    public void AddCurrency(int amout)
    {
        playerCurrency += amout;
    }
}
