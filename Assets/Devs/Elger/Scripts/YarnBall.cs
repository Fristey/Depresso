using UnityEngine;

public class YarnBall : MonoBehaviour
{
    [SerializeField] private bool trigger = false;

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            DestroyBall();
        }
    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }
}
