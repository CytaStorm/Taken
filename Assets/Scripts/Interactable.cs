using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	[SerializeField] private float radius = 3f;
	[SerializeField] private Transform interactionTransform;
	private bool isFocus = false;
	private bool hasInteracted = false;
	public virtual void Interact()
	{
		Debug.Log("Interacting with " + transform.name);
	}

    // Update is called once per frame
    void Update()
    {
        if (isFocus)
        {
            float distance = Vector3.Distance(
                PlayerController.PlayerControl.gameObject.transform.position,
                interactionTransform.position);

            if (distance <= radius)
            {
                // Only interact if it hasn't interacted before
                if (!hasInteracted)
                {
                    Debug.Log("INTERACT");
                    Interact();
                    hasInteracted = true;
                }
            }
            else
            {
                // Allow additional interactions
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

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}
}
