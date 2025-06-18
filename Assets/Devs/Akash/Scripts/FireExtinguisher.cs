using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Fire"))
        {
            FireSource fireSource = other.GetComponent<FireSource>();
            if (fireSource != null)
            {
                fireSource.ExtinguishFire();
            }
        }
    }
}

