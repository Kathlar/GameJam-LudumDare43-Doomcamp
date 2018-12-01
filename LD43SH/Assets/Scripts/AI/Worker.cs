﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    float food;
    float health = 1.0f;
    public Workplace workplace;
    
    NavMeshAgent agent;
    private CharacterAnimations animations;
    
    public bool canWork = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<CharacterAnimations>();
    }

    private void Start()
    {
        WorkerManager.WorkerNew(this);
        food = Random.Range(0.9f, 1.1f);
        health = Random.Range(0.8f, 1.2f);

        InvokeRepeating("UpdateState", Random.Range(0.0f, 1.0f), 1.0f);
    }

    void UpdateState()
    {
        // food, death
        food = Mathf.MoveTowards(food, 0, 0.001f); // 1000 seconds for a dude to die
        if (food == 0)
        {
            Die();
        }

        // slacking (when morale is 0, there is 1% chance of slack every second, morale 100 - no chance)
        float slackChance = (1 - (CampResources.instance.morale.value / 100)) / 100;
        if (Random.Range(0.0f, 1.0f) < slackChance && canWork)
        {
            if (Random.Range(0.0f, 1.0f) < 0.1f)
                StartRunAway();
            else
                StartSlack();

        }

        if (health == 0)
        {
            health = 1.0f; // przedsmiertna pasja

            Invoke("DieWork", Random.Range(0.0f, 20.0f));
        }
    }

    public void WorkedWithNoTools()
    {
        health = Mathf.Clamp(health - 0.1f, 0, 10);
    }

    public void DieCold()
    {
        Die();
    }

    public void DieHunger()
    {
        Die();
    }

    public void DieWork()
    {
        Die();
    }

    public void DieShot()
    {
        Die();
    }

    void Die()
    {
        animations.Die();
        WorkerManager.WorkerDied(this);
        Destroy(gameObject);
    }

    #region commands
    public void StartWorking(Workplace workplace)
    {
        canWork = true;
        this.workplace = workplace;
        StopAllCoroutines();
        StartCoroutine(Work(workplace));
    }

    public void StartIdle()
    {
        canWork = true;
        if (workplace)
            workplace.workers.Remove(this);
        workplace = null;
        StopAllCoroutines();
        StartCoroutine(IdleWalk());
    }

    public void StartRunAway()
    {
        canWork = false;
        if (workplace)
            workplace.workers.Remove(this);
        workplace = null;
        StopAllCoroutines();
        StartCoroutine(RunAway());
    }

    public void StartSlack()
    {
        canWork = false;
        if (workplace)
            workplace.workers.Remove(this);
        workplace = null;
        StopAllCoroutines();
        StartCoroutine(Slack());
    }

    #endregion
    #region work
    
    IEnumerator Work(Workplace workplace)
    {
        yield return null;
        ActionData data;
        while(true)
        {
            data = workplace.GetAction();
            agent.SetDestination(data.place.position);

            // wait until agents updates his path
            yield return new WaitForSeconds(0.5f);
            while (agent.remainingDistance > 1.5f)
                yield return new WaitForEndOfFrame();
            
            //animator.SetTrigger(data.animName);

            yield return new WaitForSeconds(data.time);
        }
    }

    #endregion

    #region others
    IEnumerator IdleWalk()
    {
        yield return null;
    }

    IEnumerator RunAway()
    {
        animations.Escape();
        yield return null;
        GetComponent<Renderer>().material.color = Color.cyan;
        Vector3 pt = EscapingManager.GetEscapePoint(this);
        agent.SetDestination(pt);
        agent.speed = 1;

        while (transform.position.magnitude < 30)
        {
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(Random.Range(5, 20));

        DieCold();
    }

    IEnumerator Slack()
    {
        yield return null;
        GetComponent<Renderer>().material.color = Color.black;
        for (int i = 0; i < 6; ++i)
        {
            Vector3 gotoPos = transform.position + Random.insideUnitSphere * 5;
            gotoPos.y = transform.position.y;

            agent.SetDestination(gotoPos);
            yield return new WaitForSeconds(5);
        }
        GetComponent<Renderer>().material.color = Color.blue;
        StartIdle();
    }
    #endregion
}