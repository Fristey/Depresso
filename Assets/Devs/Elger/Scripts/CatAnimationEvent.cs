using UnityEngine;

public class CatAnimationEvent : MonoBehaviour
{
    [SerializeField] private CatScript cat;

    public void Hit()
    {
        cat.CallInteraction();
    }
}
