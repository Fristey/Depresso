using UnityEngine;
using UnityEngine.UIElements;

public class VisualSwapper : MonoBehaviour
{
    [SerializeField] private Appearances[] visuals;
    [SerializeField] private GameObject visualHolder;

    [SerializeField] private GameObject emptyCup;
    [SerializeField] private Vector3 basePos;

    public void Swap(GameObject visual, Vector3 position)
    {
        for (int i = 0; i < visualHolder.transform.childCount; i++)
        {
            Destroy(visualHolder.transform.GetChild(i).gameObject);
        }

        GameObject _go = Instantiate(visual, position, Quaternion.identity,visualHolder.transform);
        _go.transform.localPosition = position;
    }

    public void ResetVisual()
    {
        for (int i = 0; i < visualHolder.transform.childCount; i++)
        {
            Destroy(visualHolder.transform.GetChild(i).gameObject);
        }

        GameObject _go = Instantiate(emptyCup, basePos, Quaternion.identity, visualHolder.transform);
        _go.transform.localPosition = basePos;
    }
}