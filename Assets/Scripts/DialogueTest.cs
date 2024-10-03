using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    void Start()
    {
        TestDialogueGraph();
    }

    void TestDialogueParsing()
    {
        string filePath = Application.dataPath + "/Scripts/Story.twee";
        DialogueTreeParser.ParseFile(filePath);
    }

    void TestDialogueGraph()
    {

        DialogueGraph graph = new DialogueGraph("/Scripts/Story.twee");
    }
}
