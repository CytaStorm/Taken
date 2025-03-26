using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitUpdater : MonoBehaviour
{
    [SerializeField] SceneController _sceneController;

    // Treat these like a dictionary key/value pairing. One speaker per texture.
    [SerializeField] List<string> speakerNames;
    [SerializeField] List<Sprite> portraitTextures;

    public Image portrait;
    private NewDialogueNode currentNode;
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
        currentNode = _sceneController.Traverser.CurrentNode;

		if (currentNode == null)
		{
			return;
		}
        // If current node contains a new speaker, then switch portraits
        if (currentNode.Text.Contains(':'))
        {
            // Get speaker name
            int delimiterIndex = currentNode.Text.IndexOf(':');            
            speakerName = currentNode.Text.Substring(0, delimiterIndex);       
            
            // Remove html tags
            while (speakerName.Contains('<'))
            {
                
                int startIndex = speakerName.IndexOf('<');
                int endIndex = speakerName.IndexOf('>');
                int count = endIndex - startIndex + 1;
                speakerName = speakerName.Remove(startIndex, count);
                
            }
            speakerName = speakerName.ToLower();

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
            // NOTE: if this doesn't work, try having a gameobject for each character portrait
            // that enables/disables itself when the provided character is the speaker

            //portrait.material = portraitMaterials[portraitIndex];
            portrait.sprite = portraitTextures[portraitIndex];
        }
    }
}
