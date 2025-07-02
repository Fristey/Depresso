using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private GameObject currencyCanvans;
    [SerializeField] private TextMeshProUGUI moneyText, pointsText;
    private int amountOfPoints = 0;

    [SerializeField] private CurrencyManager currencyManager;

    public void AddPoints(int pointsAdded)
    {
        amountOfPoints += pointsAdded;
    }
    private void Update()
    {
        pointsText.text = amountOfPoints.ToString();
        moneyText.text = currencyManager.playerCurrency.ToString();
        PlayerPrefs.SetInt("score", amountOfPoints);
        PlayerPrefs.Save();
    }

    public void ShowPoints()
    {
        currencyCanvans.SetActive(true);
    }
    public void StopShowing()
    {
        currencyCanvans.SetActive(false);
    }

}
