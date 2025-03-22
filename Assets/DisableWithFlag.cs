using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWithFlag : MonoBehaviour
{
    public List<string> flagName;
    public SceneController sceneManager;
    public GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {
        targetObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Limit O(n) searches to when the object is deactivated
        if (targetObject.activeSelf == true)
        {
            // Search SceneTwoManager for matching flag
            foreach (NewDialogueFlag flag in sceneManager.DialogueFlags)
            {
                if ((flag.Names.Equals(flagName)) && (flag.IsTrue))
                {
                    targetObject.SetActive(false);
                }
            }
        }
    }
}
