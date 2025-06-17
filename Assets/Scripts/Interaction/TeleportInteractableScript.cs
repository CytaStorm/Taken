using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportInteractableScript : InteractableScript
{
	[SerializeField] private GameObject _teleportTo;
	[SerializeField] private Vector3 _newCameraOffset;

	//protected override void Update()
	//{

	//}
	public override IEnumerator Interact()
	{
		//fading in, go back to fade back out
		if (SceneController.Instance.FadingIn)
		{
			if (SceneController.Instance.LatestFade != null)
			{
				SceneController.Instance.StopCoroutine(
					SceneController.Instance.LatestFade);
			}
			SceneController.Instance.FadeBackOut(1);
			yield return new WaitForSeconds(
				SceneController.Instance.TimeRemainingOnFadeOut());
		}  
		//Otherwise proceed as normal
		else
		{
			SceneController.Instance.Fade(1);
			yield return new WaitForSeconds(1);
		}

		PlayerController.Instance.Agent.isStopped = true;
		PlayerController.Instance.Agent.ResetPath();
		PlayerController.Instance.Agent.Warp(_teleportTo.transform.position);
		PlayerController.Instance.Cam.gameObject.
			GetComponent<CameraController>().offset = _newCameraOffset;
		yield return null;
	}
}
