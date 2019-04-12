
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour, ITurnBased
{
    //properties
    public Camera cam;

    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    public LineRenderer line;

    public GameObject SwordR;
    public GameObject SwordL;
    public GameObject SwordRS;
    public GameObject SwordLS;
    public GameObject Dagger;
    public GameObject DaggerS;

    public float APRange = 10;
    public int AP = 2;

    public float armor = 10;
    public float health = 100;

    public float attackRange = 2.0f;

    void Start()
    {
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool DoTurn()//when in combat
    {
        line.gameObject.SetActive(true);
        if(AP > 0)//only able to act while having action points
        {
            Vector3 movementPoint = transform.position;
            movementPoint = movementRange(APRange);//retract clicked point to movement range
            //works good because it is also visualized
            //Debug.Log(movementPoint);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0))//click
            {
                Debug.Log("clicked!");
                AP--;
                if (Physics.Raycast(ray, out hit))//if on enemy and in distance, hit, otherwise run there
                {
                    if (hit.collider.tag == "Enemy" && Vector3.Distance(character.transform.position, hit.collider.transform.position) < attackRange)
                    {
                        character.Attack();
                    }
                    else
                    {
                        agent.SetDestination(movementPoint);
                    }
                    
                }
            }
            //adjust movement animation according to stopping distance
            if (agent.remainingDistance > agent.stoppingDistance)
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

    public void StartTurn()//reset AP at the start of turn
    {
        AP = 2;
    }

    public void TransitionToCombat()//stop character and switch weapons upon entering combat
    {
        character.Stop();
        agent.SetDestination(transform.position);
        character.stealth(false);
        SwordR.SetActive(true);
        SwordL.SetActive(true);
        Dagger.SetActive(false);

        SwordRS.SetActive(false);
        SwordLS.SetActive(false);
        DaggerS.SetActive(true);

    }

    public void StartStealth()//upon entering stealth
    {

    }

    public void doStealth()//while doing stealth
    {
        line.gameObject.SetActive(false);
        character.stealth(true);//maybe put these in StartStealth() when going over the code again
        SwordR.SetActive(false);
        SwordL.SetActive(false);
        Dagger.SetActive(true);

        SwordRS.SetActive(true);
        SwordLS.SetActive(true);
        DaggerS.SetActive(false);

        //Debug.Log("Remain distance: " + agent.remainingDistance + "  stopping Distance: " + agent.stoppingDistance);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && !character.IsDead())
            {
                agent.SetDestination(hit.point);//go where clicked

                if (hit.collider.tag == "Enemy")//also attack if cliked area collides with an object tagged "Enemy"
                {
                    if (Vector3.Distance(character.transform.position, hit.collider.transform.position) < attackRange)
                    {
                        character.Move(Vector3.zero, false, false);
                        agent.SetDestination(transform.position);

                        character.Attack();//damage mechanics tbi...
                    }
                    //make the poor fucker hurt
                }
            }
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
    }

    public Vector3 movementRange(float maxDist)//calculate movement range
    {
        NavMeshPath path = new NavMeshPath();
        float dist = 0;
        List<Vector3> cornerList = new List<Vector3>();
        Vector3 finalPoint = transform.position;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && !character.IsDead())//same protocol as in the AI range calculations
        {
            if(NavMesh.CalculatePath(transform.position, hit.point, agent.areaMask, path))
            {
                cornerList.Add(transform.position);

                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    float distCalc = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                    dist += distCalc;

                    if (dist < maxDist)
                    {
                        cornerList.Add(path.corners[i + 1]);
                    }
                    else
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
        if(cornerList.Count > 0)
        {
            finalPoint = cornerList.Last();
        }
        return finalPoint;
    }

}
















