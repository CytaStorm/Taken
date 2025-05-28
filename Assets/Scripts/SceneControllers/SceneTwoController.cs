using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SceneTwoController : SceneController
{
	// Actors
	[SerializeField] private GameObject _sallos;

	// Actor Components
	private NPCScript _sallosScript;
	private NavMeshAgent _sallosAgent;
	private Animator _sallosAnimator;

	Transform sallosMesh;

	//Interactable
	[SerializeField] private GameObject _deadTree;
	private InteractableScript _deadTreeScript;
	[SerializeField] private GameObject _tinder;
	private InteractableScript _tinderScript;
	[SerializeField] private GameObject _stove;
	private InteractableScript _stoveScript;
	[SerializeField] private GameObject _hatchet;
	private InteractableScript _hatchetScript;

	//props
	[SerializeField] private GameObject _stoveLogs;
	[SerializeField] private GameObject _stoveTinder;


	// Start is called before the first frame update
	new void Start()
	{
		base.Start();
		_deadTreeScript = _deadTree.GetComponent<InteractableScript>();
		_tinderScript = _tinder.GetComponent<InteractableScript>();
		_stoveScript = _stove.GetComponent<InteractableScript>();
		_hatchetScript = _hatchet.GetComponent<InteractableScript>();

		_sallosScript = _sallos.GetComponent<NPCScript>();
		_sallosAgent = _sallos.GetComponent<NavMeshAgent>();
		_sallosAnimator = _sallos.GetComponent<Animator>();
		sallosMesh = _sallosScript.Mesh.transform;

		//Hook up events
		_flagNames.Add("talkedToSallos");
		_flagNames.Add("finishedTinderQuest");
		_flagNames.Add("finishedFirewoodQuest");
		_flagNames.Add("placeStoveLogs");
		_flagNames.Add("placeStoveTinder");
		_flagNames.Add("sallosWalkIn");
		_flagNames.Add("sallosDiscussionRepeat");
		_flagNames.Add("sharpenedHatchet");
		CreateEventFlags();

		_eventFlags[0].OnValueChange += delegate
		{
			print("talkedToSallos");
			ChangeGraph(_sallosScript, "sallosTalkedTo");
		};

		//Sallos goes to firewood
		_eventFlags[1].OnValueChange += delegate { SallosGoToLog(); };

		//Sallos goes to tinder
		_eventFlags[2].OnValueChange += delegate { SallosGoToTinder(); };

		//place firewood
		_eventFlags[3].OnValueChange += delegate { _stoveLogs.SetActive(true);  };

		//place tinder
		_eventFlags[4].OnValueChange += delegate { _stoveTinder.SetActive(true); };

		//Sallos walk in
		_eventFlags[5].OnValueChange += delegate { 
			StartCoroutine(SallosWalkIntoTent(5)); };

		//Sallos discussion repeat
		_eventFlags[6].OnValueChange += delegate
		{
			_sallosScript.Graph.StartNode = 
				_sallosScript.Graph.Nodes.FirstOrDefault(
					newNode => newNode.Name == "sallosDiscussionRepeat");
		};

		_eventFlags[7].OnValueChange += delegate
		{
			_sallosScript.Graph.StartNode =
				_sallosScript.Graph.Nodes.FirstOrDefault(
					newNode => newNode.Name == "timeToEat");
		};
	}

	// Update is called once per frame
	new void Update()
	{
		base.Update();
	}

	private void SallosGoToLog()
	{
		_sallosAgent.speed = 3f;
		_sallosAgent.SetDestination(_deadTreeScript.InteractionPoint.transform.position);
		StartCoroutine(GoToLocation(_deadTree.transform.position, "sallosAtDeadTree"));
		_deadTreeScript.Interactable = false;
	}

	private void SallosGoToTinder()
	{
		_sallosAgent.speed = 3f;
		_sallosAgent.SetDestination(_tinderScript.InteractionPoint.transform.position);
		StartCoroutine(GoToLocation(_tinder.transform.position, "sallosAtTinder"));
		_tinderScript.Interactable = false;
	}

	private IEnumerator SallosWalkIntoTent(float fadeTime)
	{
		StartCoroutine(Fade(fadeTime));
		yield return new WaitForSeconds(fadeTime);
		//Disable stove interaction
		_stoveScript.Interactable = false;

		//teleport player
		PlayerController.Instance.Agent.Warp(new Vector3(-51, 2.17f, 174));
		PlayerController.Instance.gameObject.transform.forward =
			(_stove.transform.position -
			PlayerController.Instance.transform.position);

		//Teleport sallos to edge of tent
		_sallosAgent.Warp(new Vector3(-52, 2.17f, 179));
		_sallosScript.InteractionPivot.transform.rotation =
			Quaternion.Euler(0, 90, 0);
		_sallosAgent.speed = 1.5f;
		_sallosAgent.SetDestination(_stoveScript.InteractionPoint.transform.position);
		_hatchet.SetActive(true);
		StartCoroutine(GoToLocation(_stove.transform.position, "sallosDiscussion"));
	}


	private IEnumerator GoToLocation(
		Vector3 destination, string newGraphName)
	{
		while (Quaternion.Angle(
			_sallos.transform.rotation, Quaternion.identity) > 0)
		{
			_sallos.transform.rotation = Quaternion.RotateTowards(
				_sallos.transform.rotation,
				Quaternion.identity,
				250 * Time.deltaTime);
			yield return null;
		}

		_sallosScript.Interactable = false;
		while (Vector3.Distance(sallosMesh.position, _sallosAgent.destination) > 0.1f)
		{
			yield return null;
		}

		_sallosAgent.isStopped = true;

		Quaternion q = Quaternion.LookRotation(
			destination - sallosMesh.transform.position);

		while (Quaternion.Angle(sallosMesh.transform.rotation, q) > 0)
		{
			sallosMesh.transform.rotation =
				Quaternion.RotateTowards(sallosMesh.transform.rotation, q, 250 * Time.deltaTime);
			yield return null;
		}

		//Arrived at log
		_sallosScript.Interactable = true;
		ChangeGraph(_sallosScript, newGraphName);
		_sallosScript.FacePlayerWhileTalking = false;
	}
}

