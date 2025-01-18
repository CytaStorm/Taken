using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string flagName;
    [SerializeField] string destinationScene;
    public float delayTime;
    [SerializeField] SceneTwoManager sceneManager;
    [SerializeField] bool dimScreen = true;

    public bool sceneChangeActive = false;
    private float sceneChangeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Search SceneTwoManager for matching flag
        foreach (DialogueFlag flag in sceneManager.dialogueFlags)
        {
            if ((flag.Name == flagName) && (flag.IsTrue))
            {
                sceneChangeActive = true;
            }
        }

        if (sceneChangeActive)
        {
            sceneChangeTimer += Time.deltaTime;

            if (sceneChangeTimer > delayTime)
            {
                SceneManager.LoadScene(destinationScene);
            }
        }
    }
}
