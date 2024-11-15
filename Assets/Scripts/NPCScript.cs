using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCScript : Interactable
{
    public string filePath;
    public DialogueGraph Graph { get; private set; }
    public UnityEvent<NPCScript> UpdateSceneGraph { get; private set; }


    private void Awake()
    {
        Graph = new DialogueGraph(filePath);
        UpdateSceneGraph = new UnityEvent<NPCScript>();
        Debug.Log(gameObject.name + " " + UpdateSceneGraph);
    }    

    public override void Interact()
    {
        base.Interact();
        // Event to call method in sceneManager that updates currentGraph upon interaction
        UpdateSceneGraph.Invoke(this);        
    }
}
