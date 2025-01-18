using UnityEngine;

public class AlwaysWalk : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetFloat("VelocityPercent", 1);
    }
}
