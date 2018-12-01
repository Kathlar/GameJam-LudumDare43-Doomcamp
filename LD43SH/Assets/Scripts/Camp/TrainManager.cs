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
        numberOfDaysToNextTrain = 1;
        TrainSpawn();
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

        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value + 30.0f, 0, 100);

        string failComment = "";
        bool gotEnoughResources = resources.TakeEverything(scenario, out failComment);
        if (scenario.numberOfPeople < 0)
        {
            if (WorkerManager.workers.Count < scenario.numberOfPeople)
            {
                gotEnoughResources = false;
                failComment = "oh, everyone looks pretty dead, but wee need at least " + scenario.numberOfPeople + " new workers... Hey, at least we can take you!";
            }
            else
            {
                for (int i = WorkerManager.workers.Count - 1; i >= 0; --i)
                {
                    Worker w = WorkerManager.workers[i];
                    w.DieSilent();
                }
            }
        }

        FindObjectOfType<TrainInfo>().ShowTrainInfo(scenario, gotEnoughResources);
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
