using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;  // Import the AI Navigation namespace

public abstract class InteractableScript : MonoBehaviour
{
              
    [SerializeField] protected Transform interactionTransform;
    [SerializeField] protected Vector3 destinationPosition;
    [SerializeField] protected NavMeshAgent agent;

    public float radius = 3f;
    public bool hasLimitedInteractions = true;
    public bool canMove = true;

    protected bool isFocus = false;       
    protected bool hasInteracted = false; 
    protected bool isMoving = false;      
    protected bool canInteract = true;    
    protected int interactionCount = 0;

    [SerializeField] private TextAsset twineFile;
    public DialogueGraph Graph { get; protected set; }
    public UnityEvent<InteractableScript> UpdateSceneGraph { get; private set; }

    protected virtual void Awake()
    {
		//print(twineFile);
        Graph = new DialogueGraph(twineFile);
        UpdateSceneGraph = new UnityEvent<InteractableScript>();
        //Debug.Log(gameObject.name + " " + UpdateSceneGraph);
    }

	public abstract void Interact();

    protected virtual void Update()
    {
        // Checks if NPC is focused by the player
        if (isFocus && canInteract)
        {
            float distance = Vector3.Distance(
                PlayerController.PlayerControl.gameObject.transform.position,
                interactionTransform.position);
            // If its able to be interacted with, Interact
            if (distance <= radius && !hasInteracted && !isMoving)
            {
                Debug.Log("INTERACT");
                UIManager.UI.ChangeToDialogue();
                Interact();
                hasInteracted = true;
                interactionCount++;
            }
            else if (distance > radius && !isMoving)
            {
                hasInteracted = false;
            }
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

	//private void OnDrawGizmosSelected()
	//{
	//	Gizmos.color = Color.yellow;
	//	Gizmos.DrawWireSphere(interactionTransform.position, radius);
	//}
}
