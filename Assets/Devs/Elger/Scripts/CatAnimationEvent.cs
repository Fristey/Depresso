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
    }

    public void LockMovement()
    {
        cat.agent.isStopped = true;
    }

    public void UnlockMovement()
    {
        cat.agent.isStopped = false;    
    }
}
