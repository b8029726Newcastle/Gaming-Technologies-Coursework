using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//create abstract class
public abstract class FilteredFlockBehaviour : FlockBehaviour //inherit from FlockBehaviour
{
    //APPROPRIATE DESIGN PATTERNS AND GAME STRUCTURE: Create abstract method to be overridden in other FLOCK classes
    //(AlignmentBehaviour, AvoidanceBehaviour, CohesionBehaviour, and SteeredCohesionBehaviour Class)
    public ContextFilter filter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
