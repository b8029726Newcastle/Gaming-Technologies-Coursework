using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scriptable object
[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")] //new menu items can be seen after clicking on assets then "right" click > create
public class AvoidanceBehaviour : FilteredFlockBehaviour //inherit from FilteredFlockBehaviour Class
{
    //ADVANCED REAL-TIME AI TECHNIQUES: Flocking Avoidance Behaviour

    //APPROPRIATE DESIGN PATTERNS AND GAME STRUCTURE: Overriding an Inherited Method from FilteredFlockBehaviour Class which also Inherited from FlockBehaviour Class
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbours, return no adjustment
        if (context.Count == 0)
            return Vector3.zero;

        //add all points together and average
        Vector3 avoidanceMove = Vector3.zero;

        //count how many are in avoidance radius
        int countAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //new
        foreach (Transform item in filteredContext)
        {
            //get square distance between item and agent
            if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                countAvoid++;

                //move away from neighbour
                //also calculates offset from agent position
                avoidanceMove += agent.transform.position - item.position;
            }            
        }

        //calculate average
        if (countAvoid > 0)
            avoidanceMove /= countAvoid;

        return avoidanceMove;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
