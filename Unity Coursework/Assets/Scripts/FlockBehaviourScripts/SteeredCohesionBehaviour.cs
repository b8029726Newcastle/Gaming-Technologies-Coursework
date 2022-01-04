using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scriptable object
[CreateAssetMenu(menuName = "Flock/Behaviour/Steered Cohesion")] //new menu items can be seen after clicking on assets then "right" click > create
public class SteeredCohesionBehaviour : FilteredFlockBehaviour //inherit from FilteredFlockBehaviour Class
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;

    //ADVANCED REAL-TIME AI TECHNIQUES: Flocking SteeredCohesion Behaviour

    //APPROPRIATE DESIGN PATTERNS AND GAME STRUCTURE: Overriding an Inherited Method from FilteredFlockBehaviour Class which also Inherited from FlockBehaviour Class
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbours, return no adjustment
        if (context.Count == 0)
            return Vector3.zero;

        //add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            //move towards position of neighbour
            cohesionMove += item.position;
        }

        //calculate average
        cohesionMove /= context.Count;

        //create offset from agent position
        cohesionMove -= agent.transform.position;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);

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
