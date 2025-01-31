using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitPositioner : MonoBehaviour
{
    [SerializeField] LayoutElement dialogueText;
    [SerializeField] Vector2 buffer = new Vector2(10f, 5f);

    private Image portrait;
    private RectTransform portraitRect;
    private RectTransform dialogueRect;


    // Start is called before the first frame update
    void Start()
    {        
        portrait = GetComponent<Image>();
        portraitRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        dialogueRect = dialogueText.GetComponent<RectTransform>();

        // Update portrait x position
        // NOTE: Image anchor point must be in the center of the screen!
        float xPos = (Screen.width / 2f) - (portraitRect.rect.width / 2f) - dialogueRect.rect.width;
        float yPos = - (Screen.height / 2f) + (portraitRect.rect.height / 2f);
        portraitRect.anchoredPosition = new Vector2(xPos - buffer.x, yPos + buffer.y);
    }
}
