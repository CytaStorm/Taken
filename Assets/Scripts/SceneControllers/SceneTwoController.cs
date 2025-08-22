using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SceneTwoController : SceneController
{
	//Material
	[SerializeField] private Material _sallosMaterial;
	// Actors
	[Header("Actors")]
	[SerializeField] private GameObject _sallos;
	[SerializeField] private GameObject _player;

	// Actor Components
	private NPCScript _sallosScript;
	private NavMeshAgent _sallosAgent;
	[SerializeField] private Animator _sallosAnimator;

	private NPCScript _playerScript;
	private NavMeshAgent _playerAgent;
	[SerializeField] private Animator _playerAnimator;

	//Interactable
	[Header("Objects")]
	[SerializeField] private GameObject _deadTree;
	private InteractableScript _deadTreeScript;
	[SerializeField] private GameObject _tinder;
	private InteractableScript _tinderScript;
	[SerializeField] private GameObject _stove;
	private InteractableScript _stoveScript;

	[Header("Hatchet")]
	[SerializeField] private GameObject _hatchet;
	[SerializeField] private Material _hatchetMat;
	[SerializeField] private Texture _dullHatchetTexture;
	[SerializeField] private Texture _sharpHatchetTexture;
	[SerializeField] private Light _hatchetGlow;
	[Space(10)]

	//props
	[SerializeField] private GameObject _stoveLogs;
	[SerializeField] private GameObject _stoveTinder;
	[SerializeField] private GameObject _stoveLight;


	// Start is called before the first frame update
	new void Start()
	{
		base.Start();
		//Reset materials
        _sallosMaterial.SetFloat("_Dissolve_Effect", 0);
		_hatchetMat.SetColor("_Emissive_Color", Color.black);
		_hatchetMat.SetTexture("_Texture", _dullHatchetTexture);
		_hatchetGlow.intensity = 0;
		_stoveLight.SetActive(false);

		_deadTreeScript = _deadTree.GetComponent<InteractableScript>();
		_tinderScript = _tinder.GetComponent<InteractableScript>();
		_stoveScript = _stove.GetComponent<InteractableScript>();

		_sallosScript = _sallos.GetComponent<NPCScript>();
		_sallosAgent = _sallos.GetComponent<NavMeshAgent>();

		_playerScript = _player.GetComponent<NPCScript>();
		_playerAgent = _player.GetComponent<NavMeshAgent>();


		//Hook up events
		_flagNames.Add("talkedToSallos"); //0
		_flagNames.Add("finishedTinderQuest"); //1
		_flagNames.Add("finishedFirewoodQuest"); //2
		_flagNames.Add("placeStoveLogs"); //3
		_flagNames.Add("placeStoveTinder"); //4
		_flagNames.Add("sallosWalkIn"); //5
		_flagNames.Add("sallosDiscussionRepeat"); //6
		_flagNames.Add("sharpenedHatchet"); //7
		_flagNames.Add("eatCutscene"); //8
		_flagNames.Add("raiseArm");
		_flagNames.Add("focus");
		_flagNames.Add("lowerHand");
		_flagNames.Add("getUp");
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
		_eventFlags[3].OnValueChange += delegate {
			_playerAnimator.SetTrigger("Place");
			_stoveLogs.SetActive(true); };

		//place tinder
		_eventFlags[4].OnValueChange += delegate { 
			_playerAnimator.SetTrigger("Place");
			_stoveTinder.SetActive(true); };

		//Sallos walk in
		_eventFlags[5].OnValueChange += delegate {
			StartCoroutine(SallosWalkIntoTent(2));
		};

		//Sallos discussion repeat
		_eventFlags[6].OnValueChange += delegate
		{
			_sallosScript.Graph.StartNode =
				_sallosScript.Graph.Nodes.FirstOrDefault(
					newNode => newNode.Name == "sallosDiscussionRepeat");
		};

		//Sharpen hatchet
		_eventFlags[7].OnValueChange += delegate
		{
			_sallosScript.Graph.StartNode =
				_sallosScript.Graph.Nodes.FirstOrDefault(
					newNode => newNode.Name == "timeToEat");
		};

		//Start talking with Sallos while eating.
		_eventFlags[8].OnValueChange += delegate
		{
			StartCoroutine(EatCutscene(3f));
		};

		_eventFlags[9].OnValueChange += delegate {
			_playerAnimator.SetTrigger("RaiseArm");
		};

		_eventFlags[10].OnValueChange += delegate
		{
			_playerAnimator.SetTrigger("Focus");
			StartCoroutine(HatchetGlow(5f));
		};

		_eventFlags[11].OnValueChange += delegate
		{
			_playerAnimator.SetTrigger("LowerArm");
			StartCoroutine(HatchetUnglow(5f));
		};

		_eventFlags[12].OnValueChange += delegate 
		{
			_playerAnimator.SetTrigger("getUp");
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
		_sallosScript.InteractionPivot.transform.rotation = 
			Quaternion.Euler(0, 250, 0);
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
		Fade(fadeTime);
		yield return new WaitForSeconds(fadeTime);

		//Disable stove interaction
		_stoveScript.Interactable = false;

		//teleport player
		PlayerController.Instance.Agent.Warp(new Vector3(-51, 2.17f, 175));
		PlayerController.Instance.gameObject.transform.forward =
			(_stove.transform.position -
			PlayerController.Instance.transform.position);

		//Teleport sallos to edge of tent
		_sallosAgent.Warp(_stoveScript.InteractionPoint.transform.position);
		_sallosAgent.transform.forward = _stove.transform.position;
		_sallosScript.InteractionPivot.transform.rotation =
			Quaternion.Euler(0, 90, 0);
		_hatchet.SetActive(true);
		ChangeGraph(_sallosScript, "sallosDiscussion");
		_stoveLight.SetActive(true);
		yield return null;
	}

	private IEnumerator EatCutscene(float fadeTime)
	{
		Fade(fadeTime);
		yield return new WaitForSeconds(fadeTime);

		//Teleport sallos and eulyss to eating pose
		_sallosAgent.Warp(new Vector3(-51, 2.17f, 175));
		_sallosAgent.transform.forward = 
			new Vector3(-52, 2.17f, 176.3f) -
			_sallosAgent.transform.position;

		PlayerController.Instance.Agent.Warp(new Vector3(-53, 2.17f, 176));
		PlayerController.Instance.gameObject.transform.forward = 
			new Vector3(-52, -2.17f, 176.3f) - 
			PlayerController.Instance.transform.position;

		//Interact with sallos
		Traverser.SetNewGraph(
			_graphs.FirstOrDefault(graph => graph.Name == "likeIt"));
	}

	private IEnumerator GoToLocation(
		Vector3 destination, string newGraphName)
	{
		_sallosAgent.stoppingDistance = 0.15f;
		//while (Quaternion.Angle(
		//	_sallos.transform.rotation, Quaternion.identity) > 0)
		//{
		//	_sallos.transform.rotation = Quaternion.RotateTowards(
		//		_sallos.transform.rotation,
		//		Quaternion.identity,
		//		250 * Time.deltaTime);
		//	yield return null;
		//}

		_sallosScript.Interactable = false;

		//while (Vector3.Distance(sallosMesh.position, _sallosAgent.destination) > 0.1f)
		//{
		//	yield return null;
		//}

		while (_sallosAgent.pathPending ||
			_sallosAgent.remainingDistance > _sallosAgent.stoppingDistance ||
			_sallosAgent.velocity.sqrMagnitude != 0f)
		{
			yield return null;
		}

		_sallosAgent.isStopped = true;

		//Quaternion q = Quaternion.LookRotation(
		//	destination - sallosMesh.transform.position);

		Quaternion q = Quaternion.LookRotation(
			destination - _sallos.transform.position);

		while (Quaternion.Angle(_sallos.transform.rotation, q) > 0)
		{
			_sallos.transform.rotation =
				Quaternion.RotateTowards(
					_sallos.transform.rotation, q, 250 * Time.deltaTime);
			yield return null;
		}

		//Arrived at log
		_sallosScript.Interactable = true;
		ChangeGraph(_sallosScript, newGraphName);
		_sallosScript.FacePlayerWhileTalking = false;
	}

	private IEnumerator HatchetGlow(float durationSeconds)
	{
		for (float i = 0; i < durationSeconds; i += Time.deltaTime)
		{
			_hatchetMat.SetColor(
				"_Emissive_Color", 
				new Color(0, 173, 191) * Mathf.Lerp(0, 10, i / durationSeconds));

			_hatchetGlow.intensity = Mathf.Lerp(0, 40000, i / durationSeconds);
			yield return null;
		}
		yield return null;	
	}
	private IEnumerator HatchetUnglow(float durationSeconds)
	{
		_hatchetMat.SetTexture("_Texture", _sharpHatchetTexture);
		for (float i = durationSeconds; i >= 0 ; i -= Time.deltaTime)
		{
			_hatchetMat.SetColor(
				"_Emissive_Color", 
				new Color(0, 173, 191) * Mathf.Lerp(-10, 10, i / durationSeconds));
			_hatchetGlow.intensity = Mathf.Lerp(0, 40000, i / durationSeconds);
			yield return null;
		}

		_hatchetMat.SetColor("_Emissive_Color", Color.black);
		_hatchetGlow.intensity = 0;

		yield return null;	
	}

	private void OnDrawGizmos()
	{
		//Gizmos.DrawRay(sallosMesh.transform.position, );
	}
}

