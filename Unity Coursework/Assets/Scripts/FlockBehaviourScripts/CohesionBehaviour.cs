using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scriptable object
[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")] //new menu items can be seen after clicking on assets then "right" click > create
public class CohesionBehaviour : FilteredFlockBehaviour //inherit from FilteredFlockBehaviour Class
{
    //implement abstract class
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbours, return no adjustment
        if(context.Count == 0)
            return Vector3.zero;

        //add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //new
        foreach (Transform item in filteredContext)
        {
            //move towards position of neighbour
            cohesionMove += item.position;
        }

        //calculate average
        cohesionMove /= context.Count;

        //create offset from agent position
        cohesionMove -= agent.transform.position;

        return cohesionMove;
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
