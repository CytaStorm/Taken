using UnityEngine;

public class EnableWithFlag : MonoBehaviour
{
    public string flagName;
    public SceneController sceneManager;
    public GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {
        targetObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Limit O(n) searches to when the object is deactivated
        if (targetObject.activeSelf == false)
        {
            // Search SceneTwoManager for matching flag
            foreach (DialogueFlag flag in sceneManager.DialogueFlags)
            {
                if ((flag.Name == flagName) && (flag.IsTrue))
                {
                    targetObject.SetActive(true);
                }
            }
        }
    }
}
