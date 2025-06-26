using UnityEngine;

public class PouringScript : MonoBehaviour
{
    [SerializeField] private Transform bottleTip;
    [SerializeField] private GameObject ObjectToPour;

    public void PourRate()
    {
        Instantiate(ObjectToPour, bottleTip);
    }
}
