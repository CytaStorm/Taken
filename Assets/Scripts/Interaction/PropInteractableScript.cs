using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropInteractableScript : InteractableScript
{
    public override IEnumerator Interact()
    {
        UIManager.UI.ChangeToDialogue();
        InteractionCount++;
        UpdateSceneGraph.Invoke(this);
        yield return null;
    }
}
