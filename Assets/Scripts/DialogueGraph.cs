using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.ComponentModel;

public class DialogueGraph : MonoBehaviour
{
    //FIELDS
    //Streamreader stuff
    [SerializeField] private StreamReader reader;

    [SerializeField, Description("Path after Assets/")] 
    private string localPath;
    string filePath;

    //Graph stuff
    List<DialogueNode> nodes;
    Dictionary<string, List<DialogueNode>> adjList;
    bool[,] adjMatrix = null;

    // Start is called before the first frame update
    void Start()
    {
        //For now, just load data on start
        LoadDialogue();

        //For now, instantiate hardcoded nodes and a hardcoded adjacency matrix

    }

    /// <summary>
    /// Load the dialogue from a set filepath from Unity
    /// </summary>
    void LoadDialogue()
    {
        //For now we are hardcoding nodes and adjacencies
        HardcodeAdjacencies();


        ////Define stream and streamreaders
        //filePath = Application.dataPath + localPath;
        //reader = new StreamReader(filePath);

        ////Peek will return -1 at end of the file
        //while(reader.Peek() >= 0)
        //{
        //    //Code reading logic here:
        //}
    }


    private void HardcodeAdjacencies()
    {
        //Hardcoded nodes - horrible, disgusting, blasphemous
        DialogueNode start = new DialogueNode("Start", "");
        DialogueNode suicide = new DialogueNode("Suicide", "Suicide");
        DialogueNode suicideEnd = new DialogueNode("Suicide end", "Suicide end");
        DialogueNode walk = new DialogueNode("Walk", "Walk");
        DialogueNode stopByFlowers = new DialogueNode("Stop by flowers", "Stop by flowers");
        DialogueNode seeAMountain = new DialogueNode("See a mountain", "See a mountain");
        DialogueNode climbTheMountain = new DialogueNode("Climb the mountain", "Climb the mountain");
        DialogueNode dontClimbMountain = new DialogueNode("Dont climb mountain", "Dont climb mountain");
        DialogueNode yap = new DialogueNode("Yap", "Yap");
        DialogueNode yapAboutFortnite = new DialogueNode("Yap about fortnite", "Yap about fortnite");
        DialogueNode yapAboutTalkTuah = new DialogueNode("Yap about talk tuah", "Yap about talk tuah");
        DialogueNode yapAboutAids = new DialogueNode("Yap about aids", "Yap about aids");
        DialogueNode experienceBliss = new DialogueNode("Experience bliss", "Experience bliss");

        //Add each node to the list of every node
        nodes = new List<DialogueNode>()
        {
            start,
            suicide,
            suicideEnd,
            walk,
            stopByFlowers,
            seeAMountain,
            climbTheMountain,
            dontClimbMountain,
            yap,
            yapAboutFortnite,
            yapAboutTalkTuah,
            yapAboutAids,
            experienceBliss
        };

        //Hardcode adjList (a dictionary) with string identifier being the "Data" property of each node
        adjList = new Dictionary<string, List<DialogueNode>>();
        adjList.Add(start.Data, new List<DialogueNode> { suicide, walk, yap});
        adjList.Add(suicide.Data, new List<DialogueNode> { suicideEnd });
        adjList.Add(suicideEnd.Data, new List<DialogueNode>());
        adjList.Add(walk.Data, new List<DialogueNode> { stopByFlowers, seeAMountain});
        adjList.Add(stopByFlowers.Data, new List<DialogueNode>());
        adjList.Add(seeAMountain.Data, new List<DialogueNode> { climbTheMountain, dontClimbMountain });
        adjList.Add(climbTheMountain.Data, new List<DialogueNode> ());
        adjList.Add(dontClimbMountain.Data, new List<DialogueNode> ());
        adjList.Add(yap.Data, new List<DialogueNode> { yapAboutFortnite, yapAboutTalkTuah, yapAboutAids });
        adjList.Add(yapAboutFortnite.Data, new List<DialogueNode> ());
        adjList.Add(yapAboutTalkTuah.Data, new List<DialogueNode> { experienceBliss });
        adjList.Add(yapAboutAids.Data, new List<DialogueNode>());
        adjList.Add(experienceBliss.Data, new List<DialogueNode> ());


    }
}
