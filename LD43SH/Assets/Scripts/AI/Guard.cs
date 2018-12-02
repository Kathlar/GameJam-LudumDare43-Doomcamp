using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public static List<Guard> guards = new List<Guard>();
    public Vector3 guardSpot;
    NavMeshAgent agent;

    bool isPursuingASlacker;
    float cd = 10.0f;
    float cdCur = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        if (!guards.Contains(this)) guards.Add(this);
    }

    void OnDisable()
    {
        if (guards.Contains(this)) guards.Remove(this);
    }

    private void Start()
    {
        if (guardSpot == Vector3.zero)
            guardSpot = transform.position;

        InvokeRepeating("WanderAroundGuardSpot", 0.0f, 10.0f);
        InvokeRepeating("UpdateFoodConsumption", Random.Range(0.0f, 1.0f), 1.0f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (cdCur > Time.time) return;
        if (isPursuingASlacker) return;        
        Worker w = other.GetComponent<Worker>();
        if (!w || w.canWork) return;

        cdCur = Time.time + cd;
        isPursuingASlacker = true;

        if (w.curState == WorkerState.running)
            StartCoroutine(ShootEscapingDude(w));
        else
            StartCoroutine(StartPacifyingADude(w));
    }


    public void GoToSpot(Vector3 spot)
    {
        guardSpot = spot;
        agent.SetDestination(guardSpot);
    }

    void UpdateFoodConsumption()
    {
        float taken = (1.0f / 120) * 3;
        CampResources.instance.food.value = Mathf.Clamp(
            CampResources.instance.food.value - taken, 0, 1000);
    }

    void WanderAroundGuardSpot()
    {
        if (isPursuingASlacker) return;

        Vector3 pt = transform.position + Random.insideUnitSphere * 3;
        pt.y = transform.position.y;

        agent.SetDestination(pt);
    }

    IEnumerator StartPacifyingADude(Worker worker)
    {
        yield return null;

        while(true) // chase worker
        {
            agent.SetDestination(worker.transform.position);
            yield return new WaitForSeconds(0.5f);
            if (Vector3.Distance(transform.position, worker.transform.position) < 1.5f)
                break;
        }

        worker.StartGettingBeaten();
        agent.SetDestination(transform.position);
        // trigger some animation

        yield return new WaitForSeconds(5.5f);
        CampResources.instance.morale.value += 1.0f;
        isPursuingASlacker = false;
    }

    IEnumerator ShootEscapingDude(Worker worker)
    {
        while (true) // chase worker
        {
            agent.SetDestination(worker.transform.position);
            yield return new WaitForSeconds(0.5f);
            if (Vector3.Distance(transform.position, worker.transform.position) < 5.0f)
                break;
        }
        //todo visual fluff

        yield return new WaitForSeconds(2.0f);

        worker.DieShot();

        CampResources.instance.morale.value += 10.0f;
        isPursuingASlacker = false;
    }
}
