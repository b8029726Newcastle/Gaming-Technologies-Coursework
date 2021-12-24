using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //maybe allow  user to pause by  pressing "ESC" key and bring up main menu or level select?
        SceneManager.LoadScene("Level Select");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //UnityEditor.EditorApplication.isPaused = true; //maybe add for future iterations
        Application.Quit();
    }
}
