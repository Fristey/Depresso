using UnityEngine;

public class CatAnimationEvent : MonoBehaviour
{
    [SerializeField] private CatScript cat;

    public void Hit()
    {
        cat.CallInteraction();
    }

    public void Land()
    {
        cat.isJumping = false;
        cat.StartChangeHeightCD();
    }

    public void LockMovement()
    {
        cat.agent.isStopped = true;
        Debug.Log("Cant move");
    }

    public void UnlockMovement()
    {
        cat.agent.isStopped = false;
        Debug.Log("Can move");
    }
}
