using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimeManager : MonoBehaviour
{
    protected CampResources campResources;
    protected TrainManager trainManager;

    public int dayNumber = 1;
    public float dayLength = 120;

    public static DayTimeManager instance;
    public Action OnDayEnd;

    void Awake()
    {
        instance = this;
        campResources = GetComponent<CampResources>();
    }

    void Start()
    {
        InvokeRepeating("ChangeDay", dayLength, dayLength);
    }

    public void ChangeDay()
    {
        dayNumber++;
        campResources.DailyUseOfResources();
        if (OnDayEnd != null) OnDayEnd();
        TrainManager.Instance.TrainSpawn();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 100), "DAY: " + dayNumber.ToString());
    }
}