using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : InteractableScript
{
    [Header("Character")]
    [SerializeField] private NavMeshAgent agent;
	[SerializeField] private Animator _animator;
    public GameObject Mesh;

    public bool canMove = true;
    protected bool isMoving = false;

    [Tooltip("Check this if the NPC only needs to move without interaction")] 
    [SerializeField] private bool isPuppet;

    protected override void Awake()
    {
        if (isPuppet) return;
        base.Awake();
    }

    protected override void Update()
    {
        //// Checks if NPC has arrived at the destination
        //if (agent.remainingDistance <= agent.stoppingDistance)
        //{
        //    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        //    {
        //        Debug.Log("NPC has arrived at the destination.");
        //    }
        //}

        if (_animator != null)
        {
            _animator.SetFloat("VelocityPercent", agent.velocity.magnitude / agent.speed);
        }

        if (isPuppet) return;
		base.Update();
	}

	protected void LookAtPlayer()
    {
        // If stationary, don't run method
        if (!canMove) { return; }

        // Face player position
        Vector3 destination = PlayerController.PlayerControl.transform.position;
        if (destination != null)
        {
            transform.LookAt(destination);
        }
    }

    protected void TurnToFacePlayer()
    {
        // If stationary, don't run method
        if (!canMove) { return; }

        // Get current angle
        Vector3 oldAngle = transform.rotation.eulerAngles; 
        // Face player position
        Vector3 destination = PlayerController.PlayerControl.transform.position;
        if (destination != null)
        {
            transform.LookAt(destination);
        }
        // Set angle to halfway between
        Vector3 angleDifference = transform.rotation.eulerAngles - oldAngle;
    }

    // Moves the NPC to the destination
    protected void MoveToPosition(Vector3 destination)
    {
        // Only move if the interactable has an agent AND a destination
        if ((agent != null) && (destination != null))
        {
            agent.SetDestination(destination); // Sets the destination for the NavMeshAgent
            isMoving = true; // Sets the NPC to moving state
        }
    }

	public override void Interact()
	{
		LookAtPlayer();
        base.Interact();
	}

    /// <summary>
    /// Disables Interaction for a certain amount of time.
    /// </summary>
    /// <param name="seconds">How long to disable interactions for</param>
    /// <returns></returns>
    public IEnumerator DisableInteractionForDuration(float seconds)
    {
        //Disable interaction
        isPuppet = true;

        yield return new WaitForSeconds(seconds);

        isPuppet = false;
    }

    public IEnumerator RotateToFaceDirection(Vector3 target)
    {
        yield return 0;

        while (transform.position.x != agent.destination.x && 
            transform.position.z != agent.destination.z)
        {
            yield return null;
        }

        var q = Quaternion.LookRotation(target - transform.position);

        while(Quaternion.Angle(transform.rotation, q) > 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 50 * Time.deltaTime);
            yield return null;
        }
    }
}
