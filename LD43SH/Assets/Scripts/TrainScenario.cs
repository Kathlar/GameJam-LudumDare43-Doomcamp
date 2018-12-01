using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Train Scenario")]
public class TrainScenario : ScriptableObject
{
    public float food;
    public int numberOfPeople;
    public int numberOfDaysToNextTrain = 10;
    public float minimalWoodValue, minimalMetalValue, minimalStoneValue;
    [TextArea]
    public string comradeText;

}