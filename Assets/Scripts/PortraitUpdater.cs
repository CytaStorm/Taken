using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitUpdater : MonoBehaviour
{
    SceneController _sceneController;
    [SerializeField] UIManager _uiManager;

    [SerializeField] Image _portrait;

    // Treat these like a dictionary key/value pairing. One speaker per texture.
    [SerializeField] List<string> _speakerNames;
    [SerializeField] List<Sprite> _portraitTextures;

    
    private GameObject _portraitObject;
    private List<Transform> _portraitSiblingTransforms;
    private DialogueNode _currentNode;
    private string _speakerName; // make public for debugging only!

    // Start is called before the first frame update
    void Start()
    {
        _sceneController = SceneController.Instance;
        // Initialize global variables
        _portraitObject = _portrait.gameObject;

        _portraitSiblingTransforms = new List<Transform>();
        foreach (Transform childTransform in _portraitObject.transform.parent)
        {
            _portraitSiblingTransforms.Add(childTransform);
        }

        // Sanitize strings
        foreach (string name in _speakerNames)
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
        _currentNode = _sceneController.Traverser.CurrentNode;

        if (_currentNode == null)
        {
            return;
        }

        // If current node contains a new speaker, then switch portraits
        if (_currentNode.Text.Contains(':'))
        {
            // Get speaker name
            int delimiterIndex = _currentNode.Text.IndexOf(':');
            _speakerName = _currentNode.Text.Substring(0, delimiterIndex);

            // Remove xml tags
            while (_speakerName.Contains('<'))
            {

                int startIndex = _speakerName.IndexOf('<');
                int endIndex = _speakerName.IndexOf('>');
                int count = endIndex - startIndex + 1;
                _speakerName = _speakerName.Remove(startIndex, count);

            }
            _speakerName = _speakerName.ToLower();

            // Find index of associated portrait
            int portraitIndex = 0;
            for (int i = 0; i < _speakerNames.Count; i++)
            {
                if (_speakerName == _speakerNames[i])
                {
                    portraitIndex = i;
                }
            }

            // Switch portraits
            _portrait.sprite = _portraitTextures[portraitIndex];
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
    /// <param name="setState">"true" to set everything active,
    /// "false" to set everything inactive</param>
    private void SetActivePortraitAndSiblings(bool setState)
    {
        _portraitObject.SetActive(setState);

        // Could be optimized by creating global references to every child
        foreach (Transform sibling in _portraitSiblingTransforms)
        {
            sibling.gameObject.SetActive(setState);
        }
    }
}
