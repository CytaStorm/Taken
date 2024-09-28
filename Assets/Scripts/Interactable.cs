using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // Import the AI Navigation namespace

public class Interactable : MonoBehaviour
{
    [SerializeField] private float radius = 3f;           
    [SerializeField] private Transform interactionTransform;  
    [SerializeField] private Vector3 destinationPosition;  


    private bool isFocus = false;       
    private bool hasInteracted = false; 
    private bool isMoving = false;      
    private bool canInteract = true;    
    private int interactionCount = 0;   
    private NavMeshAgent agent;         



    void Start()
    {
        // Get the NavMeshAgent component attached to the NPC
        agent = GetComponent<NavMeshAgent>();
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + transform.name);
        MoveToPosition(destinationPosition);
    }

    // Moves the NPC to the destination
    private void MoveToPosition(Vector3 destination)
    {
        if (agent != null)
        {
            agent.SetDestination(destination); // Sets the destination for the NavMeshAgent
            isMoving = true; // Sets the NPC to moving state
        }
    }

    
    void Update()
    {
        // Checks if NPC is focused by the player
        if (isFocus && canInteract)
        {
            float distance = Vector3.Distance(
                PlayerScript.Player.PlayerController.transform.position,
                interactionTransform.position);
            // If its able to be interacted with, Interact
            if (distance <= radius && !hasInteracted && !isMoving)
            {
                Debug.Log("INTERACT");
                UIController.UI.ChangeToDialogue();
                Interact();
                hasInteracted = true;
                interactionCount++;

                // Disables further interactions after the second interaction
                if (interactionCount >= 2)
                {
                    canInteract = false;
                    Debug.Log("Interaction disabled after second interaction.");
                }
            }
            else if (distance > radius && !isMoving)
            {
                hasInteracted = false;
            }
        }

        // Checks if NPC has arrived at the destination
        if (isMoving && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                isMoving = false;  // NPC has finished moving
                Debug.Log("NPC has arrived at the destination.");
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

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}
}
