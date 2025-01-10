using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
    private NavMeshAgent _sallosAgent;
    private Animator _sallosAnimator;
    private NavMeshAgent _eulyssAgent;
    private Animator _eulyssAnimator;
    private NavMeshAgent _akifAgent;
    private Animator _akifAnimator;


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
		eventFlags[0].onValueChange += delegate { PlayEventZero(); };

        // Get component data for all actors
        _sallosAgent = sallos.GetComponent<NavMeshAgent>();
        _sallosAnimator = sallos.GetComponent<Animator>();

        _eulyssAgent = eulyss.GetComponent<NavMeshAgent>();
        _eulyssAnimator = eulyss.GetComponent<Animator>();

        _akifAgent = akif.GetComponent<NavMeshAgent>();
        _akifAnimator = akif.GetComponent<Animator>();

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
    }

    private void PlayEventZero()
    {
        _sallosAgent.SetDestination(new Vector3(-2.11f, 0f, 7.35f));
        _eulyssAgent.SetDestination(new Vector3(-2.27f, 0f, 5.86f));
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
