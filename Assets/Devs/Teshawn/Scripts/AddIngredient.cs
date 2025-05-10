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
            collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Add(ingredientes);
            collision.gameObject.GetComponent<MixingCup>().ingredientesNames.Add(nameOfIngredient);
        }
    }
}
