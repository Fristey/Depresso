using UnityEngine;

public class YarnBall : MonoBehaviour
{
    [SerializeField] private bool trigger = false;
    [SerializeField] private Rigidbody rb;

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            DestroyBall();
        }
    }

    public void Release()
    {
        if(rb.isKinematic)
        {
            rb.isKinematic = false;
        } else
        {
            rb.isKinematic = true;
        }

    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }
}
