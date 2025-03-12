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
        if (_sceneController.FadesIn)
        {
            _canvasGroup.alpha = 1;
        }
        else
        {
            _canvasGroup.alpha = 0;
            print("not fade in");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_sceneController.FadesIn)
        {
            _canvasGroup.alpha = 1 - _sceneController.FadeInTimerPercent;
        }

		if (_sceneController.FadingOut)
		{
			_canvasGroup.alpha = _sceneController.FadeOutTimerPercent;
		}
    }
}
