using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodUI : MonoBehaviour
{
    float dayLength { get { return DayTimeManager.instance.dayLength; } }
    float lastFood;
    float curFood;

    public Text text;

    private void Start()
    {
        InvokeRepeating("UpdateState", 0.1f, 1.0f);
    }

    void UpdateState()
    {
        lastFood = curFood;
        curFood = CampResources.instance.food.value;

        float diff = lastFood - curFood;
        float changePerDay = diff * dayLength;

        if (diff < 0) return; // train added new food

        text.text = ((int)curFood) + " (-" + ((int)changePerDay) + ")";
    }
}
