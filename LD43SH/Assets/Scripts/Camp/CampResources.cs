using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampResources : MonoBehaviour
{
    private List<Resource> Resources = new List<Resource>();
    public Resource food = new Resource(ResourceType.Food, 100, 10);
    public Resource morale = new Resource(ResourceType.Morale, 50, 0);
    public Resource metal = new Resource(ResourceType.Metal, 50, 0);
    public Resource stone = new Resource(ResourceType.Stone, 50, 0);
    public Resource wood = new Resource(ResourceType.Wood, 50, 0);

    public int numberOfPeople;
    public int numberOfGuards;
    public Text numberOfPeopleText, numberOfGuardsText;

    void Start()
    {
        Resources.Add(food);
        Resources.Add(morale);
        Resources.Add(metal);
        Resources.Add(stone);
        Resources.Add(wood);
        CountPeople();
    }

    void Update()
    {
        foreach (Resource resource in Resources)
        {
            resource.resourceText.text = resource.resourceType.ToString() + "\n" + resource.value.ToString();
        }

        numberOfPeopleText.text = "Workers\n" + numberOfPeople.ToString();
        numberOfGuardsText.text = "Guards\n" + numberOfGuards.ToString();
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
    Food, Morale, Metal, Wood, Stone
}