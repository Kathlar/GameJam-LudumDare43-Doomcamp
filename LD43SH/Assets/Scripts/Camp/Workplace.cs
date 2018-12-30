using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Workplace : MonoBehaviour
{
    public ActionData[] actions;
    public ResourceType resourceType;
    public ToolType toolType;
    [Range(0, 100)]
    public int desiredWorkersCount;
    public List<Worker> workers;
    [Range(0.0F, 100.0F)]
    public float gain = 1.0F;
    public int maxWorkersCount = 100;
    [HideInInspector]
    public int workersWithoutTools = 0;

    public List<DUPA> upgradeRequirements;
    public float gainPerUpgrade = 0.01f;
    public int maxWorkerPerUpgrade = 2;
    public int level = 0;

    protected Resource resource;
    
    private void Start()
    {
        resource = CampResources.instance.GetResource(resourceType);
        actions = GetComponentsInChildren<ActionData>();
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
                if (worker.currentActivity != null &&
                    worker.currentActivity.workState == WorkState.available)
                {
                    workers.Add(worker);
                    worker.SetWorkplace(this);

                    break;
                }
            }
        }
        else if (workers.Count > desiredWorkersCount)
        {
            Worker worker = workers[workers.Count - 1];

            workers.Remove(worker);
            worker.SetWorkplace(null);
        }
    }

    private void UpdateResource()
    {
        float effitiency = 0;

        effitiency += gain * workers.Count / 2;

        if (toolType == ToolType.pick)
        {
            int tools = Mathf.Clamp(
                workers.Count,
                0,
                (int)CampResources.instance.hammers.value
                );

            effitiency += gain * tools / 2;
            workersWithoutTools = workers.Count - tools;
        }
        else if (toolType == ToolType.axe)
        {
            int tools = Mathf.Clamp(
                workers.Count,
                0,
                (int)CampResources.instance.axes.value);

            effitiency += gain * tools / 2;
            workersWithoutTools = workers.Count - tools;
        }
        else if (toolType == ToolType.hammer)
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
        int toolsDestroyed = (workers.Count - workersWithoutTools) / 10;

        if (resourceType == ResourceType.Boulders)
            CampResources.instance.hammers.value = Mathf.Clamp(
                CampResources.instance.hammers.value - toolsDestroyed,
                0,
                1000);
        else if (resourceType == ResourceType.Lumber)
            CampResources.instance.axes.value = Mathf.Clamp(
                CampResources.instance.axes.value - toolsDestroyed,
                0,
                1000);
        else if (resourceType == ResourceType.Steel)
            CampResources.instance.picks.value = Mathf.Clamp(
                CampResources.instance.picks.value - toolsDestroyed,
                0,
                1000);
    }

    public bool CanUpgrade(out string error)
    {
        if (level >= upgradeRequirements.Count)
        {
            error = "Max Level";
            return false;
        }

        DUPA requirements = upgradeRequirements[level];

        foreach(ResRequirement rr in requirements.tab)
        {
            Resource res = CampResources.instance.Resources
                .Where(x => x.resourceType == rr.type)
                .First();

            if (res.value < rr.amount)
            {
                error = "not enough " + rr.type.ToString();
                return false;
            }
        }

        error = "";
        return true;
    }

    public void Upgrade() // run CanUpgrade first
    {
        if (level >= upgradeRequirements.Count)
            return;

        DUPA requirements = upgradeRequirements[level];

        foreach (ResRequirement rr in requirements.tab)
        {
            Resource res = CampResources.instance.Resources
                .Where(x => x.resourceType == rr.type)
                .First();

            res.value -= rr.amount;
        }

        maxWorkersCount += maxWorkerPerUpgrade;
        gain += gainPerUpgrade;
        level += 1;
    }

    public ActivityInfo GetActivity()
    {
        ActivityInfo result = new ActivityInfo();
        result.source = this;
        result.name = "Work at " + name;
        result.toolRequiredID = toolType;
        result.workState = WorkState.busy;
        result.tireRate = 0.5f;

        ActionData actionData = actions[Random.Range(0, actions.Length - 1)];
        result.AddAction("GoTo", actionData.place.position);
        result.AddAction("PlayAnimation", "Work"/*actionData.animName*/);
        result.AddAction("Wait", actionData.time);

        return result;
    }
}

[System.Serializable]
public struct DUPA
{
    public List<ResRequirement> tab;
}

[System.Serializable]
public struct ResRequirement
{
    public ResourceType type;
    public float amount;
}