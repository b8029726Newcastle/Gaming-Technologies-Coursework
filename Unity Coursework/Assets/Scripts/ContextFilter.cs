using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//create abstract class
public abstract class ContextFilter : ScriptableObject //inherit from scriptable object
{
    //APPROPRIATE DESIGN PATTERNS AND GAME STRUCTURE: Create abstract method to be overridden in other FLOCK classes (SameFlockFilter class)
    public abstract List<Transform> Filter(FlockAgent agent, List<Transform> original);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
