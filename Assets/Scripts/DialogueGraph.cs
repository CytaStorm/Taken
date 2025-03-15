using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.ComponentModel;
public class DialogueGraph
{
    public string name;
    //FIELDS
    //Streamreader stuff
    [SerializeField] private StreamReader reader;

    [SerializeField, Description("Path after Assets/")] 
    private string localPath;

    //Graph stuff
    private List<DialogueNode> nodes;
    private DialogueNode start;

    // Properties
    public DialogueNode StartNode {  get { return start; } }
    public List<DialogueNode> Nodes {  get { return nodes; } }
    public bool WasTraversed { get; set; }
        
    public DialogueGraph(TextAsset textFile)
    {
        //Start by parsing shit, then construct itself with that data
        Parse(textFile);
    }

    private void Parse(TextAsset textFile)
    {
        nodes = TwineParser.ParseFile(textFile);        

        // Find refernece to starting node
        foreach (DialogueNode node in nodes)
        {
            if (node.NodeName.Trim().ToLower().Equals("intro"))
            {
                start = node;
                break;
            }
        }

        // In case there isn't a node named start, set start to the first
        // node in the list 
        if (start == null) { start = nodes[0]; }

        //CreateAdjacencies();
    } 

    public void ReassignStart()
    {
        DialogueNode newStart;

        // If there are no links, don't reassign start
        if (start.Links.Count == 0 || start.NodeName == "newIntro") { return; }

        // find the next viable start node by acessing the first start's link
        newStart = start.Links[0];

        // reassign start
        Nodes.Remove(start);
        newStart.NodeName = "newIntro";
        start = newStart;
    }

    /// <summary>
    /// Print out all nodes for debugging purposes
    /// </summary>
    //public void PrintNodes()
    //{
    //    for (int i = 0;i < nodes.Count;i++) 
    //    {
    //        print($"Index {i}:" + nodes[i].ToString());
    //    }
    //}
}
