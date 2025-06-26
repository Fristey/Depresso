using TMPro;
using UnityEngine;

//shows the points after the day
public class PointsManager : MonoBehaviour
{
    [SerializeField] private GameObject scoreCanvans;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string aRank, bRank, cRank, dRank, sRank, fail;

    private int amountOfPoints;

    public void AddPoints(int pointsAdded)
    {
        amountOfPoints += pointsAdded;
    }


    public void CalculateScore()
    {
        scoreCanvans.SetActive(true);

        if (amountOfPoints > 20)
        {
            scoreText.text = string.Join("", dRank);
        }
        else if (amountOfPoints > 40)
        {
            scoreText.text = string.Join("", cRank);
        }
        else if (amountOfPoints > 60)
        {
            scoreText.text = string.Join("", bRank);
        }
        else if (amountOfPoints > 80)
        {
            scoreText.text = string.Join("", aRank);
        }
        else if (amountOfPoints > 100)
        {
            scoreText.text = string.Join("", sRank);
        }
        else if (amountOfPoints < 10)
        {
            scoreText.text = string.Join("", fail);
        }
    }
}
