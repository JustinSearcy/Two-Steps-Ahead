using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int currentBuildIndex = 0;

    void Start()
    {
        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(currentBuildIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(currentBuildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
