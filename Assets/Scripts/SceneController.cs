using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    #region Dialogue Components
    /// <summary>
    /// All graphs in scene.
    /// </summary>
    [Header("Dialogue Components")]
	protected List<NewDialogueGraph> _graphs;

	/// <summary>
	///	Twine json to read from.
	/// </summary>
	[Tooltip("Twine JSON to read from")] [SerializeField]
    protected TextAsset _twineJson;

	/// <summary>
	/// Does the scene start with dialogue?
	/// </summary>
	public bool autoStartDialogue = false;

    /// <summary>
    /// Graph used for scene as a whole.
    /// </summary>
    protected NewDialogueGraph _sceneGraph;

	/// <summary>
	/// All dialogue flags in scene.
	/// </summary>
	public List<NewDialogueFlag> DialogueFlags;

    /// <summary>
    /// All interactables in scene
    /// </summary>
    protected GameObject[] _interactables;

	/// <summary>
	/// UI Manager in Scene
	/// </summary>
	public UIManager _UIManager;

	/// <summary>
	/// Dialogue Traverser for traversing dialogue.
	/// </summary>
	public DialogueTraverser Traverser;
	#endregion

	#region Scene Changing
	/// <summary>
	/// Destination scene.
	/// </summary>
	[Header("Scene Changing")]
	[Tooltip("Destination Scene")][SerializeField] protected string _destinationScene;

	/// <summary>
	/// Time in seconds, to fade out
	/// </summary>
	[Tooltip("Time, in seconds, that it takes to fade out of scene.")]
	[SerializeField] protected float _fadeOutTime;

	/// <summary>
	/// Is the scene currently fading out?
	/// </summary>
	[HideInInspector] public bool FadingOut = false;

    /// <summary>
    /// Current time of fadeout.
    /// </summary>
    protected float _fadeOutTimer = 0f;

	/// <summary>
	/// Time, in seconds, to fade in.
	/// </summary>
	[Space(10)]
	[Tooltip("Time, in seconds, to fade in.")][SerializeField] protected float _fadeInTime;

	/// <summary>
	/// Does the scene fade in?
	/// </summary>
	public bool FadesIn = true;

    /// <summary>
    /// Current time of fadein.
    /// </summary>
    protected float _fadeInTimer = 0f;

	public delegate IEnumerator OnSceneChangeHandler(float seconds);
	public event OnSceneChangeHandler onSceneChange;
	#endregion

	[Space(10)] [SerializeField] protected Material _sallosMaterial;

	[Header("DEBUG")] public bool DEBUG;

	public float FadeOutTimerPercent
	{
		get
		{
			return (_fadeOutTimer / (_fadeOutTime - 1.5f));
		}
	}

	public float FadeInTimerPercent
	{
		get
		{
			return (_fadeInTimer / (_fadeInTime - 1.5f));
		}
	}



	public bool IsDialogueAutoImplemented()
	{
		return autoStartDialogue;
	}

	// Start is called before the first frame update
	protected void Start()
	{
		_sallosMaterial.SetFloat("_Dissolve_Effect", 0);

		//Setup graphs
		JSONGraph jsonGraph = JsonUtility.FromJson<JSONGraph>(_twineJson.text);
		_graphs = jsonGraph.CreateGraphs();
		DialogueFlags = new List<NewDialogueFlag>();
		Traverser = new DialogueTraverser(this, _UIManager);
		_sceneGraph = _graphs.FirstOrDefault(graph => graph.Name == "scene");

		//Find all interactable objects in scene
		_interactables = GameObject.FindGameObjectsWithTag("Interactable");
		//sets up each interactable in scene
		foreach (GameObject interactable in _interactables) 
		{
			//Sets up graph for interactable
			InteractableScript interactScript = interactable.GetComponent<InteractableScript>();
			interactScript.Graph = _graphs.FirstOrDefault(graph => graph.Name == interactable.name);

			interactScript.UpdateSceneGraph.AddListener(UpdateCurrentGraph);
		}

		foreach (NewDialogueGraph graph in _graphs)
		{
			foreach (NewDialogueNode node in graph.Nodes)
			{
				CreateDialogueFlags(node);
			}
		}

		// If scene dialogue is to be auto-implemented, set dialogue to that of first NPC
		if (autoStartDialogue)
		{

			ActivateSceneDialogue();
			//UpdateCurrentGraph(_interactables[0].GetComponent<InteractableScript>());
		}

		//Scene changing
		onSceneChange += _UIManager.PauseAllButtons;
		foreach (NewDialogueFlag flag in DialogueFlags)
		{
			if (flag.Names.Contains("end"))
			{
				flag.onValueChange += delegate 
				{
					print("here");
					FadingOut = true;
					onSceneChange(5);
				};
			}
		}
	}

    // Update is called once per frame
    protected void Update()
	{
		if (DEBUG)
		{
			foreach (NewDialogueFlag flag in DialogueFlags)
			{
				print(flag);
			}
		}

		#region Fades in/out
		if (FadesIn)
		{
			_fadeInTimer = Mathf.Clamp(_fadeInTimer + Time.deltaTime, 0, _fadeInTime);
		}
		if (_fadeInTimer == _fadeInTime)
		{
			FadesIn = false;
		}
		

		if (FadingOut)
		{
			_fadeOutTimer += Time.deltaTime;
			if (_fadeOutTimer > _fadeOutTime)
			{
				SceneManager.LoadScene(_destinationScene);
			}
		}
		#endregion
	}

    protected void ActivateSceneDialogue()
	{
		Traverser.SetNewGraph(_sceneGraph);

		//print(Traverser.CurrentGraph.StartNode.Text);
	}

    /// <summary>
    /// Populates list of dialogue flags with every flag in every dialogue graph
    /// in the scene
    /// </summary>
	/// <param name="graph">Graph to add flags from</param>
    protected void CreateDialogueFlags(NewDialogueNode node)
	{
		foreach (NewDialogueFlag flag in node.FlagsToChange)
        {
            NewDialogueFlag match = DialogueFlags.FirstOrDefault(
				matchFlag => matchFlag.Names.SequenceEqual(flag.Names));
			//skip if there is a match
            if (match != null) continue;
            DialogueFlags.Add(new NewDialogueFlag(flag.Names));
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

	public bool CheckTraversal(List<NewDialogueFlag> flagsToCheck)
	{
		return Traverser.CheckTraversal(flagsToCheck);
	}

    /// <summary>
    /// Update the currentGraph variable based on the npcScript the sceneManager gets an
    /// UpdateSceneGraph call from
    /// </summary>
    /// <param name="npcScript">Script that the sceneManager recieved an event call from</param>
    protected void UpdateCurrentGraph(InteractableScript interactScript)
	{
		Traverser.SetNewGraph(interactScript.Graph);
	}

	public void ChangeDialogueFlag (NewDialogueFlag newFlag)
	{
		foreach (NewDialogueFlag flag in DialogueFlags)
		{
			if (flag.Names.Equals(newFlag.Names)) 
			{
				flag.IsTrue = newFlag.IsTrue;
				//print(flag);
			}
		}	
	}

	//private bool CheckNameListEquality()

	public void NextScene()
	{
		FadingOut = true;
	}
}
