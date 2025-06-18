using UnityEngine;

public class CounterEntrance : MonoBehaviour
{
    [SerializeField] private CatScript cat;

    [SerializeField] private Transform area;
    [SerializeField] private MeshRenderer areaRen;
    [SerializeField] private GameObject otherLink;

    [SerializeField] private bool onCounterSwitch;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Cat is going up");
        cat.Jump(area, areaRen,onCounterSwitch,otherLink);
    }
}
