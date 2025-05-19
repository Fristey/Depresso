using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Ingredientes", menuName = "Scriptable Objects/Ingredientes")]
public class Ingredientes : ScriptableObject,IComparable<Ingredientes>
{
    public string nameOfIngredient;

    public GameObject ingredientToSpawn;

    public int CompareTo(Ingredientes other)
    {
        return nameOfIngredient.CompareTo(other.nameOfIngredient);
    }
}
