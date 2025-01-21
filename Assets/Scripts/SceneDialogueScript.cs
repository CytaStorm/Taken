using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDialogueScript : InteractableScript
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += AutoLoad;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AutoLoad(Scene scene, LoadSceneMode mode)
    {
        base.Interact();
    }
}
