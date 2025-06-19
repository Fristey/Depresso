using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredientes", menuName = "Scriptable Objects/Recipes")]
public class Recipes : ScriptableObject
{
    public List<Ingredientes> requiredIngredientes = new List<Ingredientes>();

    public string nameOfDrink;
    public GameObject drink;
    public int price;
}
