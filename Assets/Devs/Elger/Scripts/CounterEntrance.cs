using UnityEngine;

public class CounterEntrance : MonoBehaviour
{
    [SerializeField] private CatScript cat;

    [SerializeField] private Transform area;
    [SerializeField] private MeshRenderer areaRen;

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Cat is going up");
        cat.Jump(area, areaRen);
    }
}
