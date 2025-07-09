using UnityEngine;

using System.Collections;
using UnityEngine.VFX;

public class VisualSwapper : MonoBehaviour
{
    [SerializeField] private GameObject visualHolder;

    [SerializeField] private GameObject emptyCup;
    private GameObject curCup;

    [SerializeField] private Vector3 basePos;

    [SerializeField] private Animator vfxAnimator;

    private void Start()
    {
        curCup = emptyCup;
    }

    public void Swap(GameObject visual, Vector3 position)
    {
        if(curCup != visual)
        {
            vfxAnimator.SetTrigger("Swap");
        }

        for (int i = 0; i < visualHolder.transform.childCount; i++)
        {
            Destroy(visualHolder.transform.GetChild(i).gameObject);
        }

        GameObject _go = Instantiate(visual, position, visualHolder.transform.rotation, visualHolder.transform);
        _go.transform.localPosition = position;

        curCup = visual;
    }

    public void ResetVisual()
    {
        if(curCup != emptyCup)
        {
            vfxAnimator.SetTrigger("Swap");
        }

        for (int i = 0; i < visualHolder.transform.childCount; i++)
        {
            Destroy(visualHolder.transform.GetChild(i).gameObject);
        }

        GameObject _go = Instantiate(emptyCup, basePos, visualHolder.transform.rotation, visualHolder.transform);
        _go.transform.localPosition = basePos;

        curCup = emptyCup;
    }
}