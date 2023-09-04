using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SceneLoadertest(int SceneIndex)
        {
            SceneManager.LoadScene(SceneIndex);
        }
    public void QuitGame()
        {
            Application.Quit();
        }
}
