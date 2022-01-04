using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//new menu items can be seen after clicking on assets then "right" click > create
[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")] 
public class SameFlockFilter : ContextFilter //inherit from context filter
{
    //ADVANCED REAL-TIME AI TECHNIQUES: Flocking

    //APPROPRIATE DESIGN PATTERNS AND GAME STRUCTURE: Overriding an Inherited Method from ContextFilter Class
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        
        //iterate through list whether it's a flock agent and a member of same flock
        foreach(Transform item in original)
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>();
            if(itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock)
            {
                filtered.Add(item);
            }
        }
        return filtered;
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
