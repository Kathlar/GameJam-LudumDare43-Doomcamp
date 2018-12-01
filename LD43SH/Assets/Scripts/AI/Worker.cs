using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    float food = 1.0f;
    public GameObject deadBody;
    public Workplace workplace;
    
    NavMeshAgent agent;
    Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        WorkerManager.WorkerNew(this);
    }

    private void Start()
    {
        //StartCoroutine(IdleWalk());
        InvokeRepeating("UpdateState", Random.Range(0.0f, 1.0f), 1.0f);
    }

    void UpdateState()
    {
        // food, morale, death etc
        food = Mathf.MoveTowards(food, 0, 0.001f); // 1000 seconds for a dude to die
        if (food == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deadBody, transform.position, transform.rotation);
        WorkerManager.WorkerDied(this);
        Destroy(gameObject);
    }

    #region commands
    public void StartWorking(Workplace workplace)
    {
        StopAllCoroutines();
        StartCoroutine(Work(workplace));
    }

    public void StartIdle()
    {
        StopAllCoroutines();
        StartCoroutine(IdleWalk());
    }

    public void StartRunAway()
    {
        StopAllCoroutines();
        StartCoroutine(RunAway());
    }

    public void StartSlack()
    {
        StopAllCoroutines();
        StartCoroutine(Slack());
    }

    public void StartBunt()
    {
        StopAllCoroutines();
        StartCoroutine(Bunt());
    }

    #endregion
    #region work
    
    IEnumerator Work(Workplace workplace)
    {
        this.workplace = workplace;
        ActionData data;
        while(true)
        {
            data = workplace.GetAction();
            agent.SetDestination(data.place.position);
            
            yield return new WaitForEndOfFrame(); // wait until agents updates his path
            
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
        StopAllCoroutines();
        yield return null;
    }

    IEnumerator RunAway()
    {
        yield return null;
    }

    IEnumerator Slack()
    {
        yield return null;
    }

    IEnumerator Bunt()
    {
        yield return null;
    }
    #endregion
}