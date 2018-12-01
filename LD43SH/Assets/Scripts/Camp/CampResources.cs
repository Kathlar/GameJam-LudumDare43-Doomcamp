using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CampResources : MonoBehaviour
{
    private List<Resource> Resources = new List<Resource>();
    public Resource food = new Resource(ResourceType.Food, 100, 10);
    public Resource morale = new Resource(ResourceType.Morale, 50, 0);
    public Resource metal = new Resource(ResourceType.Steel, 50, 0);
    public Resource stone = new Resource(ResourceType.Boulders, 50, 0);
    public Resource wood = new Resource(ResourceType.Lumber, 50, 0);
    public Resource hammers = new Resource(ResourceType.Hammer, 0, 0);
    public Resource axes = new Resource(ResourceType.Axe, 0, 0);
    public Resource picks = new Resource(ResourceType.Pick, 0, 0);

    public int numberOfPeople;
    public int numberOfGuards;
    public Text numberOfPeopleText, numberOfGuardsText;

    public static CampResources instance;
    public static float lastMetalTaken, lastWoodTaken, lastStoneTaken;

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

    void Start()
    {
        CountPeople();
    }

    void Update()
    {
        foreach (Resource resource in Resources)
        {
            if (resource.resourceText == null)
                continue;

            resource.resourceText.text = resource.resourceType.ToString() + "\n" + resource.value.ToString();
        }

        numberOfPeopleText.text = "Workers\n" + numberOfPeople.ToString();
        numberOfGuardsText.text = "Guards\n" + numberOfGuards.ToString();
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
        //numberOfGuards = ;
        numberOfPeople = WorkerManager.workers.Count;
        numberOfGuards = Guard.guards.Count;
    }

    public bool TakeEverything(TrainScenario scenario)
    {
        if (metal.value < scenario.minimalMetalValue || wood.value < scenario.minimalWoodValue ||
            stone.value < scenario.minimalStoneValue)
        {
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
    Food, Morale, Steel, Lumber, Boulders,
    Hammer, Axe, Pick
}