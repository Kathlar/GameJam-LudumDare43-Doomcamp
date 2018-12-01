using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private Vector3 lastPosition;
    public Animator animator;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (transform.position != lastPosition)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
        lastPosition = transform.position;
    }

    public void Escape()
    {
        animator.SetBool("Escape", true);
    }

    public void Die()
    {
        animator.transform.SetParent(null);
        animator.SetBool("Dead", true);
    }
}
