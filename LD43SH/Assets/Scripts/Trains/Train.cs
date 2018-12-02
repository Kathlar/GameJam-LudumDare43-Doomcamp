using System.Collections;
using System.Collections.Generic;
using Battlehub.SplineEditor;
using UnityEngine;

public class Train : MonoBehaviour
{
    [HideInInspector] public TrainManager manager;
    protected SplineFollow spline;
    private TrainStopper stopper;
    private TrainWorkerSpawner workerSpawner;
    private float distance, startSplineSpeed;
    float lastTrainTime = 0;

    void Awake()
    {
        spline = GetComponent<SplineFollow>();
        stopper = FindObjectOfType<TrainStopper>();
        workerSpawner = gameObject.GetComponent<TrainWorkerSpawner>();
        startSplineSpeed = spline.Speed;
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, stopper.transform.position);
        spline.Speed = startSplineSpeed * Mathf.Clamp(distance / 30, 0, 1);
    }

    public void SpawnTrain()
    {
        spline.IsRunning = true;
        spline.IsLoop = false;
    }

    public void StopTrain()
    {
        if (lastTrainTime + 5 > Time.time) //quick fix a weird bug
            return;
        lastTrainTime = Time.time;

        spline.IsRunning = false;
        workerSpawner.SpawnWorkers(manager.TrainScenarios[0].numberOfPeople);
        manager.TrainArrive();
        Invoke("StartTrain", 10);
    }

    public void StartTrain()
    {
        spline.IsRunning = true;
    }

    public void DespawnTrain()
    {
        spline.IsLoop = true;
        spline.IsRunning = false;
    }
}
