using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

enum CatStates
{
    Sitting,
    Walking,
    Interacting,
    Distracted,
    WalkingToCup
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
    [SerializeField] private float range;
    [SerializeField] private LayerMask generationMask;
    [SerializeField] private Vector3 destination;

    [SerializeField] private Transform accesibleArea;
    [SerializeField] private MeshRenderer accesibleAreaRen;

    [SerializeField] private float heightChangeCD;
    private bool canChangeHeight = true;
    private bool onCounter = false;

    string areaMask = "CatWalkable";

    private Vector3 floorTopRight;
    private Vector3 floorBottemLeft;

    private Vector3 counterTopRight;
    private Vector3 counterBottemLeft;

    private Transform counterAccesibleArea;
    private MeshRenderer counterAccesibleAreaRen;

    [SerializeField] private GameObject counterLink;

    [Header("CupLaunch")]
    [SerializeField] private LayerMask cupCheckMask;
    [SerializeField] private float cupLaunchForce;
    [SerializeField] private float cupLauchCD;

    [Header("Yarn")]
    [SerializeField] private YarnSpawner spawner;

    private GameObject yarnBall;

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

        float x1 = accesibleAreaRen.bounds.size.x / 2;
        float y1 = accesibleAreaRen.bounds.size.y / 2;
        float z1 = accesibleAreaRen.bounds.size.z / 2;

        Vector3 bounds1 = new Vector3(x1, y1, z1);

        floorTopRight = accesibleArea.position + bounds1;
        floorBottemLeft = accesibleArea.position - bounds1;
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
        if (other.tag == "Cup")
        {
            StartNewAction();
            StartCoroutine(CupLaunchCooldown());
        }
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

                if (Vector3.Distance(transform.position, destination) < 0.2f)
                {
                    StartNewAction();
                }

                if (canLaunch && !walkingToMachine)
                {
                    CheckForCups();
                }
                break;
            case CatStates.Interacting:
                BreakMachine();

                break;
            case CatStates.Distracted:
                if (yarnBall != null)
                {
                    if (CheckPath(yarnBall.transform.position))
                    {
                        destination = yarnBall.transform.position;
                        agent.destination = destination;
                    }
                    else
                    {
                        EndDistraction();
                        spawner.ReturnYarn();
                    }
                }
                break;
            case CatStates.WalkingToCup:
                if (Vector3.Distance(transform.position, destination) < 0.2)
                {
                    StartNewAction();
                }
                break;
            default:
                break;
        }

        agent.speed = type.speed.Evaluate(annoyance);

        if(state != CatStates.Distracted)
        {
            if (annoyance < 100)
            {
                annoyance += annoyancePerSec * Time.deltaTime;
            }
        }
        else if (annoyance > 0)
        {
            annoyance -= annoyancePerSec * Time.deltaTime;
        }

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
            destination = GenerateTarget(target.transform.position);
            agent.SetDestination(destination);

            StartCoroutine(CupLaunchCooldown());

            state = CatStates.WalkingToCup;
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

                            if (v3 != null)
                            {
                                destination = v3;
                                state = CatStates.WalkingToCup;
                            }
                        }
                        else
                        {
                            destination = GenerateTarget();
                            state = CatStates.Walking;
                        }

                        agent.destination = destination;


                        break;
                    case CalledFunction.walkToMachine:
                        if (canDmg)
                        {
                            destination = GenerateTarget(FindNearestCup(GameObject.FindGameObjectsWithTag("CoffeeMachine")).transform.position);
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

    private Vector3 GenerateTarget(Vector3 target = default(Vector3))
    {
        Vector3 potentiolTarget = Vector3.zero;

        if (target != default(Vector3))
        {
            NavMeshHit hit;
            var catWalkableMask = 1 << NavMesh.GetAreaFromName(areaMask);

            NavMesh.SamplePosition(target, out hit, range, catWalkableMask);

            if (CheckPath(hit.position))
            {
                potentiolTarget = hit.position;
            }
        }
        
        if(potentiolTarget == Vector3.zero)
        {
            int loops = 0;

            Vector3 topRight;
            Vector3 bottemLeft;

            if (onCounter && !canChangeHeight)
            {
                topRight = counterTopRight;
                bottemLeft = counterBottemLeft;

                areaMask = "CatCounter";

                float x2 = counterAccesibleAreaRen.bounds.size.x / 2;
                float y2 = counterAccesibleAreaRen.bounds.size.y / 2;
                float z2 = counterAccesibleAreaRen.bounds.size.z / 2;

                Vector3 bounds2 = new Vector3(x2, y2, z2);

                counterTopRight = counterAccesibleArea.position + bounds2;
                counterBottemLeft = counterAccesibleArea.position - bounds2;

            }
            else
            {
                topRight = floorTopRight;
                bottemLeft = floorBottemLeft;

                areaMask = "CatWalkable";
            }

            while (potentiolTarget == Vector3.zero && loops < 100)
            {
                Vector3 tempTarget = new Vector3(UnityEngine.Random.Range(topRight.x, bottemLeft.x), UnityEngine.Random.Range(topRight.y, bottemLeft.y), UnityEngine.Random.Range(topRight.z, bottemLeft.z));

                NavMeshHit hit;
                var catWalkableMask = 1 << NavMesh.GetAreaFromName(areaMask);

                NavMesh.SamplePosition(tempTarget, out hit, range, catWalkableMask);

                if (CheckPath(hit.position))
                {
                    potentiolTarget = hit.position;
                }

                loops++;
            }
            Debug.Log("generating dest took" + loops + "tries");
        }
        return potentiolTarget;
    }

    private bool CheckPath(Vector3 goal)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(goal, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        else
        {
            return false;
        }
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

                if (CheckPath(cups[i].transform.position))
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
                if (targetCup.tag == "CoffeeMachine")
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

        StartNewAction();
    }

    public void BreakMachine()
    {
        curCoffeeMachine.fixedOrBroken = espressoAndCoffeeMachine.FixedOrBroken.Broken;
        agent.isStopped = false;
        state = CatStates.Walking;

        StartNewAction();
    }

    public void StartDistraction(GameObject distraction)
    {
        StopAllCoroutines();

        yarnBall = distraction;
        state = CatStates.Distracted;
    }

    public void EndDistraction()
    {
        StartNewAction();

        yarnBall = null;
    }

    public void Jump(Transform areaTrans, MeshRenderer areaRen, bool input, GameObject link)
    {
        if (!onCounter)
        {
            counterAccesibleArea = areaTrans;
            counterAccesibleAreaRen = areaRen;
        }

        counterLink = link;
        onCounter = input;

        StartCoroutine(HeightChangeCD());
    }
    private IEnumerator HeightChangeCD()
    {
        canChangeHeight = false;
        counterLink.SetActive(false);
        yield return new WaitForSeconds(heightChangeCD);
        canChangeHeight = true;
        counterLink.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(destination, Vector3.one);
    }
}