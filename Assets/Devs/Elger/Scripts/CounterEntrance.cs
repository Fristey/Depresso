using UnityEngine;

public class CounterEntrance : MonoBehaviour
{
    [SerializeField] private CatScript cat;

    [SerializeField] private Transform area;
    [SerializeField] private MeshRenderer areaRen;
    [SerializeField] private GameObject[] otherLinks;

    [SerializeField] private bool onCounterSwitch;

    //private void OnTriggerEnter(Collider other)
    //{
    //    cat.Jump(area, areaRen,onCounterSwitch,otherLinks);
    //}
}
