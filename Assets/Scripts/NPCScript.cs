using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCScript : Interactable
{
    public string filePath;
    public DialogueGraph Graph { get; private set; }
    public UnityEvent updateSceneGraph;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Graph = new DialogueGraph(filePath);
        updateSceneGraph = new UnityEvent();
    }

    public override void Interact()
    {
        base.Interact();
        // ADD UNITY EVENT HERE TO CALL METHOD IN SCENEMANAGER THAT UPDATES currentGraph
        updateSceneGraph.Invoke();        
    }
}
