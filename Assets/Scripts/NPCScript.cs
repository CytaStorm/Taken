using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCScript : Interactable
{
    public string filePath;
    public DialogueGraph Graph { get; private set; }
    public UnityEvent<NPCScript> UpdateSceneGraph { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Graph = new DialogueGraph(filePath);
        UpdateSceneGraph = new UnityEvent<NPCScript>();
    }

    public override void Interact()
    {
        base.Interact();
        // Event to call method in sceneManager that updates currentGraph upon interaction
        UpdateSceneGraph.Invoke(this);        
    }
}
