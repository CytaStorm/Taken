using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public bool autoImplementDialogue = false;

	[Header("Dialogue Components")]
    public List<DialogueFlag> DialogueFlags;
    public List<GameObject> Interactables;
    public UIManager _UIManager;
    public DialogueTraverser Traverser;     
    private DialogueGraph _currentGraph;

	#region Scene Changing
	[Header("Scene Changing")]
    [SerializeField] string destinationScene;
    [SerializeField] float delayTime;
	public delegate IEnumerator OnSceneChangeHandler(float seconds);
	public event OnSceneChangeHandler onSceneChange;
    public bool sceneChangeActive = false;
    private float sceneChangeTimer = 0f;
	#endregion

	[SerializeField] private Material _sallosMaterial;

	[Header("DEBUG")] public bool DEBUG;

	public float timerPercent
    {
        get
        {
            return (sceneChangeTimer / (delayTime - 1.5f));
        }
    }

    public bool IsDialogueAutoImplimented()
    {
        return autoImplementDialogue;
    }

    // Start is called before the first frame update
    void Start()
    {
		_sallosMaterial.SetFloat("_Dissolve_Effect", 0);
		DialogueFlags = new List<DialogueFlag>();
        Traverser = new DialogueTraverser(this, _UIManager);

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

		//Scene changing
		onSceneChange += _UIManager.PauseAllButtons;
		foreach (DialogueFlag flag in DialogueFlags)
		{
			if (flag.Name == "end")
			{
				flag.onValueChange += delegate 
				{
					print("here");
					sceneChangeActive = true;
					onSceneChange(5);
				};
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (DEBUG)
        {
		    foreach (DialogueFlag flag in DialogueFlags)
		    {
		    	print(flag);
		    }
        }

		//foreach (DialogueFlag flag in DialogueFlags)
		//{
		//	if (flag.Name == flagName &&
		//		flag.IsTrue &&
		//		sceneChangeActive != true)
		//	{
		//		sceneChangeActive = true;

		//		sceneChangeTimer += Time.deltaTime;

		//		if (sceneChangeTimer > delayTime)
		//		{
		//			UnityEngine.SceneManagement.SceneManager.LoadScene(destinationScene);
		//		}
		//	}
		//}

		if (sceneChangeActive)
		{
			sceneChangeTimer += Time.deltaTime;
			if (sceneChangeTimer > delayTime)
			{
				SceneManager.LoadScene(destinationScene);
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
                        DialogueFlags.Add(new DialogueFlag(flag.Name));
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
        Traverser.GoToNode(choice);
    }

    public bool CheckTraversal(DialogueNode destinationNode)
    {
        return Traverser.CheckTraversal(destinationNode, DialogueFlags);
    }

    /// <summary>
    /// Checks if there is already a flag in the scene's internal list with the same name
    /// </summary>
    /// <param name="flag">Flag to check against list</param>
    /// <returns></returns>
    private bool FlagListContainsMatch(DialogueFlag flag)
    {
        foreach(DialogueFlag listFlag in DialogueFlags)
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
        _currentGraph = interactScript.Graph;
        Traverser.SetNewGraph(_currentGraph);

		//Send new node info to UI Manager
		_UIManager.NewDialogueNode(Traverser.currentNode);
		//print(currentGraph.StartNode.Info);
    }

	public void ChangeDialogueFlag (DialogueFlag newFlag)
	{
		foreach (DialogueFlag flag in DialogueFlags)
		{
			if (flag.MatchName(newFlag))
			{
				flag.IsTrue = newFlag.IsTrue;
				//print(flag);
			}
		}	
	}
}
