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
    public int maxWorkersCount = 100;
    [HideInInspector]
    public int workersWithoutTools = 0;

    protected Resource resource;
    
    private void Start()
    {
        resource = CampResources.instance.GetResource(resourceType);
        DayTimeManager.instance.OnDayEnd += OnDayEnd;

        InvokeRepeating("UpdateWorkersCount", Random.value, 0.5F);
        InvokeRepeating("UpdateResource", Random.value, 1.0F);
    }

    private void Update()
    {
        RemoveDeadWorkers();
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
                if (worker.workplace == null && worker.canWork)
                {
                    workers.Add(worker);
                    worker.StartWorking(this);

                    break;
                }
            }
        }
        else if (workers.Count > desiredWorkersCount)
        {
            Worker worker = workers[workers.Count - 1];

            workers.Remove(worker);
            worker.StartIdle();
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
            workersWithoutTools = workers.Count - tools;
        }
        else if (resourceType == ResourceType.Wood)
        {
            int tools = Mathf.Clamp(
                workers.Count,
                0,
                (int)CampResources.instance.axes.value);

            effitiency += gain * tools / 2;
            workersWithoutTools = workers.Count - tools;
        }
        else if (resourceType == ResourceType.Metal)
        {
            int tools = Mathf.Clamp(
                workers.Count,
                0,
                (int)CampResources.instance.picks.value);

            effitiency += gain * tools / 2;
            workersWithoutTools = workers.Count - tools;
        }
        else // tool workshop: always full production
        {
            effitiency += gain * workers.Count / 2;
        }

        resource.value += gain * workers.Count;
    }

    public void OnDayEnd()
    {
        for (int i = 0; i < workersWithoutTools && i < workers.Count; ++i)
            workers[i].WorkedWithNoTools();

        int toolsDestroyed = (workers.Count - workersWithoutTools) / 10;

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
