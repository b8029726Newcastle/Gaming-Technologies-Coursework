using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaclePush : MonoBehaviour
{
    [SerializeField]
    float forceMagnitude;

    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>(); //access other script as an instance
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) //function gets called whenever the character controller collides with anything
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if(rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position; //position of character minus position of obstacle
            forceDirection.y = 0;
            forceDirection.Normalize();

            /* Add force to obstacles.
             * This makes the spikeballs rollable/pushable to an extent.
             * As a result, forces players to be more careful as to not "continually push" a spikeball.
             * Otherwise they would incur continuous damage!
             */
            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spikeball 1"))
        {
            /* Player receives 3 damage on initial collision with the spikes.
             * Player takes more damage if collision lingers.
             * (i.e. Some obstacles are designed to be movable and a player accidentally pushing on a spikeball may cause them damage over time!)
             */
            player.TakeDamageOverTime(3, 1);

            //AUDIO: Play accompanying audio when player takes damage
            FindObjectOfType<AudioManager>().Play("Slice Damage");

            Debug.Log($"Colliding with {collision.gameObject.tag} , you are taking 3 collision damage!");
        }
        if (collision.gameObject.CompareTag("Spikeball 2"))
        {
            /* Player receives 5 damage on initial collision with the spikes.
             * Player takes more damage if collision lingers.
             * (i.e. Some obstacles are designed to be movable and a player accidentally pushing on a spikeball may cause them damage over time!)
             */
            player.TakeDamageOverTime(5, 1);

            //AUDIO: Play accompanying audio when player takes damage
            FindObjectOfType<AudioManager>().Play("Slice Damage");

            Debug.Log($"Colliding with {collision.gameObject.tag} , you are taking 5 collision damage!");
        }

    }
}
