using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

enum CatStates
{
    Sitting,
    Walking,
    Interacting,
    Distracted,
    WalkingToCup,
    Jumping
}

public enum CalledFunction
{
    walk,
    sit,
    walkToCup,
    walkToMachine
}

public enum CalledInteraction
{
    damage,
    push
}
public class CatScript : PermEvent
{
    [Header("Behaviour")]
    [SerializeField] private CatType type;
    [SerializeField] private CatStates state;
    [SerializeField] private float annoyance;
    private float annoyancePerSec;
    [SerializeField] private Vector3 spawnPos;

    [Header("Movement")]
    [SerializeField] private float range;
    [SerializeField] private Vector3 destination;

    [SerializeField] private Transform accesibleArea;
    [SerializeField] private MeshRenderer accesibleAreaRen;

    [SerializeField] private float heightChangeCD;
    private bool canChangeHeight = true;
    private bool onCounter = false;

    public bool isJumping = false;
    private bool canJump = true;

    string areaMask = "CatWalkable";
    [SerializeField] private Vector3 groundHeight;
    [SerializeField] private Vector3 counterHeight;

    private Vector3 floorTopRight;
    private Vector3 floorBottemLeft;

    private Vector3 counterTopRight;
    private Vector3 counterBottemLeft;

    private Transform counterAccesibleArea;
    private MeshRenderer counterAccesibleAreaRen;

    [SerializeField] private GameObject[] counterLinks;

    private bool sitbuffer = false;

    private List<Vector3> dests = new List<Vector3>();

    [Header("Interaction")]
    [SerializeField] private LayerMask cupCheckMask;
    [SerializeField] private float cupLaunchForce;
    [SerializeField] private float cupLauchCD;

    private GameObject curInteractObject;
    private CalledInteraction curInteract;

    [Header("Yarn")]
    [SerializeField] private YarnSpawner spawner;

    private GameObject yarnBall;

    [Header("Components")]
    [SerializeField] private Animator animator;
    public NavMeshAgent agent;


    //Misc
    private bool canLaunch = true;
    private bool canDmg = true;
    private bool walkingToMachine = false;
    private bool focus = false;
    private espressoAndCoffeeMachine curCoffeeMachine;

    private TutorialManager tutorialManager;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        annoyancePerSec = type.annoyancePerSec;

        accesibleArea = GameObject.FindWithTag("CatBounds").transform;
        accesibleAreaRen = accesibleArea.GetComponent<MeshRenderer>();

        float x1 = accesibleAreaRen.bounds.size.x / 2;
        float y1 = accesibleAreaRen.bounds.size.y / 2;
        float z1 = accesibleAreaRen.bounds.size.z / 2;

        Vector3 bounds1 = new Vector3(x1, y1, z1);

        floorTopRight = accesibleArea.position + bounds1;
        floorBottemLeft = accesibleArea.position - bounds1;

        spawner = FindFirstObjectByType<YarnSpawner>();
        spawner.catScript = this;
    }

    private void Start()
    {
        tutorialManager = TutorialManager.instance;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(HasStarted)
        {
            tutorialManager.StartTutorial("Cat");
            StartSpecificAction(CalledFunction.walk);
        }
    }
    #region Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (!walkingToMachine && state != CatStates.Jumping && !focus && !isJumping)
        {
            SetInteract(CalledInteraction.push, other.gameObject);

            if (tutorialManager.StepFinished("Cat", 2))
            {
                //canChangeHeight = true;
                //counterLink.SetActive(true);
                //StartSpecificAction(CalledFunction.walkToMachine);
            }
        }
    }

    public void SetInteract(CalledInteraction interaction, GameObject go)
    {
        curInteract = interaction;
        curInteractObject = go;

        agent.isStopped = true;

        switch (interaction)
        {
            case CalledInteraction.damage:
                walkingToMachine = false;
                break;
            case CalledInteraction.push:
                break;
        }

        animator.SetTrigger("Interact");
    }

    public void CallInteraction()
    {
        switch (curInteract)
        {
            case CalledInteraction.damage:
                curCoffeeMachine.fixedOrBroken = espressoAndCoffeeMachine.FixedOrBroken.Broken;
                tutorialManager.StepFinished("Cat", 3);
                StartNewAction();
                break;
            case CalledInteraction.push:
                Rigidbody rb = curInteractObject.GetComponent<Rigidbody>();

                Vector3 pos = curInteractObject.transform.position;
                pos.y = 0;

                Vector3 dir = (pos + curInteractObject.transform.position).normalized;

                rb.AddForce(dir * cupLaunchForce);

                Vector3 angle = curInteractObject.transform.rotation.eulerAngles + (dir * cupLaunchForce);

                rb.AddTorque(angle);
                if (curInteractObject.tag == "Cup")
                {
                    StartNewAction();
                    StartCoroutine(CupLaunchCooldown());
                }
                break;
        }

        agent.isStopped = false;
        state = CatStates.Walking;
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

    IEnumerator CupLaunchCooldown()
    {
        canLaunch = false;
        yield return new WaitForSeconds(cupLauchCD);
        canLaunch = true;
    }
    #endregion
    #region Destination Genenration and Location checking
    private Vector3 GenerateTarget(Vector3 target = default(Vector3))
    {
        Vector3 potentiolTarget = Vector3.zero;

        if (target != default(Vector3))
        {
            NavMeshHit hit;
            var catWalkableMask = 1 << NavMesh.GetAreaFromName(areaMask);

            NavMesh.SamplePosition(target, out hit, range, catWalkableMask);

            if (CheckPath(hit.position, areaMask))
            {
                Debug.Log("Route possible");
                potentiolTarget = hit.position;
            }
        }

        if (potentiolTarget == Vector3.zero)
        {
            int loops = 0;

            Vector3 topRight;
            Vector3 bottemLeft;

            string otherMask;

            //if (transform.position.y >= counterHeight.y && !canChangeHeight)
            //{
            //topRight = counterTopRight;
            //bottemLeft = counterBottemLeft;

            //areaMask = "Counter"; ;

            //float x2 = counterAccesibleAreaRen.bounds.size.x / 2;
            //float y2 = counterAccesibleAreaRen.bounds.size.y / 2;
            //float z2 = counterAccesibleAreaRen.bounds.size.z / 2;

            //Vector3 bounds2 = new Vector3(x2, y2, z2);

            //counterTopRight = counterAccesibleArea.position + bounds2;
            //counterBottemLeft = counterAccesibleArea.position - bounds2;
            //}
            //else
            //{
            topRight = floorTopRight;
            bottemLeft = floorBottemLeft;

            areaMask = "CatWalkable";
            //}

            while (potentiolTarget == Vector3.zero && loops < 100)
            {
                Vector3 tempTarget = new Vector3(UnityEngine.Random.Range(topRight.x, bottemLeft.x), UnityEngine.Random.Range(topRight.y, bottemLeft.y), UnityEngine.Random.Range(topRight.z, bottemLeft.z));

                NavMeshHit hit;
                var catWalkableMask = 1 << NavMesh.GetAreaFromName(areaMask);

                NavMesh.SamplePosition(tempTarget, out hit, range, catWalkableMask);

                if (CheckPath(hit.position, areaMask))
                {
                    potentiolTarget = hit.position;
                    dests.Clear();
                }

                Debug.Log("loops = " + loops);

                dests.Add(tempTarget);

                loops++;
            }
        }
        return potentiolTarget;
    }

    private bool CheckPath(Vector3 goal, string areaMask)
    {
        NavMeshPath path = new NavMeshPath();
        var catWalkableMask = 1 << NavMesh.GetAreaFromName(areaMask);

        NavMesh.CalculatePath(transform.position, goal, catWalkableMask, path);

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

                if (CheckPath(cups[i].transform.position, areaMask))
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

                return targetCup;
        }
        return null;
    }
    #endregion
    private void Update()
    {
        if(agent.isOnOffMeshLink && !isJumping && canJump)
        {
            Debug.Log("Start jump");
            StartCoroutine(MaxJumpTime());
            StartChangeHeightCD();

            if(CheckPath(destination, areaMask))
            {
                destination = GenerateTarget();
                agent.destination = destination;
            }

            isJumping = true;

            agent.isStopped = true;
            animator.SetBool("Jump", true);
            state = CatStates.Jumping;
        }

        switch (state)
        {
            case CatStates.Sitting:
                break;
            case CatStates.Walking:
                if (Vector3.Distance(transform.position, destination) < 1.2f && walkingToMachine)
                {
                    SetInteract(CalledInteraction.damage, curCoffeeMachine.gameObject);
                    break;
                }
                else if (Vector3.Distance(transform.position, destination) < 0.3f && !walkingToMachine)
                {
                    StartNewAction();
                    focus = false;
                    if (tutorialManager.StepFinished("Cat", 1))
                    {
                        StartSpecificAction(CalledFunction.walkToCup);
                    }
                }

                if (canLaunch && !walkingToMachine && !focus)
                {
                    CheckForCups();
                }
                break;
            case CatStates.Interacting:

                break;
            case CatStates.Distracted:
                if (yarnBall != null)
                {
                    if (CheckPath(yarnBall.transform.position, areaMask))
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
            case CatStates.Jumping:
                if (!agent.isOnOffMeshLink)
                {
                    animator.SetBool("Jump", false);
                    state = CatStates.Walking;
                }
                break;
            default:
                break;
        }

        agent.speed = type.speed.Evaluate(annoyance);

        if (state != CatStates.Distracted)
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

    #region Actions
    private void StartSpecificAction(CalledFunction function)
    {
        switch (function)
        {
            case CalledFunction.walk:
                destination = GenerateTarget();
                agent.destination = destination;

                state = CatStates.Walking;
                break;
            case CalledFunction.sit:
                StartCoroutine(SitTimer());
                state = CatStates.Sitting;
                sitbuffer = true;
                break;
            case CalledFunction.walkToCup:

                GameObject v3 = FindNearestCup(GameObject.FindGameObjectsWithTag("Cup"));

                if (v3 != null)
                {
                    destination = v3.transform.position;
                    state = CatStates.WalkingToCup;
                }

                agent.destination = destination;
                break;
            case CalledFunction.walkToMachine:

                GameObject go = FindNearestCup(GameObject.FindGameObjectsWithTag("CoffeeMachine"));

                if (go != null)
                {
                    Debug.Log("Walking to machine");
                    destination = GenerateTarget(go.transform.position);
                    curCoffeeMachine = go.GetComponent<espressoAndCoffeeMachine>();
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
                        if (!sitbuffer)
                        {
                            StartCoroutine(SitTimer());
                            state = CatStates.Sitting;
                            sitbuffer = true;
                        }
                        else
                        {
                            sitbuffer = false;
                            StartNewAction();
                        }

                        break;
                    case CalledFunction.walkToCup:
                        if (canLaunch)
                        {
                            GameObject v3 = FindNearestCup(GameObject.FindGameObjectsWithTag("Cup"));

                            if (v3 != null)
                            {
                                destination = v3.transform.position;
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
                            GameObject go = FindNearestCup(GameObject.FindGameObjectsWithTag("CoffeeMachine"));

                            if (go != null)
                            {
                                Debug.Log("Walking to machine");
                                destination = GenerateTarget(go.transform.position);
                                curCoffeeMachine = go.GetComponent<espressoAndCoffeeMachine>();
                                walkingToMachine = true;
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
                }

                if (type.catActions[i].function != CalledFunction.sit)
                {
                    sitbuffer = false;
                }
            }
        }
    }

    private IEnumerator SitTimer()
    {
        animator.SetBool("Sitting", true);
        yield return new WaitForSeconds(5);
        animator.SetBool("Sitting", false);

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
    #endregion


    //public void Jump(Transform areaTrans, MeshRenderer areaRen, bool input, GameObject[] link)
    //{
    //    //float dist = Vector3.Distance(new Vector3(0, transform.position.y, 0), new Vector3(0, destination.y, 0));

    //    //if (canChangeHeight && dist > 0.5f)
    //    //{
    //    //    counterAccesibleArea = areaTrans;
    //    //    counterAccesibleAreaRen = areaRen;

    //    //    counterLinks = link;
    //    //    onCounter = input;

    //    //    agent.isStopped = true;
    //    //    animator.SetBool("Jump", true);
    //    //    state = CatStates.Jumping;

    //    //    StartCoroutine(HeightChangeCD());
    //    //}
    //}
    private IEnumerator MaxJumpTime()
    {
        yield return new WaitForSeconds(2);
        if(agent.isStopped)
        {
            agent.isStopped = false;
            isJumping = false;
            animator.SetBool("Jump", false);
        }
    }

    public void StartChangeHeightCD()
    {
        StartCoroutine(HeightChangeCD());
    }
    private IEnumerator HeightChangeCD()
    {
        for (int i = 0; i < counterLinks.Length; i++)
        {
            counterLinks[i].SetActive(false);
        }
        canJump = false;
        yield return new WaitForSeconds(heightChangeCD);
        canJump = true;
        for (int i = 0; i < counterLinks.Length; i++)
        {
            counterLinks[i].SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(destination, Vector3.one);

        Gizmos.color = Color.yellow;
        for (int i = 0; i < dests.Count; i++)
        {
            Gizmos.DrawCube(dests[i], Vector3.one);
        }
    }
}