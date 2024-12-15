using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    public string destinationScene;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNextScene()
    {
        // Change current scene to that with the specified name
        if (destinationScene != null)
        {
            SceneManager.LoadScene(destinationScene);
        }
        // Otherwise, set to next scene in build settings
        else
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.buildIndex + 1 < SceneManager.sceneCount)
            {
                SceneManager.LoadScene(currentScene.buildIndex + 1);
            }            
        }
    }

}
