using System.Collections.Generic;
using UnityEngine;

public class DialogueTraverser
{
    //FIELDS
    //objects
    public SceneController sceneManager;
    public UIManager _UIManager;
    public NewDialogueGraph CurrentGraph;
    public NewDialogueNode CurrentNode;
    /*
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject parentOfButton;
    */

    //Other values
    /*
    [SerializeField] private Vector2 buttonCenter;
    */

    /// <summary>
    /// Creates a new DialogueGraph traverser
    /// </summary>
    /// <param name="graph"></param>
    public DialogueTraverser (SceneController sceneManager, UIManager _UIManager)
    {
        this.sceneManager = sceneManager;
        this._UIManager = _UIManager;
    }

    /// <summary>
    /// Given a graph in a gameobject, sets it as the current graph and
    /// sets the graph's starting node to be the current node
    /// </summary>
    /// <param name="graph"></param>
    public void SetNewGraph(NewDialogueGraph graph)
    {
        //Reassign start node
        //this.graph = graph;
        //if (graph.traversed == false)
        //{
        //    graph.traversed = true;
        //}
        //else
        //{
        //    graph.ReassignStart();
        //}
        CurrentGraph = graph;
        CurrentNode = graph.StartNode;
		_UIManager.NewDialogueNode(CurrentNode);
		GoToSelf();
    }

	/// <summary>
	/// Used to check flags to change on starting node of graph.
	/// </summary>
	private void GoToSelf()
	{
		foreach (NewDialogueFlag newFlag in CurrentNode.FlagsToChange) 
		{
			sceneManager.ChangeDialogueFlag(newFlag);
		}
	}

    /// <summary>
    /// Sets currentNode to a new node based on player input. If choice is -1,
	/// go to current node.
    /// </summary>
    /// <param name="choice"></param>
    public void GoToNode(int choice)
    {
		CurrentNode = CurrentNode.Links[choice].ConnectedNode;

		foreach (NewDialogueFlag newFlag in CurrentNode.FlagsToChange) 
		{
			sceneManager.ChangeDialogueFlag(newFlag);
		}

        //Send currentNode info to UI Manager
		_UIManager.NewDialogueNode(CurrentNode);
    }

    /// <summary>
    /// Checks if the destination node has a conditional that would stop traversal
    /// </summary>
    /// <param name="destinationNode">Node that is to be traveled to</param>
    /// <param name="flags">List of all possible conditionals in the scene</param>
    /// <returns>True if all conditionals are met, false otheriwse</returns>
    public bool CheckTraversal(DialogueNode destinationNode, List<DialogueFlag> flags)
    {        
        // First, find every conditional in the scene's list of dialogueFlags that corresponds
        // to a conditional in the distination node
        // Then, check if any of these nodes don't match. Return false if so.
        foreach (DialogueFlag destinationFlag in destinationNode.Flags) 
        {
            foreach (DialogueFlag flag in flags) 
            {
                if (flag.MatchName(destinationFlag))
                { 
                    if (flag != destinationFlag)
                    {
                        // Print error message to console
                        //Debug.Log($"Unable to traverse because condition {flag.Name} wasn't met");

                        return false;                        
                    }
                }
            }
        }

        // Otherwise return true
        //Debug.Log("Successful traversal!");
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
