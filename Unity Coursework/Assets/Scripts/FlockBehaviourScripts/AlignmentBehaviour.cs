using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scriptable object
[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")] //new menu items can be seen after clicking on assets then "right" click > create
public class AlignmentBehaviour : FilteredFlockBehaviour //inherit from //new menu items can be seen after clicking on assets then "right" click > create Class
{
    //implement abstract class
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbours, maintain current alignment
        if (context.Count == 0)
            return agent.transform.forward; //maybe agent.transform.up

        //add all points together and average
        Vector3 alignmentMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //new
        foreach (Transform item in filteredContext)
        {
            alignmentMove += item.transform.forward; //item.transform.up
        }
        alignmentMove /= context.Count;

        return alignmentMove;
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
