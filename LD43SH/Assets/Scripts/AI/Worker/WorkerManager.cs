using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static List<Worker> workers = new List<Worker>();
    public Worker workerPrefab;

    public static WorkerManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("UpdateState", 0, 1.0f);
    }

    void UpdateState()
    {
        for (int i = workers.Count - 1; i >= 0; --i)
            if (workers[i] == null)
                workers.RemoveAt(i);
    }

    public static void WorkerDied(Worker worker)
    {
        workers.Remove(worker);
    }
    
    public static Worker SpawnWorker(Vector3 position, Quaternion rotation)
    {
        Worker worker = Instantiate(instance.workerPrefab, position, rotation);

        workers.Add(worker);        
        worker.Init(instance);
        worker.transform.parent = instance.transform;

        return worker;
    }
}
