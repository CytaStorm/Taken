using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SceneThreeController : CutsceneController
{
    // Actors
    public GameObject Sallos;
    public GameObject Eulyss;
    public GameObject Akif;
    public GameObject Goon;
    
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

        //Add flags
        _flagNames.Add("walkLeft");
        _flagNames.Add("enterAkif");
        _flagNames.Add("enterGoon");
        _flagNames.Add("walkCloser");
        _flagNames.Add("stab");
        _flagNames.Add("disappear");
        _flagNames.Add("reach");

        //Create eventFlags list based on string list flagNames
        foreach(string name in _flagNames)
        {
            foreach (NewDialogueFlag flag in DialogueFlags) 
            {
                if (!flag.Names.Contains(name)) continue;

                _eventFlags.Add(flag);
            }
        }

        // Assign all event methods to OnClick() events for all buttons
        _eventFlags[0].onValueChange += delegate { WalkLeft(); };
        _eventFlags[1].onValueChange += delegate { EnterAkif(); };
        _eventFlags[2].onValueChange += delegate { EnterGoon(); };
        _eventFlags[3].onValueChange += delegate { WalkCloser(); };
        _eventFlags[4].onValueChange += delegate { Stab(); };
        _eventFlags[5].onValueChange += delegate { Disappear(); };
        _eventFlags[6].onValueChange += delegate { Reach(); };

        // Get component data for all actors
        _sallosAgent = Sallos.GetComponent<NavMeshAgent>();
        _sallosAnimator = Sallos.GetComponent<Animator>();

        _eulyssAgent = Eulyss.GetComponent<NavMeshAgent>();
        _eulyssAnimator = Eulyss.GetComponent<Animator>();

        _akifAgent = Akif.GetComponent<NavMeshAgent>();
        _akifAnimator = Akif.GetComponent<Animator>();

        _goonAgent = Goon.GetComponent<NavMeshAgent>();
        _goonAnimator = Goon.GetComponent<Animator>();

        // Set initial positions of all actors
        Sallos.transform.position = new Vector3(-0.41f, 0, 0.2f);
        Eulyss.transform.position = new Vector3(-0.93f, 0, -0.49f);
        Akif.transform.position = new Vector3(-4.92f, 0, 16.85f);
        Goon.transform.position = new Vector3(-12.08f, 0, 4.37f);
    }

	// Update is called once per frame
	new void Update()
    {
        base.Update(); // Update timer each frame
    }

    /// <summary>
    /// Finds the event flag with the specified name. (ONLY USE ON EVENT FLAGS B/C THEY HAVE ONE NAME ONLY)
    /// </summary>
    /// <param name="eventName">Flag name to look for.</param>
    /// <returns>Flag with that name.</returns>
    private NewDialogueFlag FindEventWithName(string eventName)
    {
        return _eventFlags.FirstOrDefault(flag => flag.Names[0] == eventName);
    }

    private void WalkLeft()
    {
        // Move sallos and eulyss along forest trail
        _sallosAgent.SetDestination(new Vector3(-2.11f, 0f, 7.35f));
        _eulyssAgent.SetDestination(new Vector3(-2.27f, 0f, 5.86f));
        StartCoroutine(UIManager.UI.PauseAllButtons(3f));
    }

    private void EnterAkif()
    {
        // Move akif towards sallos and eulyss
        _akifAgent.SetDestination(new Vector3(-2.990002f, 0f, 10.45f));
        StartCoroutine(UIManager.UI.PauseAllButtons(2.7f));
    }

    private void EnterGoon()
    {
        // Move the goon to corner sallos and eulyss
        _goonAgent.SetDestination(new Vector3(-4.56f, 0f, 7.1f));
        _sallosAnimator.SetTrigger("LookLeftTrigger");
        _eulyssAnimator.SetTrigger("LookLeftTrigger");
        StartCoroutine(ChangeLookLeft(_sallosAnimator, 1, 0.5f));
        StartCoroutine(ChangeLookLeft(_eulyssAnimator, 1, 0.66f));
        StartCoroutine(UIManager.UI.PauseAllButtons(2.9f));
    }

    private void WalkCloser()
    {
        // Move both akif and the goon closer to sallos and eulyss
        _akifAgent.SetDestination(new Vector3(-2.798f, 0f, 8.952f));
        _goonAgent.SetDestination(new Vector3(-3.66f, 0f, 7.04f));
        StartCoroutine(ChangeLookLeft(_sallosAnimator, 0, 0.5f));
        StartCoroutine(ChangeLookLeft(_eulyssAnimator, 0, 0.45f));
        StartCoroutine(UIManager.UI.PauseAllButtons(1.3f));
    }

    private void Reach()
    {
		_eulyssAnimator.SetTrigger("ReachOut");
    }

    private void Stab()
    {
        _akifAnimator.SetTrigger("Stab");
        _sallosAnimator.SetTrigger("StepBack");
        StartCoroutine(UIManager.UI.PauseAllButtons(0.7f));
    }

    private void Disappear()
    {
        StartCoroutine(FadeAway(5f));
        StartCoroutine(UIManager.UI.PauseAllButtons(5f));
    }

    private IEnumerator FadeAway(float seconds)
    {
        float time = 0f;

        while (_sallosMaterial.GetFloat("_Dissolve_Effect") < 1)
        {
            _sallosMaterial.SetFloat("_Dissolve_Effect", time / seconds);
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
