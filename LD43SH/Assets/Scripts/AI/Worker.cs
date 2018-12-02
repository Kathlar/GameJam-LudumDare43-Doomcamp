using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    float food;
    float health = 1.0f;
    public Workplace workplace;
    public Freezer freezer;
    
    NavMeshAgent agent;
    private CharacterAnimations animations;
    
    public bool canWork = true;

    public WorkerState curState = WorkerState.idle;
    public ActionData data;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<CharacterAnimations>();
    }

    private void Start()
    {
        WorkerManager.WorkerNew(this);
        food = Random.Range(0.75f, 1.25f);
        health = Random.Range(0.8f, 1.2f);

        InvokeRepeating("UpdateState", Random.Range(0.0f, 1.0f), 1.0f);
    }

    void UpdateState()
    {
        // food, death (12 days to die with no food)

        float reduction = (1.0f / 120);

        if (CampResources.instance.food.value > 0)
        {
            float taken = (1.0f / 120) * CampResources.instance.foodRationsRate;
            reduction -= taken;

            CampResources.instance.food.value = Mathf.Clamp(
                CampResources.instance.food.value - taken, 0, 1000);
        }

        food = Mathf.MoveTowards(food, 0, reduction);
        if (food == 0)
        {
            Die();
        }

        // slacking (when morale is 0, there is 1% chance of slack every second, morale 100 - no chance)
        float slackChance = (1 - (CampResources.instance.morale.value / 100)) / 100;
        if (Random.Range(0.0f, 1.0f) < slackChance && canWork)
        {
            if (Random.Range(0.0f, 1.0f) < 0.25f)
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
        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value - 0.01f, 0, 100);
        health = Mathf.Clamp(health - 0.1f, 0, 10);
    }

    public void DieCold()
    {
        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value - 2, 0, 100);
        DieFreeze();
    }

    public void DieHunger()
    {
        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value - 1, 0, 100);
        DieFreeze();
    }

    public void DieWork()
    {
        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value - 1, 0, 100);
        DieFreeze();
    }

    public void DieShot()
    {
        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value + 10, 0, 100);
        Die();
    }

    public void DieFreeze()
    {
        Die();
        /*
        animations.Unparent();
        WorkerManager.WorkerDied(this);
        freezer.Enable(0.0F);
        Destroy(gameObject);
        */
    }

    public void DieSilent()
    {
        WorkerManager.WorkerDied(this);
        Destroy(gameObject);
    }

    void Die()
    {
        animations.Die();
        WorkerManager.WorkerDied(this);
        freezer.Enable(2.0F);
        Destroy(gameObject);
    }

    #region commands
    public void StartWorking(Workplace workplace)
    {
        curState = WorkerState.working;
        canWork = true;
        this.workplace = workplace;
        StopAllCoroutines();
        StartCoroutine(Work(workplace));
    }

    public void StartIdle()
    {
        curState = WorkerState.idle;
        canWork = true;
        if (workplace)
            workplace.workers.Remove(this);
        workplace = null;
        StopAllCoroutines();
        StartCoroutine(IdleWalk());
    }

    public void StartRunAway()
    {
        curState = WorkerState.running;
        canWork = false;
        if (workplace)
            workplace.workers.Remove(this);
        workplace = null;
        StopAllCoroutines();
        StartCoroutine(RunAway());
    }

    public void StartSlack()
    {
        curState = WorkerState.slacking;
        canWork = false;
        if (workplace)
            workplace.workers.Remove(this);
        workplace = null;
        StopAllCoroutines();
        StartCoroutine(Slack());
    }

    public void StartGettingBeaten()
    {
        curState = WorkerState.beaten;
        StopAllCoroutines();
        StartCoroutine(GetBeaten());
    }

    #endregion
    #region work
    
    IEnumerator Work(Workplace workplace)
    {
        yield return null;
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

        while (transform.position.magnitude < 60)
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
    
    IEnumerator GetBeaten() // get rekt
    {
        yield return null;

        agent.SetDestination(transform.position);
        // trigger some animation

        yield return new WaitForSeconds(5.0f);
        health = Mathf.Clamp(health - 0.3f, 0, 2); // beating hurts
        canWork = true;
    }
    #endregion
}

public enum WorkerState
{
    working,
    idle,
    slacking,
    running,
    beaten,
}