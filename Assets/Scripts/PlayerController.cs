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
	[SerializeField] private PlayerInput _playerInput; 

	[SerializeField] private Camera _cam;
	[SerializeField] private NavMeshAgent _agent;
	[SerializeField] private int _raycastRange = 100;
	[SerializeField] private LayerMask _movementMask;
	[SerializeField] private LayerMask _interactableMask;

	private Interactable _focus;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void OnMoveInteract(InputAction.CallbackContext ctx)
	{
		Vector3 mousePos = Mouse.current.position.ReadValue();
		Ray ray = _cam.ScreenPointToRay(mousePos);
		RaycastHit hit;

		//Only interact with UI objects
		if (UIController.UI.CurrentUIMode == UIMode.Dialogue) return;

		//Player clicks in game
		// if ray hits interactable
		if (Physics.Raycast(ray, out hit, _raycastRange, _interactableMask))
		{
			Interactable interactable = hit.collider.GetComponent<Interactable>();
			if (interactable != null)
			{
				SetFocus(interactable);
				_agent.SetDestination(hit.point);
			}
		}

		// if the ray hits something walkable move the player towards it
		else if (Physics.Raycast(ray, out hit, _raycastRange, _movementMask))
		{
			_agent.SetDestination(hit.point);
		}
	}

	private void SetFocus(Interactable newFocus)
	{
		if (newFocus == _focus) return;

		//New focus object
		if (_focus != null) _focus.OnDefocused();
		_focus = newFocus;
		newFocus.OnFocused();
	}
	
	private void RemoveFocus()
	{
		if (_focus != null) _focus.OnDefocused();
		_focus = null;
	}
}
