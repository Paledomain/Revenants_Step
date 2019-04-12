using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerBehav : MonoBehaviour {//cut in size a lot and trans

    //properties
    //public RuntimeAnimatorController stealthController;
    //public RuntimeAnimatorController combatController;

    public Camera cam;
    public NavMeshAgent agent;

    public GameObject SwordR;
    public GameObject SwordL;
    public GameObject SwordRS;
    public GameObject SwordLS;
    public GameObject Dagger;
    public GameObject DaggerS;

    //public Vector3 newHeading;
    //public Vector3 target;
    //public float proximityBias = 3.5f;

    public ThirdPersonCharacter character;

    // Use this for initialization
    void Start ()
    {
        agent.updateRotation = false;

        //newHeading = new Vector3(0, 0, 0);
       //target = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //cast ray
        /*RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);*/

        //check if movable
        /*if (Vector3.Distance(target, transform.position) < proximityBias)
        {
            move(0, 0);
        }*/

        //check game stealth state
        /*if (GameManager.gm.stealth > 0)
        {
            animator.runtimeAnimatorController = stealthController;
            SwordR.SetActive(false);
            SwordL.SetActive(false);
            Dagger.SetActive(true);

            SwordRS.SetActive(true);
            SwordLS.SetActive(true);
            DaggerS.SetActive(false);
        }
        else
        {
            animator.runtimeAnimatorController = combatController;
            SwordR.SetActive(true);
            SwordL.SetActive(true);
            Dagger.SetActive(false);

            SwordRS.SetActive(false);
            SwordLS.SetActive(false);
            DaggerS.SetActive(true);
        }*/

        //move if movable, attack if enemy contact
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }


            /*if (Physics.Raycast(ray, out hit))
            {
                target = hit.point;

                newHeading = Vector3.Normalize(hit.point - transform.position);
                
                move(newHeading.x, newHeading.z);

                if (hit.collider.tag == "Enemy")
                {
                    if (Vector3.Distance(target, transform.position) < proximityBias)
                    {
                        attack();
                        //die();
                    }
                }
            }*/

        }

    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Terrain")
        {
            move(0, 0);
            Debug.Log(other.gameObject.name);
        }
        
    }*/

}
