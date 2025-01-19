using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenDimmer : MonoBehaviour
{
    [SerializeField] SceneChanger sceneChanger;

    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneChanger.sceneChangeActive)
        {
            canvasGroup.alpha = sceneChanger.timerPercent;
        }
        
    }
}
