using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    [SerializeField] private float minCD;
    [SerializeField] private float maxCD;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    [SerializeField] private Animator animator;

    [SerializeField] private List<GameObject> cars = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(CarCD());
    }

    private IEnumerator CarCD()
    {
        float time = Random.Range(minCD, maxCD);
        yield return new WaitForSeconds(time);

        int CarIndex = Random.Range(0, cars.Count);
        for(int i = 0; i < cars.Count; i++)
        {
            if (i == CarIndex)
            {
                cars[i].SetActive(true);
            }
            else
            {
                cars[i].SetActive(false);
            }
        }

        if(Random.Range(0,2) == 0)
        {
            animator.SetTrigger("DriveLeft");
        } else
        {
            animator.SetTrigger("DriveRight");
        }

        animator.speed = Random.Range(minSpeed, maxSpeed);

        StartCoroutine(CarCD());
    }
}
