using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, -1f, -1.2f);
    public float zoom = 10f;
    public float pitch = 2f;
    private CameraState camState = CameraState.Movement;
    [SerializeField] private Camera cam;
    private float timer = 0;
    [SerializeField] private float zoomInDuration;
    [SerializeField] private float zoomInMagnitude;

    //Enum for FSM
    [SerializeField] private enum CameraState
    {
        Movement, 
        ZoomingIn, 
        Dialogue, 
        ZoomingOut
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Updates every frame but after the logic going on in Update()
    private void LateUpdate()
    {
        // follow target after each frame update
        transform.position = target.position - (offset * zoom);
        // look at target
        transform.LookAt(target.position + (Vector3.up * pitch));


        //Finite State Machine to keep track of camera behavior
        switch (camState)
        {
            //MOVEMENT STATE during typical gameplay - ie moving around 
            case CameraState.Movement:
                //Change to ZoomingIn --> Dialogue
                if (PlayerController.PlayerControl.InDialogue)
                {
                    //Switch the camera state
                    camState = CameraState.ZoomingIn;
                    timer = 0;
                }
                break;

            //DIALOGUE STATE (be ready to switch back to zoom out)
            case CameraState.Dialogue:
                if (PlayerController.PlayerControl.InDialogue == false)
                {
                    //Vector3 newPos = target.position - (offset * zoom);
                    //distanceToNewPos = newPos - transform.position;

                    //Detect if we need to zoom back out
                    camState = CameraState.ZoomingOut;
                    timer = 0;
                }
                break;

            //ZOOMING IN STATE
            case CameraState.ZoomingIn:
                //Adjust camera based on how far it needs to go and how much time has passed
                cam.orthographicSize -= zoomInMagnitude * (Time.deltaTime / zoomInDuration);
                //transform.position += distanceToNewPos * (Time.deltaTime / zoomInDuration);

                //update timer
                timer += Time.deltaTime;
                if(timer >= zoomInDuration)
                {
                    camState = CameraState.Dialogue;
                }
                break;

            //ZOOMING OUT STATE
            case CameraState.ZoomingOut:
                //Adjust camera based on how far it needs to go and how much time has passed
                cam.orthographicSize += zoomInMagnitude * (Time.deltaTime / zoomInDuration);

                //update timer
                timer += Time.deltaTime;
                if (timer >= zoomInDuration)
                {
                    camState = CameraState.Movement;
                }
                break;
        }
    }
}
