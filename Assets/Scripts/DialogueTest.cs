using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueGraph graph;
    [SerializeField] private List<DialogueFlag> dialogueFlags;

    void Start()
    {
        graph = new DialogueGraph();
        CreateAllDialogueFlags();
        DialogueTraverser traverser = new DialogueTraverser();

        traverser.SetNewGraph(graph);
        Debug.Log(traverser.currentNode);
        traverser.GoToNode(0, dialogueFlags);
        Debug.Log(traverser.currentNode);
        traverser.GoToNode(0, dialogueFlags);
        Debug.Log(traverser.currentNode);
    }

    void TestDialogueParsing()
    {
        string filePath = Application.dataPath + "/Scripts/Story.twee";
        DialogueTreeParser.ParseFile(filePath);
    }

    void TestDialogueGraph()
    {
        graph = new DialogueGraph();
    }

    /// <summary>
    /// Populates list of dialogue flags with every flag in every dialogue graph
    /// in the scene
    /// </summary>
    private void CreateAllDialogueFlags()
    {
        dialogueFlags = new List<DialogueFlag>();

        // For every dialogueFlag in every node, add to the dialogueTester's list
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
