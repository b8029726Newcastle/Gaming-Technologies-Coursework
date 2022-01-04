using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //load overworld where player can choose between 2 doors that lead into a game level (like a portal)
        SceneManager.LoadScene("Level Select");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //UnityEditor.EditorApplication.isPaused = true; //maybe add for future iterations when user presses "Esc" for example
        Application.Quit();
    }
}
