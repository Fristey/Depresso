using UnityEngine;

public class AddIngredient : MonoBehaviour
{
    private Vector3 originalPos;

    public Ingredientes ingredientes;
    public string nameOfIngredient;

    private void Start()
    {
        nameOfIngredient = ingredientes.nameOfIngredient;
        originalPos = this.transform.position;
    }

    private void Update()
    {
        if(this.transform.position.y < -5)
            this.transform.position = originalPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MixingCup>() != null)
        {
            if (collision.gameObject.GetComponent<MixingCup>().currentAmount < collision.gameObject.GetComponent<MixingCup>().maxAmount)
            {
                if (collision.gameObject.GetComponent<MixingCup>().drinkToserve != null)
                {
                    collision.gameObject.GetComponent<MixingCup>().currentAmount++;
                    Destroy(this.gameObject);
                }
                else if(!collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Contains(ingredientes))
                {
                    collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Add(ingredientes);
                    collision.gameObject.GetComponent<MixingCup>().ingredientesNames.Add(nameOfIngredient);
                    Destroy(this.gameObject);
                }
                collision.gameObject.GetComponent<MixingCup>().currentAmount++;
                Destroy(this.gameObject);
            }
        }
    }
}
