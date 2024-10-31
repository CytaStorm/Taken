using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueGraph graph;

    void Start()
    {
        TestDialogueGraph();
        //graph.PrintNodes();
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
