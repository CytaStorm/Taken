using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTraverser : MonoBehaviour
{
    //FIELDS
    //objects
    [SerializeField] DialogueGraph graph;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject parentOfButton;

    //Other values
    [SerializeField] private Vector2 buttonCenter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Sets up the first dialogue options in the encounter
    /// </summary>
    public void StartDialogue()
    {
        //Get the number of buttons that we need to spawn
        int numOfButtons = graph.StartNode.Links.Count;
        float startingY = buttonCenter.y - numOfButtons * 100 / 2;

        //Instantiate them
        for(int i = 0; /*DialogueNode*/ i < numOfButtons; i++)
        {
            GameObject button = Instantiate(
                buttonPrefab, 
                new Vector2(0, startingY + 100 * i), 
                Quaternion.identity, 
                parentOfButton.transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, startingY + 100 * i);
        }
    }
}
