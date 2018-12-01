using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//placeholder
public class Workplace : MonoBehaviour
{
    public ActionData[] waypoints;
    public List<Worker> workers;

    private void Start()
    {
        Invoke("Test", 1.0f);
    }

    public ActionData GetAction()
    {
        return waypoints[Random.Range(0, waypoints.Length)];
    }

    void Test()
    {
        foreach (Worker worker in WorkerManager.workers)
            worker.StartWorking(this);
    }

    //periodically update production, handle null, dead workers
}