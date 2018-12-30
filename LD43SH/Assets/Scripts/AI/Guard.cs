using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public static List<Guard> guards = new List<Guard>();
    public Vector3 guardSpot;
    NavMeshAgent agent;
    private CharacterAnimations animations;

    bool isPursuingASlacker;
    float cd = 10.0f;
    float cdCur = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<CharacterAnimations>();
    }

    private void Start()
    {
        Camera.main.transform.parent.parent.GetComponent<CameraController>()
            .StartGuardLerp(transform.position);

        if (guardSpot == Vector3.zero)
            guardSpot = transform.position;

        InvokeRepeating("WanderAroundGuardSpot", 0.0f, 10.0f);
        InvokeRepeating("UpdateFoodConsumption", Random.Range(0.0f, 1.0f), 1.0f);
    }

    void OnEnable()
    {
        if (!guards.Contains(this)) guards.Add(this);
    }

    void OnDisable()
    {
        if (guards.Contains(this)) guards.Remove(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (cdCur > Time.time) return;
        if (isPursuingASlacker) return;        
        Worker w = other.GetComponent<Worker>();
        if (!w || w.currentActivity.punishmentCriteria == PunishmentCriteria.nothing) return;

        cdCur = Time.time + cd;
        isPursuingASlacker = true;

        if (w.currentActivity.punishmentCriteria == PunishmentCriteria.execution)
            StartCoroutine(ShootEscapingDude(w));
        else if (w.currentActivity.punishmentCriteria == PunishmentCriteria.beating)
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

        Vector3 pt = guardSpot + Random.insideUnitSphere * 3;
        pt.y = transform.position.y;

        agent.SetDestination(pt);
    }

    IEnumerator StartPacifyingADude(Worker worker)
    {
        yield return null;

        while(true) // chase worker
        {
            if (worker == null)
            {
                isPursuingASlacker = false;
                yield break;
            }

            agent.SetDestination(worker.transform.position);
            yield return new WaitForSeconds(0.5f);
            if (Vector3.Distance(transform.position, worker.transform.position) < 1.5f)
                break;
        }

        animations.BeatUp();
        worker.BeginActivityGetBeaten();
        agent.SetDestination(transform.position);
        // trigger some animation

        yield return new WaitForSeconds(5.5f);
        CampResources.instance.morale.value += 2.0f;
        isPursuingASlacker = false;
    }

    IEnumerator ShootEscapingDude(Worker worker)
    {
        while (true) // chase worker
        {
            if (worker == null)
            {
                isPursuingASlacker = false;
                yield break;
            }

            agent.SetDestination(worker.transform.position);
            yield return new WaitForSeconds(0.5f);
            if (Vector3.Distance(transform.position, worker.transform.position) < 5.0f)
                break;
        }

        yield return new WaitForSeconds(2.0f);


        if (worker == null)
        {
            isPursuingASlacker = false;
            yield break;
        }
        animations.Shoot();
        worker.DieShot();
        GetComponent<AudioSource>().Play();

        CampResources.instance.morale.value += 10.0f;
        isPursuingASlacker = false;
    }
}
