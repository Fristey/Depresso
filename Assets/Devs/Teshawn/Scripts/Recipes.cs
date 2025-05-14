using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredientes", menuName = "Scriptable Objects/Recipes")]
public class Recipes : ScriptableObject
{
    public List<Ingredientes> requiredIngredientes = new List<Ingredientes>();

   // public enum Size { none, small, medium, large }

    //public Size drinkSize;
    public string nameOfDrink;
    public GameObject drink;
}
