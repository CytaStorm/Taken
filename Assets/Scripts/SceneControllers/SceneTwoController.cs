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

	//Interactable positions
	[SerializeField] private GameObject _deadTree;
	private InteractableScript _deadTreeScript;
	[SerializeField] private GameObject _tinder;
	private InteractableScript _tinderScript;

	// Start is called before the first frame update
	new void Start()
	{
		base.Start();
		_deadTreeScript = _deadTree.GetComponent<InteractableScript>();
		_tinderScript = _tinder.GetComponent<InteractableScript>();

		_sallosScript = _sallos.GetComponent<NPCScript>();
		_sallosAgent = _sallos.GetComponent<NavMeshAgent>();
		_sallosAnimator = _sallos.GetComponent<Animator>();
		sallosMesh = _sallosScript.Mesh.transform;

		//Hook up events
		_flagNames.Add("talkedToSallos");
		_flagNames.Add("finishedTinderQuest");
		_flagNames.Add("finishedFirewoodQuest");
		CreateEventFlags();

		_eventFlags[0].OnValueChange += delegate
		{
			print("talkedToSallos");
			ChangeGraph(_sallosScript, "sallosTalkedTo");
		};

		//Sallos goes to firewood
		_eventFlags[1].OnValueChange += delegate { SallosGoToLog(); };

		//Sallos goes to tinder
		_eventFlags[2].OnValueChange +=
			delegate { SallosGoToTinder(); };
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

