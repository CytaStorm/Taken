using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitUpdater : MonoBehaviour
{
    [SerializeField] SceneTwoManager sceneTwoManager;

    // Treat these like a dictionary key/value pairing. One speaker per texture.   
    [SerializeField] List<string> speakerNames;
    [SerializeField] List<Material> portraitMaterials;

    public Image portrait;
    private DialogueNode currentNode;
    public string speakerName; // public for debugging only!

    // Start is called before the first frame update
    void Start()
    {
        portrait = GetComponent<Image>();
        // Sanitize strings
        foreach (string name in speakerNames)
        {
            name.Trim();
            name.ToLower();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update current node
        currentNode = sceneTwoManager.traverser.currentNode;

        // If current node contains a new speaker, then switch portraits
        if (currentNode.Info.Contains(':'))
        {
            // Get speaker name
            int delimiterIndex = currentNode.Info.IndexOf(':');            
            speakerName = currentNode.Info.Substring(0, delimiterIndex);
            speakerName.ToLower();

            // Sanitize speaker name


            // Find index of associated portrait
            int portraitIndex = 0;
            for (int i = 0; i < speakerNames.Count; i++)
            {
                if (speakerName == speakerNames[i])
                {
                    portraitIndex = i;
                }
            }

            // Switch portraits
            portrait.material = portraitMaterials[portraitIndex];
        }
    }
}
