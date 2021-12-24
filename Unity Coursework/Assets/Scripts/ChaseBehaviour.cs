using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    private Transform player;
    public float speedAI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector3.MoveTowards(animator.transform.position, player.position, speedAI * Time.deltaTime);
        animator.transform.LookAt(player.transform);
        var distance = Vector3.Distance(animator.transform.position, player.position);
        Debug.Log($"Distance is {distance}");
        if (distance < 2)
        {
            animator.SetBool("isInRange", true);
            distance = Vector3.Distance(animator.transform.position, player.position);
        }
        if (distance > 2)
        {
            animator.SetBool("isInRange", false);
        }
        if (distance <= 5)
        {
            //activate pursuit mode to catch up to the player
            animator.SetBool("isTooFar", false);
        }
        if (distance > 20)
        {
            //activate pursuit mode to catch up to the player
            animator.SetBool("isTooFar", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
