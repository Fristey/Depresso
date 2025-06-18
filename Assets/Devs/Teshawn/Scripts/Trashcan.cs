using Copying;
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
                collision.gameObject.GetComponent<MixingCup>().drinkToserve = null;
                Copy.CopyingComponents(collision.gameObject.GetComponent<MixingCup>().normalCup, collision.gameObject);
            }
        }
    }
}
