using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

enum CatStates
{
    Sitting,
    Walking,
    Interacting,
}

public enum CalledFunction
{
    walk,
    sit,
    walkToCup,
    walkToMachine
}
public class CatScript : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] private CatType type;
    [SerializeField] private CatStates state;
    [SerializeField] private float annoyance;
     private float annoyancePerSec;

    [Header("Movement")]
    [SerializeField] private GameObject center;
    [SerializeField] private float range;
    [SerializeField] private LayerMask generationMask;
    [SerializeField] private Vector3 destination;

    [SerializeField] private Transform accesibleArea;
    [SerializeField] private MeshRenderer accesibleAreaRen;

    [Header("CupLaunch")]
    [SerializeField] private LayerMask cupCheckMask;
    [SerializeField] private float cupLaunchForce;
    [SerializeField] private float cupLauchCD;

    //Componenets
    private NavMeshAgent agent;

    //Misc
    private bool canLaunch = true;
    private bool canDmg = true;
    private bool walkingToMachine = false;
    private espressoAndCoffeeMachine curCoffeeMachine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        annoyancePerSec = type.annoyancePerSec;
    }

    private void Start()
    {
        StartNewAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        Vector3 pos = other.transform.position;
        pos.y = 0;

        Vector3 dir = (pos + other.transform.position).normalized;

        rb.AddForce(dir * cupLaunchForce);

        Vector3 angle = other.transform.rotation.eulerAngles + (dir * cupLaunchForce);

        rb.AddTorque(angle);

        StartNewAction();
        StartCoroutine(CupLaunchCooldown());
    }

    IEnumerator CupLaunchCooldown()
    {
        canLaunch = false;
        yield return new WaitForSeconds(cupLauchCD);
        canLaunch = true;
    }

    private void Update()
    {
        switch (state)
        {
            case CatStates.Sitting:
                break;
            case CatStates.Walking:
                if (Vector3.Distance(transform.position, destination) < 1.5f && walkingToMachine)
                {
                    state = CatStates.Interacting;
                    walkingToMachine = false;
                    agent.isStopped = true;
                    break;
                }
              
                if (Vector3.Distance(transform.position, destination) < 1)
                {
                    if (walkingToMachine)
                    {
                        Debug.Log("Dmg");
                    }
                    else
                    {
                        StartNewAction();
                    }

                }

                if (canLaunch && !walkingToMachine)
                {
                    CheckForCups();
                }
                break;
            case CatStates.Interacting:
                BreakMachine();

                break;
            default:
                break;
        }

        agent.speed = type.speed.Evaluate(annoyance);

        annoyance += annoyancePerSec * Time.deltaTime;
        Mathf.Clamp(annoyance, 0, 100);
    }

    private void CheckForCups()
    {
        //Gets all nearby cups inside a radius around the cat
        Collider[] cups = Physics.OverlapSphere(transform.position, 3, cupCheckMask);

        //Converts the colider array into a GameObject one
        GameObject[] cupsGO = new GameObject[cups.Length];
        for (int i = 0; i < cups.Length; i++)
        {
            cupsGO[i] = cups[i].gameObject;
        }

        GameObject target = FindNearestCup(cupsGO);

        //Checks if a target was found and if so making the cat move in it's direction + state swap
        if (target != null)
        {
            destination = target.transform.position;
            agent.SetDestination(destination);

            state = CatStates.Walking;
        }
    }

    private void StartNewAction()
    {
        int rolledNum = UnityEngine.Random.Range(0, 101);

        for (int i = 0; i < type.catActions.Count; i++)
        {
            Vector2 actionWindow = type.catActions[i].GetWindow(annoyance);

            if (rolledNum >= actionWindow.x && rolledNum < actionWindow.y)
            {
                switch (type.catActions[i].function)
                {
                    case CalledFunction.walk:
                        destination = GenerateTarget();
                        agent.destination = destination;

                        state = CatStates.Walking;
                        break;
                    case CalledFunction.sit:
                        StartCoroutine(SitTimer());
                        state = CatStates.Sitting;
                        break;
                    case CalledFunction.walkToCup:
                        if (canLaunch)
                        {
                            Vector3 v3 = FindNearestCup(GameObject.FindGameObjectsWithTag("Cup")).transform.position;

                            if(v3 != null)
                            {
                                destination = v3;
                            }
                            else
                            {
                                destination = GenerateTarget();
                            }
 
                        }
                        else
                        {
                            destination = GenerateTarget();
                        }

                        agent.destination = destination;

                        state = CatStates.Walking;
                        break;
                    case CalledFunction.walkToMachine:
                        if (canDmg)
                        {
                            destination = FindNearestCup(GameObject.FindGameObjectsWithTag("CoffeeMachine")).transform.position;
                            walkingToMachine = true;
                        }
                        else
                        {
                            destination = GenerateTarget();
                        }

                        agent.destination = destination;

                        state = CatStates.Walking;
                        break;
                }
            }
        }
    }

    private Vector3 GenerateTarget()
    {
        float x = accesibleAreaRen.bounds.size.x/2;
        float y = accesibleAreaRen.bounds.size.y/2;
        float z = accesibleAreaRen.bounds.size.z/2;

        Vector3 bounds = new Vector3(x, y, z);

        Vector3 topRight = accesibleArea.position + bounds;
        Vector3 bottemLeft = accesibleArea.position - bounds;

        Vector3 potentiolTarget = new Vector3(UnityEngine.Random.Range(topRight.x, bottemLeft.x), UnityEngine.Random.Range(topRight.y,bottemLeft.y), UnityEngine.Random.Range(topRight.z, bottemLeft.z));

        NavMeshHit hit;
        var catWalkableMask = 1 << NavMesh.GetAreaFromName("CatWalkable");

        bool b = NavMesh.SamplePosition(potentiolTarget, out hit, range,catWalkableMask);

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

                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(cups[i].transform.position, path);

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    if (potDist < curDist)
                    {
                        targetCup = cups[i];
                    }
                }
            }

            NavMeshPath path1 = new NavMeshPath();
            agent.CalculatePath(targetCup.transform.position, path1);

            if (path1.status == NavMeshPathStatus.PathComplete)
            {
                if(targetCup.tag == "CoffeeMachine")
                {
                    curCoffeeMachine = targetCup.GetComponent<espressoAndCoffeeMachine>();
                }
                return targetCup;
            }
        }
        return null;
    }

    private IEnumerator SitTimer()
    {
        yield return new WaitForSeconds(5);

        int num = UnityEngine.Random.Range(0, 100);

        StartNewAction();
    }

    public void BreakMachine()
    {
        curCoffeeMachine.fixedOrBroken = espressoAndCoffeeMachine.FixedOrBroken.Broken;
        agent.isStopped = false;
        state = CatStates.Walking;

        StartNewAction();
    } 
}
