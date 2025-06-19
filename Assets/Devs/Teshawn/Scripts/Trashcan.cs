using UnityEngine;

public class Trashcan : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MixingCup>() != null)
        {
            if (collision.gameObject.GetComponent<MixingCup>().drinkToserve == null)
            {
                collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Clear();
                collision.gameObject.GetComponent<MixingCup>().currentAmount = 0;
            }
            else
            {
                //Elger: channge the drink back to the normal cup
                collision.gameObject.GetComponent<MixingCup>().drinkToserve = null;
            }
        }
    }
}
