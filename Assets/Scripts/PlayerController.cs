using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

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

	[SerializeField] private float rotationDistance;

	public InteractableScript Focus;

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
		if (Focus != null)
		{
			_agent.SetDestination(Focus.InteractionPoint.transform.position);

			//if (transform.position == _target.InteractionPoint.transform.position)
			if (Vector3.Distance(transform.position, Focus.InteractionPoint.transform.position) < rotationDistance)
			{
				Vector3 direction = Focus.transform.position - transform.position;
				Quaternion targetRotation = Quaternion.LookRotation(direction);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 150 * Time.deltaTime);
			}
		}
		//Control eulyss walking anim
		_animator.SetFloat("VelocityPercent", _agent.velocity.magnitude / _agent.speed);
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
		if (newFocus == Focus) return;

		//New focus object
		//if (Focus != null) Focus.OnDefocused();
		Focus = newFocus;
	}
	
	public void RemoveFocus()
	{
		//if (Focus != null) Focus.OnDefocused();
		Focus = null;
	}

	public Vector3 GetDestination()
	{
		return _agent.destination;
	}
}
