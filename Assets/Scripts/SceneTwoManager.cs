using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTwoManager : MonoBehaviour
{
    public bool autoImplementDialogue = false;

    public List<DialogueFlag> dialogueFlags;
    public List<GameObject> Interactables;
    public UIManager _UIManager;
    public DialogueTraverser traverser;     
    private DialogueGraph currentGraph;

    [Header("DEBUG")] public bool DEBUG;

    public bool IsDialogueAutoImplimented()
    {
        return autoImplementDialogue;
    }

    // Start is called before the first frame update
    void Start()
    {
		dialogueFlags = new List<DialogueFlag>();
        traverser = new DialogueTraverser(this, _UIManager);

        // Adds sceneManager as a listner to every npc's UpdateSceneGraph event
        foreach (GameObject npc in Interactables) 
        {
            InteractableScript npcScript = npc.GetComponent<InteractableScript>();
            npcScript.UpdateSceneGraph.AddListener(UpdateCurrentGraph);            
        }

        CreateAllDialogueFlags();

        // If dialogue is to be auto-implemented, set dialogue to that of first NPC
        if (autoImplementDialogue)
        {
            UpdateCurrentGraph(Interactables[0].GetComponent<InteractableScript>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DEBUG)
        {
		    foreach (DialogueFlag flag in dialogueFlags)
		    {
		    	print(flag);
		    }
        }
    }

    /// <summary>
    /// Populates list of dialogue flags with every flag in every dialogue graph
    /// in the scene
    /// </summary>
    private void CreateAllDialogueFlags()
    {
        // NOTE: there's way too much iteration in this, might try optimizing later

        foreach (GameObject interactable in Interactables) 
        { 
            InteractableScript interactScript = interactable.GetComponent<InteractableScript>();
            DialogueGraph graph = interactScript.Graph;

            if (graph == null) { break; } // crash prevention

            // For every dialogueFlag in every node, add to the sceneManager's list
            // if it isn't already there
            foreach (DialogueNode node in graph.Nodes)
            {
                foreach (DialogueFlag flag in node.Flags)
                {
                    if (!FlagListContainsMatch(flag))
                    {                        
                        dialogueFlags.Add(new DialogueFlag(flag.Name));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Advance node according to choice
    /// </summary>
    /// <param name="choice">Index of destinationNode in current node</param>
    public void GoToNode(int choice)
    {
		//print("go to node");
        traverser.GoToNode(choice);
    }

    public bool CheckTraversal(DialogueNode destinationNode)
    {
        return traverser.CheckTraversal(destinationNode, dialogueFlags);
    }

    /// <summary>
    /// Checks if there is already a flag in the scene's internal list with the same name
    /// </summary>
    /// <param name="flag">Flag to check against list</param>
    /// <returns></returns>
    private bool FlagListContainsMatch(DialogueFlag flag)
    {
        foreach(DialogueFlag listFlag in dialogueFlags)
        {
            if (flag.Name == listFlag.Name)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Update the currentGraph variable based on the npcScript the sceneManager gets an
    /// UpdateSceneGraph call from
    /// </summary>
    /// <param name="npcScript">Script that the sceneManager recieved an event call from</param>
    private void UpdateCurrentGraph(InteractableScript interactScript)
    {
        currentGraph = interactScript.Graph;
        traverser.SetNewGraph(currentGraph);

		//Send new node info to UI Manager
		_UIManager.NewDialogueNode(traverser.currentNode);
		//print(currentGraph.StartNode.Info);
    }

	public void ChangeDialogueFlag (DialogueFlag newFlag)
	{
		foreach (DialogueFlag flag in dialogueFlags)
		{
			if (flag.MatchName(newFlag))
			{
				flag.IsTrue = newFlag.IsTrue;
				//print(flag);
			}
		}	
	}

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
