using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class SceneTwoController : CutsceneController
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
	[SerializeField] private GameObject _kindling;

	// Start is called before the first frame update
	new void Start()
	{
		base.Start();

		_sallosScript = _sallos.GetComponent<NPCScript>();
		_sallosAgent = _sallos.GetComponent<NavMeshAgent>();
		_sallosAnimator = _sallos.GetComponent<Animator>();
		sallosMesh = _sallosScript.Mesh.transform;

		//Hook up events
		_flagNames.Add("logQuest");
		_flagNames.Add("kindlingQuest");
		_flagNames.Add("turnToFacePlayer");

		CreateEventFlags();

		_eventFlags[0].onValueChange += delegate { SallosGoToKindling(); };
		_eventFlags[1].onValueChange += delegate { SallosGoToLog(); };
		_eventFlags[2].onValueChange += delegate {
			if (_eventFlags[2].IsTrue)
			{
				_sallosScript.LookAtPlayer();
			}
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
		_sallosAgent.SetDestination(new Vector3(32.81f, 0f, 38.47f));
		StartCoroutine(GoToLog());
	}
	
	private void SallosGoToKindling()
	{
		_sallosAgent.speed = 3f;
		_sallosAgent.SetDestination(new Vector3(28.02f, 0f, 34.12f));
		StartCoroutine(GoToKindling());
	}

	private IEnumerator GoToLog()
	{
		//Move to Log
		_sallosScript.Interactable = false;
		while (Vector3.Distance(sallosMesh.position, _sallosAgent.destination) > 0.1f)
		{
			yield return null;
		}

		_sallosAgent.isStopped = true;

		Quaternion q = Quaternion.LookRotation(
			_deadTree.transform.position - sallosMesh.transform.position);

		while(Quaternion.Angle(sallosMesh.transform.rotation, q) > 0)
		{
			sallosMesh.transform.rotation = 
				Quaternion.RotateTowards(sallosMesh.transform.rotation, q, 50 * Time.deltaTime);
			yield return null;
		}

		//Arrived at log
		_sallosScript.Interactable = true;
		_sallosScript.Graph = _graphs.FirstOrDefault(graph => graph.Name == "logSallos");
		_sallosScript.FacePlayerWhileTalking = false;
	}

	private IEnumerator GoToKindling()
	{
		_sallosScript.Interactable = false;
		while (Vector3.Distance(sallosMesh.position, _sallosAgent.destination) > 0.1f)
		{
			yield return null;
		}

		_sallosAgent.isStopped = true;

		Quaternion q = Quaternion.LookRotation(
			_kindling.transform.position - sallosMesh.transform.position);

		while(Quaternion.Angle(sallosMesh.transform.rotation, q) > 0)
		{
			sallosMesh.transform.rotation = 
				Quaternion.RotateTowards(sallosMesh.transform.rotation, q, 50 * Time.deltaTime);
			yield return null;
		}

		//Arrived at log
		_sallosScript.Interactable = true;
		_sallosScript.Graph = _graphs.FirstOrDefault(graph => graph.Name == "kindlingSallos");
		_sallosScript.FacePlayerWhileTalking = false;
		
	}
}
