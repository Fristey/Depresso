using System.Collections.Generic;
using UnityEngine;

public enum Size { none, small, medium, large }
[CreateAssetMenu(fileName = "Ingredientes", menuName = "Scriptable Objects/Recipes")]
public class Recipes : ScriptableObject
{
    public List<Ingredientes> requiredIngredientes = new List<Ingredientes>();

    public Size drinkSize;
    public string nameOfDrink;
    public GameObject drink;
}
