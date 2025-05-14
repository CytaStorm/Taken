using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobileUITrigger : MonoBehaviour
{
    public MobileUIBehavior target;
    private bool isMouseWithin = false;

    private void OnMouseUp()
    {
        print("Trigger clicked!");

        // return early if mouse isn't within trigger element
        if (!isMouseWithin)
        {
            return;
        }

        // move target
        target.StartMoving();
    }

    private void OnMouseEnter()
    {
        isMouseWithin = true;
    }

    private void OnMouseExit()
    {
        isMouseWithin = false;
    }
}
