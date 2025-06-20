using UnityEngine;

public class Trashcan : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MixingCup>() != null)
        {
            VisualSwapper swapper = collision.gameObject.GetComponent<VisualSwapper>();

            if (collision.gameObject.GetComponent<MixingCup>().drinkToserve == null)
            {
                collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Clear();
                collision.gameObject.GetComponent<MixingCup>().currentAmount = 0;
            }
            else
            {
                swapper.ResetVisual();
                collision.gameObject.GetComponent<MixingCup>().drinkToserve = null;
            }
        }
    }
}
