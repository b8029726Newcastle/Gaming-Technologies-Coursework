using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //for NavMeshAgent

public class ChaseBehaviour : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;
    public float speedAI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = FindObjectOfType<NavMeshAgent>(); //new
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //navMeshAgent.transform.position = Vector3.MoveTowards(movePositionTransform.position, player.position, speedAI * Time.deltaTime); //new
        //navMeshAgent.transform.position = (movePositionTransform.position * speedAI) *  Time.deltaTime; //new

        //navMeshAgent.transform.position = Vector3.MoveTowards(animator.transform.position, player.position, speedAI * Time.deltaTime); //new

        //set player's position as AI destination for pathfinding
        //AI moves towards players direction and consequently, automatically chooses the shortest path to the player
        //unless there are obstacles on the path
        //advanced pathfinding: Use NavMesh so Titan AI/Agent can move around obstacles such as the spikeballs on the map
        navMeshAgent.speed = speedAI;
        navMeshAgent.SetDestination(player.position);
        //navMeshAgent.transform.position = Vector3.MoveTowards(navMeshAgent.transform.position, player.position, speedAI * Time.deltaTime);

        //animator.transform.position = Vector3.MoveTowards(animator.transform.position, player.position, speedAI * Time.deltaTime);
        animator.transform.LookAt(player.transform);


        var distance = Vector3.Distance(animator.transform.position, player.position);
        //Debug.Log($"Distance is {distance}");

        //maybe make this into SWITCH STATEMENT
        if (distance < 2)
        {
            animator.SetBool("isInRange", true);
            distance = Vector3.Distance(animator.transform.position, player.position); //prob  don't need this line
            FindObjectOfType<AudioManager>().Play("Roar");
        }
        if (distance > 2)
        {
            animator.SetBool("isInRange", false);
        }
        if (distance <= 4) //was 5, see which is better
        {
            //set back to normal chasing speed
            animator.SetBool("isTooFar", false);
            FindObjectOfType<AudioManager>().Play("Growl");
        }
        if (distance > 20)
        {
            //activate pursuit mode to catch up to the player
            animator.SetBool("isTooFar", true);
            FindObjectOfType<AudioManager>().Play("Distant Growl");
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
