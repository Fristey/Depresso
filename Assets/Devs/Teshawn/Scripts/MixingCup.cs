using System.Collections.Generic;
using UnityEngine;

public class MixingCup : MonoBehaviour
{
    public List<Ingredientes> cupIngredientes;
    public List<string> taglist = new List<string> {"Coffee","Ice","Milk"};
    public List<string> ingredientesNames;

    public float maxAmount = 100f;
    public float currentAmount = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (taglist.Contains(other.tag))
        {   
            cupIngredientes.Add(other.gameObject.GetComponent<Ingredientes>());
        }
    }



    public void Spill(float amount)
    {
        Debug.Log("Spilling amount: " + amount);
        currentAmount = Mathf.Max(currentAmount - amount , 0f);
        Debug.Log( "Current Amount:" + currentAmount);
    }
}
