using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodUI : MonoBehaviour
{
    float dayLength { get { return DayTimeManager.instance.dayLength; } }
    float lastFood;
    float curFood;

    public Text text1;
    public Text text2;
    public Text survivalTimeText;
    public Slider slider;

    private void Start()
    {
        InvokeRepeating("UpdateState", 0.1f, 1.0f);
    }

    void UpdateState()
    {
        SurvivalTimeUpdate();
        lastFood = curFood;
        curFood = CampResources.instance.food.value;

        float diff = lastFood - curFood;
        float changePerDay = diff * dayLength;

        if (diff < 0) return; // train added new food

        text1.text = ((int)curFood).ToString();
        text2.text = "-" + ((int)changePerDay) + "/day";
    }
    
    void SurvivalTimeUpdate()
    {        
        if (CampResources.instance.foodRationsRate > 0.9f)
        {
            survivalTimeText.text = "14+ days";
        }
        else
        {
            float survivedSeconds = 120 / (1 - CampResources.instance.foodRationsRate);
            float survivedDays = survivedSeconds / dayLength;
            survivedDays -= 0.5f; // just to be a bit pessimistic :3

            if (survivedDays > 14)
                survivalTimeText.text = "14+ days";
            else
                survivalTimeText.text = survivedDays.ToString("0.0") + " days";
        }
    }
}
