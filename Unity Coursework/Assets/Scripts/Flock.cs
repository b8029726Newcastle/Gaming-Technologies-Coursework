using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(10, 500)]
    public int startingCount = 250;
    const float agentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;

    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    //radius for neighbour
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;

    //radius for avoidance
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed, squareNeighbourRadius, squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for(int i = 0; i < startingCount; i++)
        {
            //initialise  and instantiate the flock
            FlockAgent newAgent = Instantiate(agentPrefab, 
                Random.insideUnitSphere * startingCount * agentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f,  360f)),
                transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialise(this);
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //iterate through agents to find relevant neighbours and apply behaviours based on neighbour context
        foreach(FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            //calculate move based on agent and it's nearby objects
            Vector3 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;

            //check if maximum speed has been exceeded
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                //those that goes faster than the maxSpeed is reset to maxSpeed
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        //run physics overlap check
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighbourRadius);

        //iterate through colliders, take the transform attached to colliders apart from the current agent's collider
        foreach(Collider c in contextColliders)
        {
            if(c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context; //pass back to Update method
    }
}
