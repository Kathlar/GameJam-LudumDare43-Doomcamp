using System.Collections;
using System.Collections.Generic;
using Battlehub.SplineEditor;
using UnityEngine;

public class Train : MonoBehaviour
{
    [HideInInspector] public TrainManager manager;
    protected SplineFollow spline;

    void Awake()
    {
        spline = GetComponent<SplineFollow>();
    }

    public void SpawnTrain()
    {
        spline.IsRunning = true;
        spline.IsLoop = false;
    }

    public void StopTrain()
    {
        spline.IsRunning = false;
        manager.TrainArrive();
        Invoke("StartTrain", 4);
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
