using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    float food = 1.0f;
    Workplace workplace;
    GameObject deadBody;

    private void Start()
    {
        StartCoroutine(IdleWalk());

        InvokeRepeating("UpdateState", Random.Range(0.0f, 1.0f), 1.0f);
    }

    void UpdateState()
    {
        // food, morale, death etc
        food = Mathf.MoveTowards(food, 0, 0.001f);
        if (food == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deadBody, transform.position, transform.rotation);
        // worker manager handle dead dude
        Destroy(gameObject);
    }

    IEnumerator Work(Workplace workplace)
    {
        yield return null;
    }

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
}

internal class Workplace
{
    public ActionData[] waypoints;
    public List<Worker> workers;

    //periodically update production, handle null, dead workers
}

[SerializeField]
struct ActionData
{
    string animName;
    float time;
    Transform place;
}