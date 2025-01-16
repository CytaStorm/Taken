using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;  // Import the AI Navigation namespace

public abstract class InteractableScript : MonoBehaviour
{
    public UIManager _UIManager;
    [SerializeField] protected NavMeshAgent agent;

    public float radius = 3f;
    public bool hasLimitedInteractions = true;
    public bool canMove = true;

    protected bool isFocus = false;       
    protected bool hasInteracted = false; 
    protected bool isMoving = false;      
    protected bool canInteract = true;    
    protected int interactionCount = 0;
    protected bool isHighlighted = false;
    protected float highlightTimer = 0f;

    [SerializeField] private TextAsset twineFile;
    public DialogueGraph Graph { get; protected set; }
    public UnityEvent<InteractableScript> UpdateSceneGraph { get; private set; }

	[SerializeField] protected bool IsPuppet;

    protected virtual void Awake()
    {
		//print(twineFile);
		if (IsPuppet) return;
        Graph = new DialogueGraph(twineFile);
        UpdateSceneGraph = new UnityEvent<InteractableScript>();
        //Debug.Log(gameObject.name + " " + UpdateSceneGraph);


    }

	public abstract void Interact();

    protected virtual void Update()
	{
        if (isHighlighted && !isFocus)
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = new Color(Mathf.Abs(Mathf.Sin(highlightTimer)), 1, 1);
                print(renderer.material.color);
            }
        }
        highlightTimer += Time.deltaTime;

        //Interaction filter
        if (!isFocus || !canInteract)
		{
			return;
		}

		float distance = Vector3.Distance(
			PlayerController.PlayerControl.gameObject.transform.position,
			gameObject.transform.position);
		// If its able to be interacted with, Interact
		if (distance <= radius && !hasInteracted && !isMoving)
		{
			Debug.Log("INTERACT");
			_UIManager.ChangeToDialogue();
			Interact();
			hasInteracted = true;
			interactionCount++;
		}
		else if (distance > radius && !isMoving)
		{
			hasInteracted = false;
		}
    }

	public void OnDefocused()
	{
		isFocus = false;
		hasInteracted = false;
	}

	public void OnFocused()
	{
		isFocus = true;
	}

    private void OnMouseEnter()
    {
		Debug.Log("mouse entered");

        highlightTimer = 0f;
        isHighlighted = true;
    }

    private void OnMouseExit()
    {
        Debug.Log("mouse exited");
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = Color.white;            
        }
        isHighlighted = false;
    }

    //private void OnDrawGizmosSelected()
    //{
    //	Gizmos.color = Color.yellow;
    //	Gizmos.DrawWireSphere(interactionTransform.position, radius);
    //}
}
