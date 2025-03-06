using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public void Play()
    {
        // Play the scene
        SceneManager.LoadScene(1);
    }
    public void Replay () 
    {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(CurrentScene);
    }
    public void NextLevel () 
    {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        int NextScene = CurrentScene+1;
        SceneManager.LoadScene(NextScene);
    }

    public void MainMenu () {
        SceneManager.LoadScene(0);
    }
}
