using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : MonoBehaviour
{
    [SerializeField]
    public ActionData[] actions;
    public ResourceType resourceType;
    [Range(0, 100)]
    public int desiredWorkersCount;
    public List<Worker> workers;
    [Range(0.0F, 100.0F)]
    public float gain = 1.0F;

    protected Resource resource;

    private void Start()
    {
        resource = CampResources.instance.GetResource(resourceType);

        InvokeRepeating("UpdateTick", Random.value, 1.0F);
    }

    private void Update()
    {
        RemoveDeadWorkers();
    }

    private void UpdateTick()
    {
        UpdateWorkersCount();
        UpdateResource();
    }

    private void RemoveDeadWorkers()
    {
        for (int i = workers.Count - 1; i >= 0; i--)
        {
            if (!WorkerManager.workers.Contains(workers[i]))
            {
                workers.RemoveAt(i);
            }
        }
    }

    private void UpdateWorkersCount()
    {
        if (workers.Count < desiredWorkersCount)
        {
            foreach (Worker worker in WorkerManager.workers)
            {
                if (workers.Count >= desiredWorkersCount)
                {
                    break;
                }

                if (worker.workplace == null && worker.canWork)
                {
                    workers.Add(worker);
                    worker.StartWorking(this);
                }
            }
        }
        else if (workers.Count > desiredWorkersCount)
        {
            for (int i = workers.Count - 1; i >= desiredWorkersCount; i--)
            {
                Worker worker = workers[i];

                worker.StartIdle();
                workers.RemoveAt(i);
            }
        }
    }

    private void UpdateResource()
    {
        resource.value += gain * workers.Count;
    }

    public ActionData GetAction()
    {
        return actions[Random.Range(0, actions.Length - 1)];
    }
}
