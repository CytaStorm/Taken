using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CutsceneController : SceneController
{
    // Actors
    public GameObject sallos;
    public GameObject eulyss;
    public GameObject akif;
    public GameObject goon;

    public Material sallosMaterial;

    // Event trigger variables
    public List<string> flagNames;
    private List<NewDialogueFlag> eventFlags;
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
    new void Start()
    {
        base.Start();

        // REMOVED -- done by parent
        //sallosMaterial.SetFloat("_Dissolve_Effect", 0);

        // Initialize timer
        timer = 0f;

        //Create eventFlags list based on string list flagNames
        eventFlags = new List<NewDialogueFlag>();
        foreach(string name in flagNames)
        {
            foreach (NewDialogueFlag flag in DialogueFlags) 
            {
                if (!flag.Names.Contains(name)) continue;

                eventFlags.Add(flag);
            }
        }

        // Assign all event methods to OnClick() events for all buttons
        
        eventFlags[0].onValueChange += delegate { WalkLeft(); };
        eventFlags[1].onValueChange += delegate { EnterAkif(); };
        eventFlags[2].onValueChange += delegate { EnterGoon(); };
        eventFlags[3].onValueChange += delegate { WalkCloser(); };
        eventFlags[4].onValueChange += delegate { Stab(); };
        eventFlags[5].onValueChange += delegate { Disappear(); };
		eventFlags[6].onValueChange += delegate { Reach(); };
        

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
	new void Update()
    {
        base.Update();

        // Update timer each frame
        timer += Time.deltaTime;
    }

    private void WalkLeft()
    {
        // Move sallos and eulyss along forest trail
        _sallosAgent.SetDestination(new Vector3(-2.11f, 0f, 7.35f));
        _eulyssAgent.SetDestination(new Vector3(-2.27f, 0f, 5.86f));
        StartCoroutine(_UIManager.PauseAllButtons(3f));
    }

    private void EnterAkif()
    {
        // Move akif towards sallos and eulyss
        _akifAgent.SetDestination(new Vector3(-2.990002f, 0f, 10.45f));
        StartCoroutine(_UIManager.PauseAllButtons(2.7f));
    }

    private void EnterGoon()
    {
        // Move the goon to corner sallos and eulyss
        _goonAgent.SetDestination(new Vector3(-4.56f, 0f, 7.1f));
        _sallosAnimator.SetTrigger("LookLeftTrigger");
        _eulyssAnimator.SetTrigger("LookLeftTrigger");
        StartCoroutine(ChangeLookLeft(_sallosAnimator, 1, 0.5f));
        StartCoroutine(ChangeLookLeft(_eulyssAnimator, 1, 0.66f));
        StartCoroutine(_UIManager.PauseAllButtons(2.9f));
    }

    private void WalkCloser()
    {
        // Move both akif and the goon closer to sallos and eulyss
        _akifAgent.SetDestination(new Vector3(-2.798f, 0f, 8.952f));
        _goonAgent.SetDestination(new Vector3(-3.66f, 0f, 7.04f));
        StartCoroutine(ChangeLookLeft(_sallosAnimator, 0, 0.5f));
        StartCoroutine(ChangeLookLeft(_eulyssAnimator, 0, 0.45f));
        StartCoroutine(_UIManager.PauseAllButtons(1.3f));
    }

    private void Reach()
    {
		_eulyssAnimator.SetTrigger("ReachOut");
    }

    private void Stab()
    {
        _akifAnimator.SetTrigger("Stab");
        _sallosAnimator.SetTrigger("StepBack");
        StartCoroutine(_UIManager.PauseAllButtons(0.7f));
    }

    private void Disappear()
    {
        StartCoroutine(FadeAway(5f));
        StartCoroutine(_UIManager.PauseAllButtons(5f));
    }

    private IEnumerator FadeAway(float seconds)
    {
        float time = 0f;

        while (sallosMaterial.GetFloat("_Dissolve_Effect") < 1)
        {
            sallosMaterial.SetFloat("_Dissolve_Effect", time / seconds);
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(seconds);
    }

    /// <summary>
    /// Changes the LookLeft parameter for Sallos to linearly go to 0 or 1 over
    /// the specified time.
    /// </summary>
    /// <param name="desiredValue">Desired LookLeft value.</param>
    /// <param name="desiredDuration">Time the change should should take.</param>
    /// <returns></returns>
    private IEnumerator ChangeLookLeft(
		Animator animator, float desiredValue, float desiredDuration)
    {
        float timeElapsed = 0f;
        //Determine which formula to use to change lookleft value
        bool increase = animator.GetFloat("LookLeft") < desiredValue;

        if (increase)
        {
            while (animator.GetFloat("LookLeft") < desiredValue)
            {
                animator.SetFloat("LookLeft", timeElapsed / desiredDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (animator.GetFloat("LookLeft") > desiredValue)
            {
                animator.SetFloat("LookLeft", 1 - (timeElapsed / desiredDuration));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
