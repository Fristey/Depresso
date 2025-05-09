using UnityEngine;

public class Trashcan : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Clear();
        }
    }
}
