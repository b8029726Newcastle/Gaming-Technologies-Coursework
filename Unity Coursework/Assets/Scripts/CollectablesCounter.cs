using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //to access component "Text"
using UnityEngine.SceneManagement;

public class CollectablesCounter : MonoBehaviour
{
    public GameObject collectablesCounterText;
    public static int count, totalCount;
    ObstaclePush obstacle;
    string sceneName;

    //public GameObject door, otherDoor;

        /* Code that has been commented out was meant to be expanded upon as the original plan was to show a gateway/doorway/portal to 
         * the next level DYNAMICALLY at a small distance in front of the player whenever they collect all the rings.
         * 
         * However, this idea presented problems like what if the player decides to get all the rings in random order and leaves the last ones
         * in the mountain? This would mean that the door may spawn in front of the mountain, sure, but it might hover over the ground or
         * look like it went through it because the mountain is not exactly a flat surface.
         * Anyways, it's something I have careful given thought and decided it would be best to simply progress the player to the next level after
         * all Ring Collectibles have been acquired instead of making them pass through a door/portal after the fact.
         * 
         * I just left the code as I would like to expand upon it in the future if I can execute it correctly, in my own time perhaps.
         */

    // Start is called before the first frame update
    void Start()
    {
        obstacle = FindObjectOfType<ObstaclePush>();

        count = 0;
        totalCount = GameObject.FindGameObjectsWithTag("Ring").Length;

        sceneName = SceneManager.GetActiveScene().name;

        //door = GameObject.FindGameObjectWithTag("DoorRight1");
        //otherDoor = GameObject.FindGameObjectWithTag("DoorLeft1");
    }


    // Update is called once per frame
    void Update()
    {
        //interpolated string to substitute values depending on item count
        collectablesCounterText.GetComponent<Text>().text = $"Collectibles: {count}/{totalCount}"; 

        if (count == totalCount) // && allLevels != completed
        {
            //ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Reload next scene if player collects all RINGS.
            if (sceneName == "Level1")
            {
                //once all collectibles have been collected, show door to next level --- dynamically in front of player
                //door.SetActive(true);
                //otherDoor.SetActive(false); //maybe put this in Start method according to current scene

                //Load other level
                Debug.Log("Level 1 Complete!");
                SceneManager.LoadScene("Level2");
            }
            else if (sceneName == "Level2")
            {
                //once all collectibles have been collected, show door  to next level --- dynamically in front of player
                //door.SetActive(true);
                //otherDoor.SetActive(false); //maybe put this in Start method according to current scene

                //Load other level
                Debug.Log("Level 2 Complete!");
                SceneManager.LoadScene("Level1");
            }
        }
        //else if (count == totalCount) // && allLevels (BOOLEAN) == completed //load main menu or overworld level selector
        //ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Reload overworld/level select scene if player has completed both game levels.
    }

    //for future implementation
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoorLeft1"))
        {
            SceneManager.LoadScene("Level1");
        }

        if (other.gameObject.CompareTag("DoorRight1"))
        {
            SceneManager.LoadScene("Level2");
        }
    }*/


}
