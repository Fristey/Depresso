using UnityEngine;

public class JarGrabScript : MonoBehaviour
{
    public Ingredientes ingredient;

    public GameObject currentIngredient;
    public void SpawnInIngredient(Vector3 pos)
    {
        currentIngredient = Instantiate(ingredient.ingredientToSpawn, pos, Quaternion.identity);
    }


}
