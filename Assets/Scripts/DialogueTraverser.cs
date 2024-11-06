using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTraverser
{
    //FIELDS
    //objects
    [SerializeField] DialogueGraph graph;
    public DialogueNode currentNode;
    /*
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject parentOfButton;
    */

    //Other values
    /*
    [SerializeField] private Vector2 buttonCenter;
    */

    /// <summary>
    /// Creates a new DialogueGraph
    /// </summary>
    /// <param name="graph"></param>
    public DialogueTraverser ()
    {
    }

    /// <summary>
    /// Given a graph in a gameobject, sets it as the current graph and
    /// sets the graph's starting node to be the current node
    /// </summary>
    /// <param name="graph"></param>
    public void SetNewGraph(DialogueGraph graph)
    {
        this.graph = graph;
        currentNode = graph.StartNode;
        if (graph.WasTraversed == false)
        {
            graph.WasTraversed = true;
        }
        else
        {
            graph.ReassignStart();
        }
    }

    /// <summary>
    /// Sets currentNode to a new node based on player input
    /// </summary>
    /// <param name="choice"></param>
    public void GoToNode(int choice)
    {
        currentNode = currentNode.Links[choice];
    }

    /// <summary>
    /// Checks if the destination node has a conditional that would stop traversal
    /// </summary>
    /// <param name="destinationNode">Node that is to be traveled to</param>
    /// <param name="flags">List of all possible conditionals in the scene</param>
    /// <returns>True if all conditionals are met, false otheriwse</returns>
    public bool CheckTraversal(DialogueNode destinationNode, List<DialogueFlag> flags)
    {
        List<DialogueFlag> matches = new List<DialogueFlag>();

        // First, find every conditional in the scene's list of dialogueFlags that corresponds
        // to a conditional in the distination node
        foreach (DialogueFlag destinationFlag in destinationNode.Flags) 
        {
            foreach (DialogueFlag flag in flags) 
            {
                if (flag.Name == destinationFlag.Name)
                { 
                    matches.Add(flag);
                }
            }
        }

        // Then, check if any of these nodes are flase. Return false if so.
        foreach (DialogueFlag flag in matches)
        {
            if (!flag.IsTrue) { return false; }
        }

        // Otherwise return true
        return true;
    }

    /*
    /// <summary>
    /// Sets up the first dialogue options in the encounter
    /// </summary>
    public void StartDialogue()
    {
        //Get the number of buttons that we need to spawn
        int numOfButtons = graph.StartNode.Links.Count;
        float startingY = buttonCenter.y - numOfButtons * 100 / 2;

        //Instantiate them
        for(int i = 0; i < numOfButtons; i++)
        {
            GameObject button = Instantiate(
                buttonPrefab, 
                new Vector2(0, startingY + 100 * i), 
                Quaternion.identity, 
                parentOfButton.transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, startingY + 100 * i);
        }
    }
    */
}
