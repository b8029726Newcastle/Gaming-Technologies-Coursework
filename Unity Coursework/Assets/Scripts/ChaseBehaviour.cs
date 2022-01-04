using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //for NavMeshAgent

public class ChaseBehaviour : StateMachineBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    public float speedAI;

    CollectablesCounter ringCount;
    PlayerMovement player;
    Timer timer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = FindObjectOfType<NavMeshAgent>();

        player = FindObjectOfType<PlayerMovement>(); //access other script as an instance
        ringCount = FindObjectOfType<CollectablesCounter>();
        timer = FindObjectOfType<Timer>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /* ADVANCED PATHFINDING: Set player's position as AI destination for pathfinding
         * ADVANCED PATHFINDING: AI moves towards players general direction and consequently, automatically chooses the shortest path to the player
         * ADVANCED PATHFINDING: Use NavMesh so Titan AI / NavmeshAgent can move around obstacles such as the spikeballs on the map
         * Obstacles are set to "Navigation Static"
         */
        navMeshAgent.speed = speedAI; //set in animator's inspector for Chase, Pursuit Mode, and Rage Mode State
        navMeshAgent.SetDestination(playerTransform.position);
        animator.transform.LookAt(playerTransform.transform); //set where Enemy Titan AI is looking at


        var distance = Vector3.Distance(animator.transform.position, playerTransform.position);
        //Debug.Log($"Distance is {distance}"); //uncomment to see distance between player and AI in real time

        //ADVANCED REAL-TIME AI TECHNIQUES: Goal Oriented Action Programming (GOAP)
        if (distance < 1.9)
        {
            //ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Enemy Titan AI tries to attack when the player is close enough.
            animator.SetBool("isInRange", true);

            //AUDIO: Play accompanying sound clip
            FindObjectOfType<AudioManager>().Play("Roar");
        }
        if (distance >= 2)
        {
            //ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Enemy Titan AI is not close enough to attack,
            //instead continues to chase player depending on appropriate chase behaviour (normal chase or rage mode)
            animator.SetBool("isInRange", false);
        }
        if (distance <= 3.5)
        {
            /* ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Set back to normal Chase Mode (or Rage Mode depending on situation) when AI 
             * is not in range to attack but not too far to activate Pursuit Mode.
             * (typically disables the Pursuit Mode when Enemy Titan AI closes the gap after falling behind)
             */
            animator.SetBool("isTooFar", false);

            //AUDIO: Play accompanying sound clip
            FindObjectOfType<AudioManager>().Play("Growl");
        }
        if (distance > 20)
        {
            //ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Activate Pursuit Mode to catch up to the player when Enemy Titan AI falls too far behind.
            //For instance, when the player gets way ahead of the AI after a Speed Boost.
            animator.SetBool("isTooFar", true);

            //AUDIO: Play accompanying sound clip
            FindObjectOfType<AudioManager>().Play("Distant Growl");

            Debug.Log("Enemy Titan AI: Pursuit Mode Activated!");
        }
        if(timer.timeValue <= 30 || CollectablesCounter.count >= 54 || player.currentHealth < 15)
        {
            /* ADVANCED GAMEPLAY PROGRESSION TECHNIQUES:
             * Activate Rage Mode when (a) there is only 30 seconds left
             * or (b) when there are only 10 rings left (54/64 rings collected)
             * or (c) if player has less than 15 health
             */
            animator.SetBool("isEnraged", true);

            Debug.Log("Enemy Titan AI: Rage Mode Activated!");

            if (CollectablesCounter.count >= 54)
            {
                Debug.Log($"Ring Collectables left: {CollectablesCounter.count}");
            }
            if (player.currentHealth < 15)
            {
                Debug.Log($"Health is low, Current Health: {player.currentHealth}");
            }

        }
        if (timer.timeValue > 30 && CollectablesCounter.count < 54 && player.currentHealth >= 15)
        {
            /* ADVANCED GAMEPLAY PROGRESSION TECHNIQUES:
             * Deactivate Rage Mode if (a) there's more than 30 seconds left (maybe player picked up a "Time Dilation" powerup to 
             * increase 5 seconds to the current timer's value) as long as player hasn't collected 54/64 Rings yet 
             * and WHILE player has greater than 15 health.
             * 
             * Deactivate Rage Mode if (b) player has at least 15 health or greater (maybe player picked up a "Health Boost" powerup to increase current health)
             * as long as player hasn't collected 54/64 Rings yet and whiLE there's more than 30 seconds left.
             */
            animator.SetBool("isEnraged", false);
            Debug.Log("Enemy Titan AI: Rage Mode Deactivated!");

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
