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
    public Light globalLight;
    private float currentDayLength = 0;

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

    void Update()
    {
        float moraleLoss = 7 * Time.deltaTime / dayLength;
        campResources.morale.value = Mathf.Clamp(
            campResources.morale.value - moraleLoss,
            0, 100);

        currentDayLength += Time.deltaTime;

        float dayLightValue = 2 * (currentDayLength / dayLength);
        if (dayLightValue > 1) dayLightValue = Mathf.Abs(2 - dayLightValue);
        globalLight.intensity = Mathf.Clamp(dayLightValue, 0, 1);
    }

    public void ChangeDay()
    {
        currentDayLength = 0;
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