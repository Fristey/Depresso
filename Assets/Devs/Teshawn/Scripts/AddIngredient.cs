using UnityEngine;

public class AddIngredient : MonoBehaviour
{
    public Ingredientes ingredientes;
    public string nameOfIngredient;


    private void Start()
    {
        nameOfIngredient = ingredientes.nameOfIngredient;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            if (collision.gameObject.GetComponent<MixingCup>().currentAmount < collision.gameObject.GetComponent<MixingCup>().maxAmount)
            {
                if (collision.gameObject.GetComponent<MixingCup>().drinkToserve != null)
                {
                    collision.gameObject.GetComponent<MixingCup>().currentAmount++;
                    Destroy(this.gameObject);
                }
                else
                {
                    collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Add(ingredientes);
                    collision.gameObject.GetComponent<MixingCup>().ingredientesNames.Add(nameOfIngredient);
                    Destroy(this.gameObject);
                }

            }
        }
    }
}
