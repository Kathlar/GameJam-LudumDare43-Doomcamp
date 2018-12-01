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
    public int noTools = 0;

    protected Resource resource;
    
    private void Start()
    {
        resource = CampResources.instance.GetResource(resourceType);
        DayTimeManager.instance.OnDayEnd += OnDayEnd;

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
        float effitiency = 0;

        effitiency += gain * workers.Count / 2;

        if (resourceType == ResourceType.Stone)
        {
            int tools = Mathf.Clamp(
                workers.Count,
                0,
                (int)CampResources.instance.hammers.value
                );

            effitiency += gain * tools / 2;
            noTools = workers.Count - tools;
        }
        else if (resourceType == ResourceType.Wood)
        {
            int tools = Mathf.Clamp(
                workers.Count,
                0,
                (int)CampResources.instance.axes.value);

            effitiency += gain * tools / 2;
            noTools = workers.Count - tools;
        }
        else if (resourceType == ResourceType.Metal)
        {
            int tools = Mathf.Clamp(
                workers.Count,
                0,
                (int)CampResources.instance.picks.value);

            effitiency += gain * tools / 2;
            noTools = workers.Count - tools;
        }
        else // tool workshop: always full production
        {
            effitiency += gain * workers.Count / 2;
        }

        resource.value += gain * workers.Count;
    }

    public void OnDayEnd()
    {
        for (int i = 0; i < noTools && i < workers.Count; ++i)
            workers[i].WorkedWithNoTools();

        int toolsDestroyed = (workers.Count - noTools) / 10;

        if (resourceType == ResourceType.Stone)
            CampResources.instance.hammers.value = Mathf.Clamp(
                CampResources.instance.hammers.value - toolsDestroyed,
                0,
                1000);
        else if (resourceType == ResourceType.Wood)
            CampResources.instance.axes.value = Mathf.Clamp(
                CampResources.instance.axes.value - toolsDestroyed,
                0,
                1000);
        else if (resourceType == ResourceType.Metal)
            CampResources.instance.picks.value = Mathf.Clamp(
                CampResources.instance.picks.value - toolsDestroyed,
                0,
                1000);
    }

    public ActionData GetAction()
    {
        return actions[Random.Range(0, actions.Length - 1)];
    }
}
