using UnityEngine;

public class ObstacleScript : InteractableScript
{
    public UIManager _UIManager;

    protected override void Update()
    {
        // Checks if NPC is focused by the player
        if (isFocus && canInteract)
        {
            float distance = Vector3.Distance(
                PlayerController.PlayerControl.gameObject.transform.position,
                gameObject.transform.position);

            // If its able to be interacted with, Interact
            if (distance <= radius && !hasInteracted && !isMoving)
            {
                Debug.Log("INTERACT");
                _UIManager.ChangeToDialogue(); // Chest uses dialogue interface
                Interact();

                // Disables further interactions once chest is open
                hasInteracted = true;                
                canInteract = false;
                Debug.Log("Interaction disabled after initial interaction.");
                
            }
            else if (distance > radius && !isMoving)
            {
                hasInteracted = false;
            }
        }
    }

    public override void Interact()
    {
        UpdateSceneGraph.Invoke(this);        
    }
}
