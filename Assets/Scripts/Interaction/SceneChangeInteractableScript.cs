using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeInteractableScript : InteractableScript
{
    [SerializeField] private string changeToSceneName;

    //protected override void Update()
    //{

    //}
    public override void Interact()
    {
        SceneManager.LoadScene(changeToSceneName);
    }
}
