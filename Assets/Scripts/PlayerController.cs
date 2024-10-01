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

	private Interactable focus;

	//PROPERTIES
	/// <summary>
	/// Get only property saying whether the player is in dialogue or not
	/// </summary>
	public bool InDialogue
	{
		get { return inDialogue; }
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void OnMoveInteract (InputAction.CallbackContext ctx)
	{
		Vector3 mousePos = Mouse.current.position.ReadValue();
		Ray ray = cam.ScreenPointToRay(mousePos);
		RaycastHit hit;

		//Only interact with UI objects
		if (UIController.UI.CurrentUIMode == UIMode.Dialogue) return;

		//Player clicks in game
		// if ray hits interactable
		if (Physics.Raycast(ray, out hit, raycastRange, interactableMask))
		{
			Interactable interactable = hit.collider.GetComponent<Interactable>();
			if (interactable != null)
			{
				SetFocus(interactable);
				agent.SetDestination(hit.point);
			}
		}

		// if the ray hits something walkable move the player towards it
		else if (Physics.Raycast(ray, out hit, raycastRange, movementMask))
		{
			agent.SetDestination(hit.point);
		}
	}

	/// <summary>
	/// Sets focus to an interactable object.
	/// </summary>
	/// <param name="newFocus">Interactable to focus on.</param>
	private void SetFocus(Interactable newFocus)
	{
		if (newFocus == focus) return;

		//New focus object
		if (focus != null) focus.OnDefocused();
		focus = newFocus;
		newFocus.OnFocused();
	}
	
	/// <summary>
	/// Removes focus.
	/// </summary>
	private void RemoveFocus()
	{
		if (focus != null) focus.OnDefocused();
		focus = null;
	}

	/// <summary>
	/// Stops the player from moving and resets the player's destination.
	/// </summary>
	public void StopMoving()
	{
		agent.isStopped = true;
		agent.destination = transform.position;
	}

	/// <summary>
	/// Unfreezes the player agent.
	/// </summary>
	public void StartMoving()
	{
		agent.isStopped = false;
	}
}
