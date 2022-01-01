using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scriptable object
[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")] //new menu items can be seen after clicking on assets then "right" click > create
public class CompositeBehaviour : FlockBehaviour //inherit from FlockBehaviour Class
{
    public FlockBehaviour[] behaviours;
    public float[] weights;

    //implement abstract class
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //handle data mismatch
        if(weights.Length != behaviours.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector3.zero;
        }

        //set up move
        Vector3 move = Vector3.zero;

        //iterate through behaviours
        for(int i = 0; i < behaviours.Length; i++)
        {
            Vector3 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];

            //just to confirm partial move is limited to extent of the weight
            if(partialMove != Vector3.zero)
            {
                if(partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    //normalise back to magnitude of 1 and multiply by weight so it's set to the maximum weight
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }
                move += partialMove;
            }
        }
        return move;    
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
