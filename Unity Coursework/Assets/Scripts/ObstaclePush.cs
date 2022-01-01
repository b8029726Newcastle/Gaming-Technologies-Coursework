using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaclePush : MonoBehaviour
{
    [SerializeField]
    float forceMagnitude;

    PlayerMovement player;
    public GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>(); //access other script as an instance

        door = GameObject.FindGameObjectWithTag("DoorRight1");

        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Scene name is " + sceneName);
        if(sceneName == "Level2")
        {
            door.SetActive(false); //set it to TRUE when all RINGS have been COLLECTED --- Player will then pass  through this door to get to the next levll
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //function gets called whenever the character controller collides with anything
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if(rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position; //position of character minus position  of obstacle
            forceDirection.y = 0;
            forceDirection.Normalize();

            //add force to obstacles
            //this makes the  spikeballs rollable to an extent
            //as a result, forces players to be more careful as to not "continually push" a spikeball
            //otherwise they would incur continuous damage!
            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spikeball 1"))
        {
            /*players receives 3 damage on initial collision
             * players take more damage if collision lingers
             * (i.e. Some obstacles are designed to be movable and a player accidentally pushing an obstacle may cause them damage over time!)*/
            player.TakeDamageOverTime(3, 1);
            FindObjectOfType<AudioManager>().Play("Slice Damage"); //play accompanying audio when player takes damage
        }
        if (collision.gameObject.CompareTag("Spikeball 2"))
        {
            /*players receives 5 damage on initial collision
             * players take more damage if collision lingers
             * (i.e. Some obstacles are designed to be movable and a player accidentally pushing an obstacle may cause them damage over time!)*/
            player.TakeDamageOverTime(5, 1);
            FindObjectOfType<AudioManager>().Play("Slice Damage"); //play accompanying audio when player takes damage
        }

        if (collision.gameObject.CompareTag("Enemy Titan"))
        {
            //maybe remove this because it's already on the player movement for Character to take damage
            Debug.Log($"Colliding with {gameObject.tag} , you are taking initial 20collision damage!");
            /*players receives 3 damage on initial collision
             * players take more damage if collision lingers
             * (i.e. Some obstacles are designed to be movable and a player accidentally pushing an obstacle may cause them damage over time!)*/
            player.TakeDamage(5);
            FindObjectOfType<AudioManager>().Play("Blunt Damage");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoorLeft1")) //was PLAYER  --DELETE COMMENT
        {
            SceneManager.LoadScene("Level1");
        }

        if (other.gameObject.CompareTag("DoorRight1"))
        {
            SceneManager.LoadScene("Level2");
        }
    }
}
