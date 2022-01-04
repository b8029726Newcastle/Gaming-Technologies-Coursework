using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //to acess Text
using UnityEngine.SceneManagement; //to access Scene Manager

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float timeValue = 180; //3 minutes or 180 seconds
    public bool levelComplete = false;

    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>(); //access other script as an instance
    }

    // Update is called once per frame
    void Update()
    {
        //stop timer if the player has completed the level, otherwise keep counting down
        if (levelComplete)
            return;

        if(timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
        }
        if(timeValue <= 0)
        {
            //ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Reload current scene if timer reaches 0
            SceneManager.LoadScene(player.currentScene.name);
            Debug.Log("Game Over: You ran out of time! Reloading current scene/level: " + player.currentScene.name);
        }

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0; 
        }
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); //divide by 60 seconds
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); //modulo 60

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); //time format in "minutes:seconds"
    }
}
