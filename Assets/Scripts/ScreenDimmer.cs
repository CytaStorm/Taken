using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenDimmer : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private SceneController _sceneController;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		if (_sceneController.sceneChangeActive)
		{
			_canvasGroup.alpha = _sceneController.timerPercent;
		}
    }
}
