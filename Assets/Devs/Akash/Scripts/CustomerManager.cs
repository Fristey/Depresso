using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    public List<GameObject> counterStools = new List<GameObject>();
    public List<GameObject> waitPoints = new List<GameObject>();
    public GameObject exitPoint;
    public GameObject spawnPoint;
    public Animator animations;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
