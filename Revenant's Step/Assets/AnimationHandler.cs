using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationHandler : MonoBehaviour {//used to call animations

    //properties
    public Animator animator;
    public float rotationSpeed = 1000;
    public Vector3 targetDirection;

    public void attack()//damage values should probably be added to somewhere else
    {
        animator.SetTrigger("Attack1Trigger");

        StartCoroutine(COStunPause(1.2f));//some parts of this code, such as this come from the free warriors asset
    }

    public void move(float speedx, float speedz)
    {
        //Apply inputs to animator
        animator.SetFloat("Input X", 0);
        animator.SetFloat("Input Z", 0);

        targetDirection = speedz * Vector3.forward + speedx * Vector3.right;

        /*if (speedx != 0 || speedz != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
        }*/

        if (speedx != 0 || speedz != 0)  //if there is some input
        {
            //set that character is moving
            animator.SetBool("Moving", true);
        }
        else
        {
            //character is not moving
            animator.SetBool("Moving", false);
        }

    }

    public void die()
    {
        animator.SetBool("Dead", true);
    }

    public IEnumerator COStunPause(float pauseTime)//pauses between animations
    {
        yield return new WaitForSeconds(pauseTime);
    }

}
