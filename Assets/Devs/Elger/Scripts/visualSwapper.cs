using UnityEngine;

public class visualSwapper : MonoBehaviour
{
    [SerializeField] private Appearances[] visuals;
    [SerializeField] private GameObject visualHolder;

    [Header("Test")]
    [SerializeField] private bool trigger;
    [SerializeField] private int testIndex;

    public void Swap(int indentifier)
    {
        int index = 0;

        switch (indentifier)
        {
            case 0:
                index = 0;
                break;
            case 1:
                index = 1;
                break;
            case 2:
                index = 2;
                break;
        }

        for (int i = 0; i < visualHolder.transform.childCount; i++)
        {
            Destroy(visualHolder.transform.GetChild(i).gameObject);
        }

        GameObject _go = Instantiate(visuals[index].appearance, visuals[index].position, Quaternion.identity,visualHolder.transform);
        _go.transform.localPosition = visuals[index].position;
    }

    //Testing
    private void Update()
    {
        if(trigger)
        {
            Swap(testIndex);
            trigger = false;
        }
    }
}