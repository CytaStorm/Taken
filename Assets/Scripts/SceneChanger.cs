using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public bool hasTimeLimit = true;
    public float timeLimit;
    public string destinationScene;

    // Start is called before the first frame update
    void Start()
    {
        if (timeLimit == null)
        {
            hasTimeLimit = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateTimer() 
    {

    }

    void LoadNextScene()
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
