using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredientes", menuName = "Scriptable Objects/Recipes")]
public class Recipes : ScriptableObject
{
    public List<Ingredientes> requiredIngredientes;

    public GameObject drink;
}
