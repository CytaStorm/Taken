using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public LayerMask movementMask;
    public int raycastRange = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if player has clicked an area, move player to it
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            // cast ray from camera to point clicked
            Vector3 mousePos = mouse.position.ReadValue();
            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit hit;

            // if the ray hits something walkable move the player towards it
            if (Physics.Raycast(ray, out hit, raycastRange, movementMask))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
