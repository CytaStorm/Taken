using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneThreeCutscene : MonoBehaviour
{
    // Actors
    public GameObject sallos;
    public GameObject eulyss;
    public GameObject akif;
    public GameObject goon;
    
    public UIManager uiManager;
    public SceneTwoManager sceneTwoManager;

    // Event trigger variables
    public List<string> flagNames;
    private List<DialogueFlag> eventFlags;
    private double timer;

    // Actor components
    private NavMeshAgent _sallosAgent;
    private Animator _sallosAnimator;
    private NavMeshAgent _eulyssAgent;
    private Animator _eulyssAnimator;
    private NavMeshAgent _akifAgent;
    private Animator _akifAnimator;
    private NavMeshAgent _goonAgent;
    private Animator _goonAnimator;


    // Start is called before the first frame update
    // MAKE SURE EXECUTION ORDER IS SET TO LAST FOR THIS TO WORK
    void Start()
    {
        // Initialize timer
        timer = 0f;

        //Create eventFlags list based on string list flagNames
        eventFlags = new List<DialogueFlag>();
        foreach(string name in flagNames)
        {
            foreach (DialogueFlag flag in sceneTwoManager.dialogueFlags) 
            {
                if (flag.Name != name) continue;

                eventFlags.Add(flag);
            }
        }

        // Assign all event methods to OnClick() events for all buttons
        eventFlags[0].onValueChange += delegate { WalkLeft(); };
        eventFlags[1].onValueChange += delegate { EnterAkif(); };
        eventFlags[2].onValueChange += delegate { EnterGoon(); };
        eventFlags[3].onValueChange += delegate { WalkCloser(); };
        eventFlags[4].onValueChange += delegate { LookForEscape(); };
        eventFlags[5].onValueChange += delegate { Stab(); };
        eventFlags[6].onValueChange += delegate { Disappear(); };

        // Get component data for all actors
        _sallosAgent = sallos.GetComponent<NavMeshAgent>();
        _sallosAnimator = sallos.GetComponent<Animator>();

        _eulyssAgent = eulyss.GetComponent<NavMeshAgent>();
        _eulyssAnimator = eulyss.GetComponent<Animator>();

        _akifAgent = akif.GetComponent<NavMeshAgent>();
        _akifAnimator = akif.GetComponent<Animator>();

        _goonAgent = goon.GetComponent<NavMeshAgent>();
        _goonAnimator = goon.GetComponent<Animator>();

        // Set initial positions of all actors
        sallos.transform.position = new Vector3(-0.41f, 0, 0.2f);
        eulyss.transform.position = new Vector3(-0.93f, 0, -0.49f);
        akif.transform.position = new Vector3(-4.92f, 0, 16.85f);
        goon.transform.position = new Vector3(-12.08f, 0, 4.37f);
    }

	// Update is called once per frame
	void Update()
    {
        // Update timer each frame
        timer += Time.deltaTime;
    }

    private void WalkLeft()
    {
        // Move sallos and eulyss along forest trail
        _sallosAgent.SetDestination(new Vector3(-2.11f, 0f, 7.35f));
        _eulyssAgent.SetDestination(new Vector3(-2.27f, 0f, 5.86f));
        StartCoroutine(PauseAllButtons(5f));
    }

    private void EnterAkif()
    {
        // Move akif towards sallos and eulyss
        _akifAgent.SetDestination(new Vector3(-2.990002f, 0f, 10.45f));
        StartCoroutine(PauseAllButtons(5f));
    }

    private void EnterGoon()
    {
        // Move the goon to corner sallos and eulyss
        _goonAgent.SetDestination(new Vector3(-4.56f, 0f, 7.1f));
        StartCoroutine(PauseAllButtons(5f));
    }

    private void WalkCloser()
    {
        // Move both akif and the goon closer to sallos and eulyss
        _akifAgent.SetDestination(new Vector3(-2.978f, 0f, 9.038f));
        _goonAgent.SetDestination(new Vector3(-3.66f, 0f, 7.04f));
        StartCoroutine(PauseAllButtons(5f));
    }

    private void LookForEscape()
    {
        // Make sallos and eulyss look back and forth
        StartCoroutine(PauseAllButtons(5f));
    }

    private void Stab()
    {
        _akifAnimator.SetTrigger("Stab");
        StartCoroutine(PauseAllButtons(5f));
    }

    private void Disappear()
    {
        StartCoroutine(PauseAllButtons(5f));
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
