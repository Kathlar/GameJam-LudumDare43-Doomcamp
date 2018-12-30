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
        if (Vector3.Distance(transform.position, lastPosition) > .2f * Time.deltaTime)
            animator.SetBool("Move", true);
        else
            animator.SetBool("Move", false);

        lastPosition = transform.position;
    }

    public void TriggerAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);
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

    public void BeatUp()
    {
        animator.SetBool("Beat", true);
        Invoke("StopBeatUp", 1f);
    }

    protected void StopBeatUp()
    {
        animator.SetBool("Beat", false);
    }

    public void Shoot()
    {
        animator.SetBool("Shoot", true);
        Invoke("StopShoot", 3f);
    }

    protected void StopShoot()
    {
        animator.SetBool("Shoot", false);
    }
}
