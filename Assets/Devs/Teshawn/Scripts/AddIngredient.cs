using UnityEngine;

public class AddIngredient : MonoBehaviour
{
    public Ingredientes ingredientes;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Add(ingredientes);
        }
    }
}
