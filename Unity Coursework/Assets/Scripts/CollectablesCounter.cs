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

    // Start is called before the first frame update
    void Start()
    {
        obstacle = FindObjectOfType<ObstaclePush>();
        count = 0;
        //totalCount = 63;
        totalCount = GameObject.FindGameObjectsWithTag("Ring").Length;
        sceneName = SceneManager.GetActiveScene().name;
    }


    // Update is called once per frame
    void Update()
    {
        collectablesCounterText.GetComponent<Text>().text = $"Collectibles: {count}/{totalCount}"; //interpolated string to substitute values depending on count
        if (count == totalCount) // && allLevels != completed
        {
            if (sceneName == "Level1")
            {
                //once all collectibles have been collected, show door  to next level
                //obstacle.door.SetActive(true); //commented this out for now because I disable "ObstaclePush class" on other Game objects, doesn't seem like I need it anyway
                Debug.Log("Level Complete!");
                SceneManager.LoadScene("Level2");
            }
            else if (sceneName == "Level2")
            {
                //once all collectibles have been collected, show door  to next level
                //obstacle.door.SetActive(true); //commented this out for now because I disable "ObstaclePush class" on other Game objects, doesn't seem like I need it anyway
                Debug.Log("Level Complete!");
                SceneManager.LoadScene("Level1");
            }
        }
        //else if (count == totalCount) // && allLevels == completed //load main menu or level selector
    }


}
