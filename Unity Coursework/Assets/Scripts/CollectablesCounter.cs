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

    // Start is called before the first frame update
    void Start()
    {
        obstacle = FindObjectOfType<ObstaclePush>();
        count = 0;
        totalCount = 63;
    }


    // Update is called once per frame
    void Update()
    {
        collectablesCounterText.GetComponent<Text>().text = $"Collectibles: {count}/{totalCount}"; //interpolated string to substitute values depending on count
        if (count == totalCount)
        {
            //once all collectibles have been collected, show door  to next level
            obstacle.door.SetActive(true);
            Debug.Log("Level Complete!");
            SceneManager.LoadScene("Level1");
        }
    }


}
