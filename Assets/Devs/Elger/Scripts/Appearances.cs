using UnityEngine;

[CreateAssetMenu(fileName = "Appearances", menuName = "Scriptable Objects/Appearances")]
public class Appearances : ScriptableObject
{
    //Saved look and spawn position to insure that all coffees have the same ground point
    public Vector3 position;
    public GameObject appearance;
}
