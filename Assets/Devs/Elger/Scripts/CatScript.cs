using System.Collections;
using UnityEngine;
using UnityEngine.AI;


enum CatStates
{
    Sitting,
    Walking,
    Interacting,
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

    [SerializeField] private LayerMask cupCheckMask;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        DestinationReached();
    }

    private void Update()
    {
        switch (state)
        {
            case CatStates.Sitting:
                break;
            case CatStates.Walking:
                if (Vector3.Distance(transform.position, destination) < 1)
                {
                    DestinationReached();
                }
                break;
            case CatStates.Interacting:
                break;
            default:
                break;
        }

        CheckForCups();
    }

    private void CheckForCups()
    {
        Collider[] cups = Physics.OverlapSphere(transform.position, 3, cupCheckMask);
        GameObject[] cupsGO = new GameObject[cups.Length];

        for (int i = 0; i < cups.Length; i++)
        {
            cupsGO[i] = cups[i].gameObject;

        }


        GameObject target = FindNearestCup(cupsGO);
        if(target != null) 
        {
            destination = target.transform.position;
            agent.SetDestination(destination);

            state = CatStates.Walking;
        }
    }

    private void DestinationReached()
    {
        int rolledNum = Random.Range(0, 100);

        if (rolledNum >= walkChance)
        {
            Debug.Log("Walking");
            destination = GenerateTarget();
            agent.destination = destination;

            state = CatStates.Walking;
        }
        else if (rolledNum >= sitChance)
        {
            Debug.Log("Sitting");
            StartCoroutine(SitTimer());
            state = CatStates.Sitting;
        }
        else if (rolledNum >= walkToCupChance)
        {
            Debug.Log("WalkingToCup");
            destination = FindNearestCup(GameObject.FindGameObjectsWithTag("Cup")).transform.position;
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

    private GameObject FindNearestCup(GameObject[] cups)
    {
        if (cups.Length > 0)
        {
            GameObject targetCup = cups[0];

            for (int i = 0; i < cups.Length; i++)
            {
                float curDist = Vector3.Distance(targetCup.transform.position, transform.position);
                float potDist = Vector3.Distance(cups[i].transform.position, transform.position);

                if (potDist < curDist)
                {
                    targetCup = cups[i];
                }
            }

            return targetCup;
        } else { return null; }
    }

    private IEnumerator SitTimer()
    {
        yield return new WaitForSeconds(5);

        int num = Random.Range(0, 100);

        if (num <= walkToCupChance)
        {
            Debug.Log("WalkingToCup");
            destination = FindNearestCup(GameObject.FindGameObjectsWithTag("Cup")).transform.position;
            agent.destination = destination;

            state = CatStates.Walking;
        }
        else
        {
            Debug.Log("Walking");
            destination = GenerateTarget();
            agent.destination = destination;

            state = CatStates.Walking;
        }
    }
}
