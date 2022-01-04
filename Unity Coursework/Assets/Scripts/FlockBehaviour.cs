using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//create abstract class
public abstract class FlockBehaviour : ScriptableObject //inherit from scriptable object
{
    //APPROPRIATE DESIGN PATTERNS AND GAME STRUCTURE:: Create abstract method to be overridden in other FLOCK classes
    //(FilteredFlockBehaviour, CompositeBehaviour, and StayInRadiusBehaviour Class)
    public abstract Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
