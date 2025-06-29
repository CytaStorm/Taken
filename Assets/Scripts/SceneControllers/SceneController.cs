using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Interaction
    /// <summary>
    /// All graphs in scene.
    /// </summary>
    [Header("Dialogue Components")]
	protected List<DialogueGraph> _graphs;

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
    protected DialogueGraph _sceneGraph;

	/// <summary>
	/// All dialogue flags in scene.
	/// </summary>

    /// <summary>
    /// All interactables in scene
    /// </summary>
    protected GameObject[] _interactables;

	/// <summary>
	/// Dialogue Traverser for traversing dialogue.
	/// </summary>
	public DialogueTraverser Traverser;
	#endregion

	#region Event Flags
	// Names of flags that are events
    protected List<string> _flagNames = new List<string>();

    // Dialogue Flags that will raise events
    protected List<DialogueFlag> _eventFlags = new List<DialogueFlag>();
    protected double _timer = 0;
	#endregion

	#region Scene Changing
	/// <summary>
	/// Latest fade coroutine started, defined to allow stopping fades early
	/// </summary>
	public Coroutine LatestFade;
	/// <summary>
	/// Destination scene.
	/// </summary>
	[Header("Scene Changing")]
	[Tooltip("Destination Scene")][SerializeField] protected string _destinationScene;

	/// <summary>
	/// Current time it takes to fade in
	/// </summary>
	private float _currentFadeInTime;

	/// <summary>
	/// Current time it takes to fade out
	/// </summary>
	private float _currentFadeOutTime;

	/// <summary>
	/// Time, in seconds, that it takes to fade out of scene into a different scene.
	/// </summary>
	[Tooltip("Time, in seconds, that it takes to fade out of scene into a different scene.")]
	public float SceneFadeOutTime;

	/// <summary>
	/// Is the scene currently fading out?
	/// </summary>
	[HideInInspector] public bool FadingOut = false;

    /// <summary>
    /// Current time of fadeout.
    /// </summary>
    protected float _fadeOutTimer = 0f;

	/// <summary>
	/// Time, in seconds, for the scene to fade in from a different scene.
	/// </summary>
	[Tooltip("Time, in seconds, for the scene to fade in from a different scene.")]
	public float SceneFadeInTime;

	/// <summary>
	/// Is the scene currently fading in?
	/// </summary>
	public bool FadingIn = true;

    /// <summary>
    /// Current time of fadein.
    /// </summary>
    protected float _fadeInTimer = 0f;

	public delegate IEnumerator OnSceneChangeHandler(float seconds);
	public event OnSceneChangeHandler onSceneChange;

	private bool newScene = false;
	public float FadeOutTimerPercent
	{
		get
		{
			return (_fadeOutTimer / (_currentFadeOutTime));
		}
	}

	public float FadeInTimerPercent
	{
		get
		{  
			return (_fadeInTimer / (_currentFadeInTime));
		}
	}

	#endregion

	/// <summary>
	/// Singleton
	/// </summary>
	public static SceneController Instance;

	[Header("DEBUG")] public bool DEBUG;
	
	public bool IsDialogueAutoImplemented
	{
		 get => autoStartDialogue;
	}

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    protected void Start()
	{
		//Setup graphs
		JSONGraph jsonGraph = JsonUtility.FromJson<JSONGraph>(_twineJson.text);
		_graphs = jsonGraph.CreateGraphs();
		Traverser = new DialogueTraverser(this, UIManager.UI);
		_sceneGraph = _graphs.FirstOrDefault(graph => graph.Name == "scene");

		//Find all Interactable objects in scene
		_interactables = GameObject.FindGameObjectsWithTag("Interactable");
		//sets up each Interactable in scene
		foreach (GameObject interactable in _interactables) 
		{
			//Sets up graph for Interactable
			InteractableScript interactScript = interactable.GetComponent<InteractableScript>();
			interactScript.Graph = _graphs.FirstOrDefault(graph => graph.Name == interactable.name);

			interactScript.UpdateSceneGraph.AddListener(UpdateCurrentGraph);
		}

		foreach (DialogueGraph graph in _graphs)
		{
			foreach (DialogueNode node in graph.Nodes)
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
		_currentFadeInTime = SceneFadeInTime;
		_currentFadeOutTime = SceneFadeOutTime;

		onSceneChange += UIManager.UI.PauseAllButtons;
		foreach (DialogueFlag flag in Flags.Instance.DialogueFlags)
		{
			if (flag.Name.Contains("End"))
			{
				flag.OnValueChange += delegate 
					//ExtractFlags returns a list of list of newdialogue flags, but because each
				{
					print("here");
					FadingOut = true;
					newScene = true;
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
			foreach (DialogueFlag flag in Flags.Instance.DialogueFlags)
			{
				print(flag);
			}
		}


        _timer += Time.deltaTime;
		#region Fades in/out
		if (FadingIn)
		{
			_fadeInTimer = Mathf.Clamp(_fadeInTimer + Time.deltaTime, 0, _currentFadeInTime);
		}
		if (_fadeInTimer == _currentFadeInTime)
		{
			FadingIn = false;
		}
		

		if (FadingOut)
		{
			_fadeOutTimer = Mathf.Clamp(_fadeOutTimer + Time.deltaTime, 0, _currentFadeOutTime);
			if (_fadeOutTimer >= _currentFadeOutTime && newScene)
			{
				SceneManager.LoadScene(_destinationScene);
			}
		}
		if (_fadeOutTimer == _currentFadeOutTime)
		{
			FadingOut = false;
			FadingIn = true;
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
    /// in the scene. Important to note that these flags are new and default.
    /// </summary>
	/// <param name="graph">Graph to add flags from</param>
    protected void CreateDialogueFlags(DialogueNode node)
	{
		foreach (DialogueFlag flag in node.FlagsToChange)
        {
         //   NewDialogueFlag match = DialogueFlags.FirstOrDefault(
         //   	matchFlag => matchFlag.Names.SequenceEqual(flag.Names));
            DialogueFlag match = Flags.Instance.DialogueFlags.FirstOrDefault(
				matchFlag => matchFlag.Name == flag.Name);

			//skip if there is a match
            if (match != null) continue;

			//Differentiate between bool and value
			if (flag is DialogueFlagBool)
			{
				Flags.Instance.DialogueFlags.Add(new DialogueFlagBool(flag.Name));
			}
			else if (flag is DialogueFlagValue)
			{
				Flags.Instance.DialogueFlags.Add(new DialogueFlagValue(flag.Name));
			}
			else
			{
				throw new ArgumentException("Flag failed to parse");
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

	public bool CheckTraversal(List<DialogueFlag> flagsToCheck)
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

	/// <summary>
	/// Matches dialogue flag to supplied flag.
	/// </summary>
	/// <param name="newFlag">Flag to copy value from.</param>
	public void ChangeDialogueFlag (DialogueFlag newFlag)
	{
		foreach (DialogueFlag flag in Flags.Instance.DialogueFlags)
		{
			//matching flag
			if (flag.Name == newFlag.Name)
			{
				//bool flag
				if (flag is DialogueFlagBool)
				{
					((DialogueFlagBool)flag).IsTrue = ((DialogueFlagBool)newFlag).IsTrue;
					continue;
				}

				//Value flag
				if (flag is DialogueFlagValue)
				{
					DialogueFlagValue newValueFlag = (DialogueFlagValue)newFlag;
					DialogueFlagValue valueFlag = (DialogueFlagValue)flag;

					//Relative change 
					if (newValueFlag.RelativeChange != null)
					{
						valueFlag.Value += (int)newValueFlag.RelativeChange;
					}
					//absolute change
					else
					{
						valueFlag.Value = newValueFlag.Value;
					}
					continue;
				}
			}
		}
	}

	/// <summary>
	/// Changes interactable's graph.
	/// </summary>
	/// <param name="interactable">Interactable to change.</param>
	/// <param name="graphName">Name of new graph.</param>
	protected void ChangeGraph(InteractableScript interactable, string graphName)
	{
		interactable.Graph = _graphs.FirstOrDefault(
			graph => graph.Name == graphName);
	}

	/// <summary>
    /// Identifies flags with dev added flag names and adds
    /// them to the eventflags list.
    /// </summary>
    protected void CreateEventFlags()
    {
        foreach (string name in _flagNames)
        {
            foreach (DialogueFlag flag in Flags.Instance.DialogueFlags)
            {
                if (flag.Name != name) continue;

                _eventFlags.Add(flag);
            }
        }
    }

	/// <summary>
	/// Fades in and out, used for in place transitions
	/// </summary>
	/// <param name="seconds">Fade in and out time, in seconds</param>
	public void Fade(float seconds)
	{
		LatestFade = StartCoroutine(FadeInternal(seconds));
	}

	public void FadeBackOut(float newFadeInSeconds)
	{
		LatestFade = StartCoroutine(FadeBackOutInternal(newFadeInSeconds));
	}

	private IEnumerator FadeInternal(float seconds)
	{
        _currentFadeInTime = seconds;
        _currentFadeOutTime = seconds;

		_fadeInTimer = 0;
		_fadeOutTimer = 0;
		FadingOut = true;

		//disable player input
		PlayerController.Instance.CanMove = false;

		yield return new WaitForSeconds(seconds);
		
		//enable player input
		PlayerController.Instance.CanMove = true;

		//fade back in
		yield return new WaitForSeconds(seconds);

		//reset fade times
		FadingIn = false;
		FadingOut = false;
		_fadeInTimer = 0;
		_fadeOutTimer = 0;
        _currentFadeInTime = SceneFadeInTime;
		_currentFadeOutTime = SceneFadeOutTime;

		yield return null;
	}

	private IEnumerator FadeBackOutInternal(float newFadeInSeconds)
	{
		//stop fading in
		FadingIn = false;

		//how much time is remaining on fade in
		float fadeInTimeLeft = TimeRemainingOnFadeIn();

		//Begin fade out from remaining fade in time progress
		_fadeOutTimer = _fadeInTimer;
        _currentFadeOutTime = _currentFadeInTime;

		//Reset fade in time for next fade in
		_fadeInTimer = 0;
		_currentFadeInTime = newFadeInSeconds;

		//Begin fading out
		FadingOut = true;

		//disable player input
		PlayerController.Instance.CanMove = false;

		yield return new WaitForSeconds(fadeInTimeLeft);

		//enable player input
		PlayerController.Instance.CanMove = true;

		//fade in again
		yield return new WaitForSeconds(newFadeInSeconds);

		//Reset fades
		FadingIn = false;
		FadingOut = false;
		_fadeInTimer = 0;
		_fadeOutTimer = 0;
        _currentFadeInTime = SceneFadeInTime;
		_currentFadeOutTime = SceneFadeOutTime;

		yield return null;
	}

	public float TimeRemainingOnFadeIn()
	{
		return (1 - FadeInTimerPercent) * _currentFadeInTime;
	}

	public float TimeRemainingOnFadeOut()
	{
		return (1 - FadeOutTimerPercent) * _currentFadeOutTime;
	}
}
