using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

enum CatStates
{
    Sitting,
    Walking,
    Interacting,
    Waiting
}
public class CatScript : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private CatStates state;

    [SerializeField] private GameObject center;
    [SerializeField] private float range;
    [SerializeField] private LayerMask generationMask;
    [SerializeField] private Vector3 destination;

    [SerializeField] private float walkChance;
    [SerializeField] private float sitChance;
    [SerializeField] private float walkToCupChance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch (state)
        {
            case CatStates.Sitting:
                break;
            case CatStates.Walking:
                if(Vector3.Distance(transform.position,destination) < 1)
                {
                    DestinationReached();
                }
                break;
            case CatStates.Interacting:
                break;
            default:
                break;
        }
    }

    private void DestinationReached()
    {
        int rolledNum = Random.Range(0, 100);

        if(rolledNum >= walkChance) 
        {
            destination = GenerateTarget();
            agent.destination = destination;

            state = CatStates.Walking;
        }
        else if (rolledNum >= sitChance) 
        {
            StartCoroutine(SitTimer());
            state = CatStates.Sitting;
        }
        else if (rolledNum >= walkToCupChance)
        {
            destination = FindNearestCup().transform.position;
            agent.destination = destination;

            state = CatStates.Walking;
        }


    }

    private Vector3 GenerateTarget()
    {
        Vector3 potentiolTarget = new Vector3(center.transform.position.x - Random.Range(-range, range), center.transform.position.y, center.transform.position.z - Random.Range(-range, range));

        NavMeshHit hit;
        NavMesh.SamplePosition(potentiolTarget, out hit, range, generationMask);

        return hit.position;
    }

    private GameObject FindNearestCup()
    {
        GameObject[] cups = GameObject.FindGameObjectsWithTag("Cup");

        GameObject targetCup = cups[0];

        for(int i = 0; i < cups.Length; i++)
        {
            float curDist = Vector3.Distance(targetCup.transform.position,transform.position);
            float potDist = Vector3.Distance(cups[i].transform.position, transform.position);

            if(potDist < curDist) 
            {
                targetCup = cups[i];
            }
        }

        return targetCup;
    }

    private IEnumerator SitTimer()
    {
        yield return new WaitForSeconds(5);

        int num = Random.Range(0, 100);

        if(num <= walkToCupChance)
        {
            destination = FindNearestCup().transform.position;
            agent.destination = destination;

            state = CatStates.Walking;
        }
        else
        {
            destination = GenerateTarget();
            agent.destination = destination;

            state = CatStates.Walking;
        }
    }
}
