using UnityEngine;

public class JarGrabScript : MonoBehaviour
{
    public Ingredientes ingrediente;

    public void SpawnInIngredient(Vector3 pos)
    {
        Instantiate(ingrediente.ingredientToSpawn,pos,Quaternion.identity);
    }
}
