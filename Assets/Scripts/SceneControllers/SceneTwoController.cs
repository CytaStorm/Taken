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
			StartCoroutine(SallosWalkIntoTent()); };
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

	private IEnumerator SallosWalkIntoTent()
	{
		Fade();
		//Teleport sallos to edge of tent
		yield return new WaitForSeconds(5f);
		PlayerController.Instance.Agent.Warp(new Vector3(-51, 2.17f, 174));
		Quaternion.RotateTowards(
			PlayerController.Instance.transform.rotation,
			_stove.transform.rotation, 360);
		_sallosAgent.Warp(new Vector3(-52, 2.17f, 179));

		_sallosScript.InteractionPivot.transform.rotation =
			Quaternion.Euler(0, 90, 0);
		_sallosAgent.speed = 1.5f;
		_sallosAgent.SetDestination(_stoveScript.InteractionPoint.transform.position);
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

