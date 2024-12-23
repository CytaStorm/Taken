using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    //public string destinationScene;

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
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentScene.buildIndex + 1);
            Debug.Log("changed scene");
        }         
    }

}
