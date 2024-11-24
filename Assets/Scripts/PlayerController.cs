using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private PlayerInput playerInput; 

	[SerializeField] private Camera cam;
	[SerializeField] private NavMeshAgent agent;
	[SerializeField] private int raycastRange = 100;
	[SerializeField] private LayerMask movementMask;
	[SerializeField] private LayerMask interactableMask;
	[SerializeField] private bool inDialogue = false;

	private InteractableScript focus;
	private Transform target;

	//PROPERTIES
	/// <summary>
	/// Get only property saying whether the player is in dialogue or not
	/// </summary>
	public bool InDialogue
	{
		get { return inDialogue; }
	}

	//Singleton stuff
	public static PlayerController PlayerControl
	{
		get; private set;
	}

	private void Awake()
	{
		if (PlayerControl != null && PlayerControl != this)
		{
			Destroy(gameObject);
		}
		else
		{
			PlayerControl = this;
		}
	}


	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
        if (target != null)
        {
			agent.SetDestination(target.position);
        }
    }

	public void OnMoveInteract()
	{
		Vector3 mousePos = Mouse.current.position.ReadValue();
		Ray ray = cam.ScreenPointToRay(mousePos);
		RaycastHit hit;

		//Only interact with UI objects
		if (UIManager.UI.CurrentUIMode == UIMode.Dialogue) return;

		//Player clicks in game
		// if ray hits interactable
		if (Physics.Raycast(ray, out hit, raycastRange, interactableMask))
		{
			InteractableScript interactable = hit.collider.GetComponent<InteractableScript>();
			if (interactable != null)
			{
				SetFocus(interactable);
				//agent.SetDestination(hit.point);
			}
		}

		// if the ray hits something walkable move the player towards it
		else if (Physics.Raycast(ray, out hit, raycastRange, movementMask))
		{
			agent.SetDestination(hit.point);
			RemoveFocus();
		}
	}

	private void SetFocus(InteractableScript newFocus)
	{
		if (newFocus == focus) return;

		//New focus object
		if (focus != null) focus.OnDefocused();
		focus = newFocus;
		newFocus.OnFocused();

		// Follow target
		FollowTarget(newFocus);
	}
	
	private void RemoveFocus()
	{
		if (focus != null) focus.OnDefocused();
		focus = null;

		// Stop moving
		StopFollowTarget();
	}

	public void FollowTarget (InteractableScript newTarget)
	{
		agent.stoppingDistance = newTarget.radius * 0.8f; // stop just inside radius
		target = newTarget.transform;
	}

	public void StopFollowTarget()
	{
		target = null;
	}
}
