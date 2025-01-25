using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PortraitUpdater : MonoBehaviour
{
    [SerializeField] SceneTwoManager sceneTwoManager;

    // Treat these like a dictionary key/value pairing. One speaker per texture.   
    [SerializeField] List<string> speakerNames;
    [SerializeField] List<Texture> portraitTextures;

    private Image portrait;
    private DialogueNode currentNode;
    private string speakerName;

    // Start is called before the first frame update
    void Start()
    {
        portrait = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update current node
        currentNode = sceneTwoManager.traverser.currentNode;

        // If current node contains a new speaker, then switch portraits
        if (currentNode.Info.Contains(':'))
        {
            // 
        }
    }
}
