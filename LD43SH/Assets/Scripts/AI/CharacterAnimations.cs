using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private Worker worker;
    private Vector3 lastPosition;
    public Animator animator;

    void Start()
    {
        lastPosition = transform.position;
        worker = GetComponent<Worker>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, lastPosition) > .2f * Time.deltaTime)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
        lastPosition = transform.position;

        if (worker != null)
        {
            if (worker.workplace != null && worker.data != null)
            {
                if(Vector3.Distance(worker.transform.position, worker.data.place.position) < 1.5f) Work();
                else StopWork();
            }
        }
    }

    public void Escape()
    {
        animator.SetBool("Escape", true);
    }

    public void Work()
    {
        animator.SetBool("Work", true);
    }

    public void StopWork()
    {
        animator.SetBool("Work", false);
    }

    public void Die()
    {
        Unparent();
        animator.SetBool("Dead", true);
    }

    public void Unparent()
    {
        animator.transform.SetParent(null);
    }
}
