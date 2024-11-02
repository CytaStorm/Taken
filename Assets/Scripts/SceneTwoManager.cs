using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTwoManager : MonoBehaviour
{
    [SerializeField] public List<DialogueFlag> dialogueChecks;
    [SerializeField] public List<GameObject> NPCs;
    public DialogueTraverser traverser;     
    private DialogueGraph currentGraph;

    // Start is called before the first frame update
    void Start()
    {
        traverser = new DialogueTraverser();
        //CreateAllDialogueFlags();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    /// <summary>
    /// Populates list of dialogue flags with every flag in every dialogue graph
    /// in the scene
    /// </summary>
    private void CreateAllDialogueFlags()
    {
        foreach (GameObject npc in NPCs) 
        { 
            NPCScript npcScript = npc.GetComponent<NPCScript>();
            DialogueGraph graph = npcScript.Graph;

            if (graph == null) { break; } // crash prevention
            foreach(DialogueNode node in graph.Nodes)
            {

            }
        }
    }
    */
}
