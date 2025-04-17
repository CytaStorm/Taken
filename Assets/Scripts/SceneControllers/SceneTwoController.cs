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

	//Interactable positions
	[SerializeField] private GameObject _deadTree;

	// Start is called before the first frame update
	new void Start()
	{
		base.Start();

		//Hook up events
		_flagNames.Add("logQuest");
		_flagNames.Add("kindlingQuest");

		CreateEventFlags();

		_eventFlags[1].onValueChange += delegate { SallosGoToLog(); };
		//_eventFlags[1].onValueChange += delegate

		_sallosScript = _sallos.GetComponent<NPCScript>();
		_sallosAgent = _sallos.GetComponent<NavMeshAgent>();
		_sallosAnimator = _sallos.GetComponent<Animator>();
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
		StartCoroutine(ArriveAtLog());

		//while (_sallos.transform.rotation.y != -97.8)
		//{
		//    _sallos.transform.rotation = Quaternion.RotateTowards(_sallos.transform.rotation, Quaternion.Euler(0, -97.8f, 0), 180);
		//}
		//_sallosScript.InteractionPoint.transform.position = new Vector3();
	}
	
	private IEnumerator ArriveAtLog()
	{
		//Move to Log
		Transform sallosMesh = _sallosScript.Mesh.transform;

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
}
