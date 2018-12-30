using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CampResources : MonoBehaviour
{
    public List<Resource> Resources = new List<Resource>();
    public Resource food = new Resource(ResourceType.Food, 0, 0);
    public Resource morale = new Resource(ResourceType.Terror, 0, 0);
    public Resource metal = new Resource(ResourceType.Steel, 0, 0);
    public Resource stone = new Resource(ResourceType.Boulders, 0, 0);
    public Resource wood = new Resource(ResourceType.Lumber, 0, 0);
    public Resource hammers = new Resource(ResourceType.Hammer, 0, 0);
    public Resource axes = new Resource(ResourceType.Axe, 0, 0);
    public Resource picks = new Resource(ResourceType.Pick, 0, 0);

    public int numberOfPeople;
    public int numberOfGuards;
    public Text totalPeopleText, idlePeopleText, numberOfGuardsText;

    public static CampResources instance;
    public float foodRationsRate = 0.5f;
    public static float lastMetalTaken, lastWoodTaken, lastStoneTaken;

    public Slider foodRationsSlider;

    private void Awake()
    {
        instance = this;
        Resources.Add(food);
        Resources.Add(morale);
        Resources.Add(metal);
        Resources.Add(stone);
        Resources.Add(wood);
        Resources.Add(hammers);
        Resources.Add(axes);
        Resources.Add(picks);
    }
    
    void Update()
    {
        SetFoodRations();
        foreach (Resource resource in Resources)
        {
            if (resource.resourceText == null)
                continue;

            resource.resourceText.text = resource.resourceType.ToString() + "\n" + Mathf.Floor(resource.value).ToString();
        }

        CountPeople();

        int workerCount = numberOfPeople;
        int idleWorkerCount = 0;

        foreach (Worker w in WorkerManager.workers)
            if (!w.currentWorkplace)
                ++idleWorkerCount;

        totalPeopleText.text = "Total: " + workerCount;
        idlePeopleText.text = "Idle: " + idleWorkerCount;
        numberOfGuardsText.text = "Guards\n" + numberOfGuards.ToString();
    }

    public void SetFoodRations()
    {
        foodRationsRate = foodRationsSlider.value;
    }

    public Resource GetResource(ResourceType resourceType)
    {
        return Resources.Single(resource => resource.resourceType == resourceType);
    }

    public void DailyUseOfResources()
    {
        foreach (var resource in Resources)
        {
            resource.DailyUsage();
        }
    }

    void CountPeople()
    {
        numberOfPeople = WorkerManager.workers.Count;
        numberOfGuards = Guard.guards.Count;
    }

    public bool TakeEverything(TrainScenario scenario, out string comment)
    {
        comment = "";

        if (metal.value < scenario.minimalMetalValue ||
            wood.value < scenario.minimalWoodValue ||
            stone.value < scenario.minimalStoneValue)
        {
            if (metal.value < scenario.minimalMetalValue)
                comment = "oh, comrade, it looks like you didn't bring us enough steel. Our governor won't be happy about that...";

            else if (wood.value < scenario.minimalWoodValue)
                comment = "oh, comrade, it looks like you didn't bring us enough wood. Our governor won't be happy about that...";

            else if (stone.value < scenario.minimalStoneValue)
                comment = "oh, comrade, it looks like you didn't bring us enough stone. Our governor won't be happy about that...";

            metal.value = 0;
            wood.value = 0;
            stone.value = 0;
            //loose
            return false;
        }
        else
        {
            lastMetalTaken = metal.value;
            lastWoodTaken = wood.value;
            lastStoneTaken = stone.value;

            metal.value = 0;
            wood.value = 0;
            stone.value = 0;

            return true;
        }
    }
}

[Serializable]
public class Resource
{
    public ResourceType resourceType;
    public float value;
    public float dailyUsage = 0;
    public Text resourceText;

    public Resource(ResourceType rType, float startValue, float dailyUsage = 0)
    {
        resourceType = rType;
        value = startValue;
        this.dailyUsage = dailyUsage;
    }

    public void DailyUsage()
    {
        value = Mathf.Clamp(value - dailyUsage, 0, Mathf.Infinity);
    }
}

public enum ResourceType
{
    Food, Terror, Steel, Lumber, Boulders,
    Hammer, Axe, Pick
}