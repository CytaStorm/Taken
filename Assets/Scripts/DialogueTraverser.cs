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

    public void SetNewGraph(DialogueGraph graph)
    {
        this.graph = graph;
        currentNode = graph.StartNode;
    }

    /// <summary>
    /// Sets currentNode to a new node based on player input
    /// </summary>
    /// <param name="choice"></param>
    public void GoToNode(int choice)
    {
        currentNode = currentNode.Links[choice];
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
