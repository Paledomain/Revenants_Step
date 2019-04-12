using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AIController : MonoBehaviour, ITurnBased {

    //properties
    public FieldOfView fow;
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;

    public GameObject target;

    public LineRenderer line;

    public Camera cam;

    public float APRange = 10;
    public int AP = 2;
    public float attackRange = 2.0f;

    public float timer;
    public float delay = 0.2f;
    public float damping = 1.0f;

    public Transform[] points;
    private int destPoint = 0;

    public bool patrol;


    // Use this for initialization
    void Start ()
    {
        timer = delay;
    }

    // Update is called once per frame
    void Update ()
    {
    }

    void GotoNextPoint()//wont go to next point, is something wrong with here??
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        if (agent.remainingDistance > agent.stoppingDistance)//decide to move or stop using ditance to next patrol point
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }

        Debug.Log(points[destPoint].position);
        // next point in the array is the next patrol point, cycle back when necessary
        destPoint = (destPoint + 1) % points.Length;
    }

    //runs when game is in combat mode
    public bool DoTurn()
    {
        line.gameObject.SetActive(true);//open line that shows movement target
        if (AP > 0)
        {
            Vector3 movementPoint = transform.position;
            movementPoint = movementRange(APRange);

            float distance = Vector3.Distance(transform.position, target.transform.position);

            Debug.Log("doing action!");//attack or move wrt to distance
            AP--;
            if (distance < attackRange)
            {
                character.Attack();
            }
            else
            {
                agent.SetDestination(movementPoint);
            }

            if (agent.remainingDistance > agent.stoppingDistance)//stop when close to target
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
            return false;
        }
        return true;
    }


    public void StartTurn()//called at the start of the turn
    {
        AP = 2;
    }

    public void TransitionToCombat()//called once when stealth ends
    {
        agent.isStopped = true;//stops agent so that they dont slide off to the next patrol point when combat starts
        //this might be why the NPCs refuse to move in combat
        character.Stop();

        Debug.Log("combat-stop!");
        agent.speed = 3;//run at combat
    }

    public void StartStealth()//called once combat mode ends
    {
        patrol = true;//Maybe I dont need this? Observe.
        Debug.Log("Start stealth!");
        agent.speed = 1;//walk during stealth
        GotoNextPoint();//initiate patrolling
    }

    //public bool wasvisible = false;

    public void doStealth()
    {
        line.gameObject.SetActive(false);//target line invisible now
        //trying to implement a more deliberate detection system
        //timer -= Time.deltaTime;
        

        if (PlayerIsVisible())
        {
            //if (wasvisible == false)
            //{
            //    //timer = delay;
            //    wasvisible = true;
            //}

            //so that the NPC stops moving and looks at the player when they are noticed
            agent.isStopped = true;
            character.Stop();
            float distance = Vector3.Distance(transform.position, target.transform.position);
            Vector3 relativePos = target.transform.position - transform.position;

            Quaternion q = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * damping);

            //a coroutine try at making the detecion more deliberate, 
            StopCoroutine("Detection");
            StartCoroutine("Detection");

            //if (timer <= 0)
            //    timer = delay;

        }
        else
        {
            //Debug.Log("continue!");
            agent.isStopped = false;//continue patrolling
            //wasvisible = false;
            if (!agent.pathPending && agent.remainingDistance < 1.6f)
                 GotoNextPoint();
        } 
    }

    IEnumerator Detection()//still too fast
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        GameManager.gm.stealth = GameManager.gm.stealth - 45 / distance;
        yield return new WaitForSeconds(1.0f);


    }

    public bool PlayerIsVisible()
    {
        //an object within NPC's cone of sight is tagged "player"
        for (int i = 0; i < fow.visibleTargets.Count; i++)
        {
            if(fow.visibleTargets[i].tag == "Player")
            {
                return true;
            }  
        }
        return false;
    }


    public Vector3 movementRange(float maxDist)//calculate and visualize the movement range during combat
    {
        NavMeshPath path = new NavMeshPath();
        float dist = 0;
        List<Vector3> cornerList = new List<Vector3>();//corners of the path
        Vector3 finalPoint = transform.position;

        Ray ray = cam.ScreenPointToRay(target.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && !character.IsDead())
        {
            if (NavMesh.CalculatePath(transform.position, hit.point, agent.areaMask, path))
            {
                cornerList.Add(transform.position);

                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    float distCalc = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                    dist += distCalc;//distance is calculated so that it doesnt surpass maximum movement range

                    if (dist < maxDist)//if total distance at a corner less than maximm distance, add corner
                    {
                        cornerList.Add(path.corners[i + 1]);
                    }
                    else//else shorten line leading to point and replace point
                    {
                        dist -= distCalc;
                        float distDiff = maxDist - dist;
                        Vector3 pathDirection = path.corners[i + 1] - path.corners[i];
                        pathDirection.Normalize();
                        pathDirection *= distDiff;

                        cornerList.Add(path.corners[i] + pathDirection);
                        break;
                    }
                }




#pragma warning disable CS0618 // Type or member is obsolete
                line.SetVertexCount(cornerList.Count);
#pragma warning restore CS0618 // Type or member is obsolete
                line.SetPositions(cornerList.ToArray());

            }
        }
        if (cornerList.Count > 0)//add the final point to the point list
        {
            finalPoint = cornerList.Last();
        }
        return finalPoint;
    }
}
