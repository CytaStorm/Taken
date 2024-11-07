using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTwoManager : MonoBehaviour
{
    [SerializeField] public List<DialogueFlag> dialogueFlags;
    [SerializeField] public List<GameObject> NPCs;
    public DialogueTraverser traverser;     
    private DialogueGraph currentGraph;

    // Start is called before the first frame update
    void Start()
    {
        traverser = new DialogueTraverser();
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
    
}
