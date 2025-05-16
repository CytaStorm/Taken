using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
	private UIManager _UIManager;

	[SerializeField] private PlayerInput playerInput;

	public Camera Cam;
	public NavMeshAgent Agent;
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
	public static PlayerController Instance
	{
		get; private set;
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}


	// Start is called before the first frame update
	void Start()
	{
		_UIManager = UIManager.UI;
	}

	// Update is called once per frame
	void Update()
	{
		if (Focus != null)
		{
			Agent.SetDestination(Focus.InteractionPoint.transform.position);

			//casted to ints to prevent floating point imprecision
			if ((int)Vector3.Distance(transform.position, Focus.InteractionPoint.transform.position) == (int)rotationDistance)
			{
				Vector3 direction = Focus.transform.position - transform.position;
				Quaternion targetRotation = Quaternion.LookRotation(direction);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 150 * Time.deltaTime);
			}
		}
		//Control eulyss walking anim
		_animator.SetFloat("VelocityPercent", Agent.velocity.magnitude / Agent.speed);
	}

    public void OnMoveInteract(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;
		Vector3 mousePos = Mouse.current.position.ReadValue();
		Ray ray = Cam.ScreenPointToRay(mousePos);
		RaycastHit hit;

		// Don't move character if UI is clicked
		if (_UIManager.CurrentUIMode == UIMode.Dialogue) return;
		if (!(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0)))
		{
			return;
		}

        //Player clicks in game
        // if ray hits Interactable
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
			Agent.SetDestination(hit.point);
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
		return Agent.destination;
	}
}
