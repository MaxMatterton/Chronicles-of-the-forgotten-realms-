using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
   public void Reload()
    {
        int currentscene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentscene);
    }

    public void Mainscene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextScene()
    {
        int currentscene = SceneManager.GetActiveScene().buildIndex;
        int nextscene = currentscene + 1;
        SceneManager.LoadScene(nextscene);
    }
}
