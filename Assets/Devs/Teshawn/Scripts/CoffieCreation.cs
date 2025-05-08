using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoffieCreation : MonoBehaviour
{
    [SerializeField] private GameObject coffie;
    [SerializeField] private Transform coffieSpawnPoint;


    [SerializeField] private float timeBeforeCoffieDone;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Invoke(nameof(CoffieSpawn), timeBeforeCoffieDone);
            //call a function that spawns the coffie after a few seconds
        }
    }

    private void CoffieSpawn()
    {
        Instantiate(coffie, coffieSpawnPoint.position, Quaternion.identity);
    }
}
//if the machine is pressed then after a few seconds the coffie comes out