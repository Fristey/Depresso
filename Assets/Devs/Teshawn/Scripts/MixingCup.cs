using System.Collections.Generic;
using UnityEngine;

public class MixingCup : MonoBehaviour
{
    public List<Ingredientes> cupIngredientes;
    public List<string> taglist = new List<string> {"Coffee","Ice","Milk"};
    public List<string> ingredientesNames;
    public Recipes drinkToserve;

    public float maxAmount = 100f;
    public float currentAmount = 0f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MixingStation"))
        {
            other.gameObject.GetComponent<MixingStation>().CreateDrink();
        }

    }



    public void Spill(float amount)
    {
        Debug.Log("Spilling amount: " + amount);
        currentAmount = Mathf.Max(currentAmount - amount , 0f);
        Debug.Log( "Current Amount:" + currentAmount);
    }
}
