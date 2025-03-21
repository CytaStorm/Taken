using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitUpdater : MonoBehaviour
{
    [SerializeField] SceneController _sceneController;
    [SerializeField] UIManager _uiManager;

    // Treat these like a dictionary key/value pairing. One speaker per texture.
    [SerializeField] List<string> speakerNames;
    [SerializeField] List<Sprite> portraitTextures;

    public Image portrait;
    private GameObject portraitObject;
    private DialogueNode currentNode;
    public string speakerName; // public for debugging only!

    // Start is called before the first frame update
    void Start()
    {
        //portrait = GetComponent<Image>();
        portraitObject = portrait.gameObject;

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
        
        if (_uiManager.CurrentUIMode == UIMode.Dialogue)
        {
            // Activate portrait gameobject and any siblings during dialogue,
            // and update portrait texture.
            SetActivePortraitAndSiblings(true);
            UpdatePortraitSprite();
        }
        else
        {
            // Otherwise dectivate portrait and its siblings
            SetActivePortraitAndSiblings(false);
        }
    }

    private void UpdatePortraitSprite()
    {
        // Update current node
        currentNode = _sceneController.Traverser.currentNode;

        if (currentNode == null)
        {
            return;
        }

        // If current node contains a new speaker, then switch portraits
        if (currentNode.Info.Contains(':'))
        {
            // Get speaker name
            int delimiterIndex = currentNode.Info.IndexOf(':');
            speakerName = currentNode.Info.Substring(0, delimiterIndex);

            // Remove xml tags
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
        else
        {
            // If there is no speaker, deactiviate the portrait and its siblings
            SetActivePortraitAndSiblings(false);
        }
    }

    /// <summary>
    /// The border for the portrit is a sibling of the portrait, so
    /// this script makes sure it gets deactivated
    /// </summary>
    /// <param name="setState"></param>
    private void SetActivePortraitAndSiblings(bool setState)
    {
        portraitObject.SetActive(setState);

        // Could be optimized by creating global references to every child
        foreach (Transform childTransform in portraitObject.transform.parent)
        {
            childTransform.gameObject.SetActive(setState);
        }
    }
}
