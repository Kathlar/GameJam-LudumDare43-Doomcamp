using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    public static TrainManager Instance;

    protected CampResources resources;
    public List<TrainScenario> TrainScenarios;
    private int numberOfDaysToNextTrain;
    public Train train;

    void Awake()
    {
        Instance = this;
        resources = CampResources.instance;
        train = FindObjectOfType<Train>();
        train.manager = this;
    }

    void Start()
    {
        numberOfDaysToNextTrain = TrainScenarios[0].numberOfDaysToNextTrain;
    }

    public void TrainSpawn()
    {
        numberOfDaysToNextTrain--;
        if(numberOfDaysToNextTrain <= 0)
            train.SpawnTrain();
    }

    public void TrainArrive()
    {
        //train arrives on station
        TrainScenario scenario = TrainScenarios[0];
        resources.food.value += scenario.food;
        bool gotEnoughResources = resources.TakeEverything(scenario);
        FindObjectOfType<TrainInfo>().ShowTrainInfo(scenario);
        //spawn people
        if (TrainScenarios.Count > 1) TrainScenarios.RemoveAt(0);
        numberOfDaysToNextTrain = scenario.numberOfDaysToNextTrain;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 40, 150, 100), "Days to next train: " + numberOfDaysToNextTrain.ToString());
    }
}


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
