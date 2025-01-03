using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class SceneThreeController : MonoBehaviour
{
    // Actors
    public GameObject sallos;
    public GameObject eulyss;
    public GameObject akif;
    
    public UIManager uiManager;
    public SceneTwoManager sceneTwoManager;

    // Event trigger variables
    public List<string> flagNames;
    private List<DialogueFlag> eventFlags;
    private double timer;

    // Actor components
    private NavMeshAgent sallosAgent;
    private Animator sallosAnimator;
    private NavMeshAgent eulyssAgent;
    private Animator eulyssAnimator;
    private NavMeshAgent akifAgent;
    private Animator akifAnimator;


    // Start is called before the first frame update
    // MAKE SURE EXECUTION ORDER IS SET TO LAST FOR THIS TO WORK
    void Start()
    {
        // Initialize timer
        timer = 0f;

        // Create eventFlags list based on string list flagNames
        eventFlags = new List<DialogueFlag>();
        foreach(string name in flagNames)
        {
            foreach (DialogueFlag flag in sceneTwoManager.dialogueFlags) 
            { 
                if (flag.Name == name)
                {
                    eventFlags.Add(flag);
                }
            }
        }

        // Assign all event methods to OnClick() events for all buttons

        // Get component data for all actors
        sallosAgent = sallos.GetComponent<NavMeshAgent>();
        sallosAnimator = sallos.GetComponent<Animator>();

        eulyssAgent = eulyss.GetComponent<NavMeshAgent>();
        eulyssAnimator = eulyss.GetComponent<Animator>();

        akifAgent = akif.GetComponent<NavMeshAgent>();
        akifAnimator = akif.GetComponent<Animator>();

        // Set initial positions of all actors
        sallos.transform.position = new Vector3(-0.41f, 0, 0.2f);
        eulyss.transform.position = new Vector3(-0.93f, 0, -0.49f);
        akif.transform.position = new Vector3(-3.95f, 0, 14.34f);
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer each frame
        timer += Time.deltaTime;

        // Play event when the specified flag is true
        if (eventFlags[0].IsTrue)
        {
            PlayEventZero();
        }
    }

    private void PlayEventZero()
    {
        akifAgent.destination = new Vector3(-0.9f, 0, 4.67f);
        akifAnimator.SetBool("Walking", true);
        StartCoroutine(PauseAllButtons(5f));

        if (akif.transform.position == akifAgent.destination)
        {
            eventFlags[0].IsTrue = false;
            akifAnimator.SetBool("Walking", false);
        }        
    }

    private IEnumerator PauseAllButtons(float seconds)
    {
        // disable all buttons
        Debug.Log("started");
        // wait for seconds amount of time

        yield return new WaitForSeconds(seconds);

        // enable all buttons
        Debug.Log("ended");
    }
}
