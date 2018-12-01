using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimeManager : MonoBehaviour
{
    protected CampResources campResources;

    public int dayNumber = 1;
    public float dayLength = 120;

    void Awake()
    {
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
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 100), "DAY: " + dayNumber.ToString());
    }
}