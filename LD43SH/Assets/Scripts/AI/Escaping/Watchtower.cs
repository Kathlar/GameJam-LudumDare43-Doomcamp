using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watchtower : MonoBehaviour
{
    float cd = 10.0f;
    float cdCur = 0;

    private void OnTriggerStay(Collider other)
    {
        if (cdCur > Time.time) return;

        Worker w = other.GetComponent<Worker>();
        if (!w) return;

        cdCur = Time.time + cd;
        StartCoroutine(ShootEscapingDude(w));
    }

    IEnumerator ShootEscapingDude(Worker worker)
    {
        //todo visual fluff

        yield return new WaitForSeconds(2.0f);

        worker.DieShot();

        CampResources.instance.morale.value += 10.0f;
    }
}
