using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueGraph graph;

    void Start()
    {
        TestDialogueGraph();
        DialogueTraverser traverser = new DialogueTraverser();
        traverser.SetNewGraph(graph);
        Debug.Log(traverser.currentNode);
        traverser.GoToNode(0);
        Debug.Log(traverser.currentNode);
        traverser.GoToNode(0);
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
}
