using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private UIManager _UIManager;

	[SerializeField] private PlayerInput playerInput; 

	[SerializeField] private Camera cam;
	[SerializeField] private NavMeshAgent _agent;
	[SerializeField] private int raycastRange = 100;
	[SerializeField] private LayerMask movementMask;
	[SerializeField] private LayerMask interactableMask;
	[SerializeField] private bool inDialogue = false;
	[SerializeField] private Animator _animator;

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
			_agent.SetDestination(target.position);
        }
		//Control eulyss walking anim
		_animator.SetBool("Walking", _agent.velocity.magnitude > 0);
    }

	public void OnMoveInteract(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;
		Vector3 mousePos = Mouse.current.position.ReadValue();
		Ray ray = cam.ScreenPointToRay(mousePos);
		RaycastHit hit;

		//Only interact with UI objects
		if (_UIManager.CurrentUIMode == UIMode.Dialogue) return;

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
			_agent.SetDestination(hit.point);
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
		_agent.stoppingDistance = newTarget.radius * 0.8f; // stop just inside radius
		target = newTarget.transform;
	}

	public void StopFollowTarget()
	{
		target = null;
		_agent.stoppingDistance = 0;
	}

	public Vector3 GetDestination()
	{
		return _agent.destination;
	}
}
