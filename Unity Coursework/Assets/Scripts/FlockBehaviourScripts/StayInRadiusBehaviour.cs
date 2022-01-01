using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scriptable object
[CreateAssetMenu(menuName = "Flock/Behaviour/Stay in Radius")] //new menu items can be seen after clicking on assets then "right" click > create
public class StayInRadiusBehaviour : FlockBehaviour //inherit from FlockBehaviour Class
{
    public Vector3 centre;
    public float radius = 15f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //see how far away agent is from the centre
        //based on where that position is, it may get back to the centre
        Vector3 centreOffset = centre - agent.transform.position;
        float t = centreOffset.magnitude / radius; //if t = 0 then it's at centre, if 1  then at radius

        //check how close to radius
        if(t < 0.9f)
        {
            return Vector3.zero;
        }
        //square the 't' for a more quadratic effect
        return centreOffset * t * t;
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
