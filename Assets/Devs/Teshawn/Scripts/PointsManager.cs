using UnityEngine;

//shows the points after the day
public class PointsManager : MonoBehaviour
{
    private int amountOfPoints;

    public void AddPoints(int pointsAdded)
    {
        amountOfPoints += pointsAdded;
    }
}
