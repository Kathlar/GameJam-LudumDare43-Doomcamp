using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampResources : MonoBehaviour
{
    public Resource food = new Resource(ResourceType.Food, 100, 10);
    public Resource morale = new Resource(ResourceType.Morale, 50, 0);
    public Resource metal = new Resource(ResourceType.Metal, 50, 0);
    public Resource stone = new Resource(ResourceType.Stone, 50, 0);
    public Resource wood = new Resource(ResourceType.Wood, 50, 0);

    public int numberOfPeople;
    public int numberOfGuards;

    void Start()
    {
        CountPeople();
    }

    public void DailyUseOfResources()
    {
        food.DailyUsage();
        morale.DailyUsage();
        metal.DailyUsage();
        stone.DailyUsage();
        wood.DailyUsage();
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