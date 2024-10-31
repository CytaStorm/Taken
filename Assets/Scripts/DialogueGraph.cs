using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.ComponentModel;

public class DialogueGraph
{
    //FIELDS
    //Streamreader stuff
    [SerializeField] private StreamReader reader;

    [SerializeField, Description("Path after Assets/")] 
    private string localPath;
    private string filePath;

    //Graph stuff
    private List<DialogueNode> nodes;
    private DialogueNode start;
    private Dictionary<string, List<DialogueNode>> adjList;
    private bool[,] adjMatrix;

    // Properties
    public DialogueNode StartNode {  get { return start; } }

    //I DELETED THE CONSTRUCTOR

    // Start is called before the first frame update
    public DialogueGraph()
    {
        //Start by parsing shit, then construct itself with that data
        string filePath = Application.dataPath + "/Scripts/Story.twee";
        DialogueTreeParser.ParseFile(filePath);

        //Parse the jawns that used to be in the constructor
        //Parse(filePath);
    }

    private void Parse(string filePath)
    {
        nodes = DialogueTreeParser.ParseFile(filePath);

        // Find refernece to starting node
        foreach (DialogueNode node in nodes)
        {
            if (node.NodeName.Trim().ToLower().Equals("start"))
            {
                start = node;
            }
        }

        // In case there isn't a node named start, set start to the first
        // node in the list 
        if (start == null) { start = nodes[0]; }

        CreateAdjacencies();
    } 

    /// <summary>
    /// Create adjacency list and matrix
    /// </summary>
    private void CreateAdjacencies()
    {
        // Create adjacency list
        adjList = new Dictionary<string, List<DialogueNode>>();

        foreach (DialogueNode node in nodes) 
        { 
            // Create a list to store all adjacent nodes to current node
            List<DialogueNode> adjNodes = new List<DialogueNode>();
            // Iterate through current node's list of names to get references to all
            // adjacent nodes
            foreach (DialogueNode refNode in node.Links)
            {
                foreach (DialogueNode adjNode in nodes)
                {
                    if (adjNode.NodeName == refNode.NodeName) // could compare nodes themselves
                    { 
                        adjNodes.Add(adjNode);
                    }                    
                }
            }

            adjList.Add(node.NodeName, adjNodes);
        }

        // Create adjacency matrix
        adjMatrix = new bool[nodes.Count, nodes.Count];

        // For each node in the list, iterate through every other node. If the
        // second node is linked to the first, mark the corresponding element in
        // the adjacency matrix as true. Otherwise, mark it as false
        
        for (int i = 0; i < nodes.Count; i++)
        {
            DialogueNode node = nodes[i];
            
            for (int j = 0; j < nodes.Count; j++)
            {
                DialogueNode adjNode = nodes[j];
                if (adjList[node.NodeName].Contains(nodes[j]))
                {
                    adjMatrix[i,j] = true;
                }
                else
                {
                    adjMatrix[i, j] = false;
                }
            }
        }
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
