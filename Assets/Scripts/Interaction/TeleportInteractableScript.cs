using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportInteractableScript : InteractableScript
{
    [SerializeField] private GameObject _teleportTo;
    [SerializeField] private Vector3 _newCameraOffset;

    //protected override void Update()
    //{

    //}
    public override void Interact()
    {
        PlayerController.Instance.RemoveFocus();
        PlayerController.Instance.Agent.isStopped = true;
        PlayerController.Instance.Agent.ResetPath();
        PlayerController.Instance.Agent.Warp(_teleportTo.transform.position);
        PlayerController.Instance.Cam.gameObject.
            GetComponent<CameraController>().offset = _newCameraOffset;
    }
}
