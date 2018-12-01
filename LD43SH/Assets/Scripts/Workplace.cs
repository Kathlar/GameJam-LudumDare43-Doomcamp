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
        foreach (Worker worker in WorkerManager.workers)
            worker.StartWorking(this);
    }

    public ActionData GetAction()
    {
        return waypoints[Random.Range(0, waypoints.Length)];
    }

    //periodically update production, handle null, dead workers
}

[System.Serializable]
public class ActionData
{
    public string animName;
    public float time;
    public Transform place;
}