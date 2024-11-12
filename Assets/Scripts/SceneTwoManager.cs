using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneTwoManager : MonoBehaviour
{
    public List<DialogueFlag> dialogueFlags;
    [SerializeField] public List<GameObject> NPCs;
    public DialogueTraverser traverser;     
    private DialogueGraph currentGraph;

    // Start is called before the first frame update
    void Start()
    {
        dialogueFlags = new List<DialogueFlag>();
        traverser = new DialogueTraverser();

        // Adds sceneManager as a listner to every npc's UpdateSceneGraph event
        foreach (GameObject npc in NPCs) 
        {
            NPCScript npcScript = npc.GetComponent<NPCScript>();
            npcScript.UpdateSceneGraph.AddListener(UpdateCurrentGraph);            
        }

        CreateAllDialogueFlags();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// Populates list of dialogue flags with every flag in every dialogue graph
    /// in the scene
    /// </summary>
    private void CreateAllDialogueFlags()
    {
        // NOTE: there's way too much iteration in this, might try optimizing later

        foreach (GameObject npc in NPCs) 
        { 
            NPCScript npcScript = npc.GetComponent<NPCScript>();
            DialogueGraph graph = npcScript.Graph;

            if (graph == null) { break; } // crash prevention

            // For every dialogueFlag in every node, add to the sceneManager's list
            // if it isn't already there
            foreach (DialogueNode node in graph.Nodes)
            {
                foreach (DialogueFlag flag in node.Flags)
                {
                    if (!dialogueFlags.Contains(flag))
                    {
                        dialogueFlags.Add(flag);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Advance node according to choice
    /// </summary>
    /// <param name="choice">Index of destinationNode in current node</param>
    public void GoToNode(int choice)
    {
        traverser.GoToNode(choice, dialogueFlags);
    }

    /// <summary>
    /// Update the currentGraph variable based on the npcScript the sceneManager gets an
    /// UpdateSceneGraph call from
    /// </summary>
    /// <param name="npcScript">Script that the sceneManager recieved an event call from</param>
    private void UpdateCurrentGraph(NPCScript npcScript)
    {
        currentGraph = npcScript.Graph;
        traverser.SetNewGraph(currentGraph);
        Debug.Log("Updated current graph");
    }
    
}
