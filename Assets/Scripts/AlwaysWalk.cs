using UnityEngine;

public class AlwaysWalk : MonoBehaviour
{
    public float speed;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = speed;
        animator.SetBool("Walking", true);
    }
}
