using System.Collections.Generic;
using UnityEngine;

public class MixingCup : MonoBehaviour
{
    public List<Ingredientes> cupIngredientes;
    public List<string> taglist = new List<string> {"Coffee","Ice","Milk"};
    public List<string> ingredientesNames;

    private void OnTriggerEnter(Collider other)
    {
        if (taglist.Contains(other.tag))
        {   
            cupIngredientes.Add(other.gameObject.GetComponent<Ingredientes>());
        }
    }
}
