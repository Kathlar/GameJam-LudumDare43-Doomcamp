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
        train = FindObjectOfType<Train>();
        train.manager = this;
    }

    void Start()
    {
        resources = CampResources.instance;
        numberOfDaysToNextTrain = 1;
        TrainSpawn();
    }

    public void TrainSpawn()
    {
        numberOfDaysToNextTrain--;
        if (numberOfDaysToNextTrain <= 0)
        {
            Camera.main.transform.parent.parent.GetComponent<CameraController>()
                .StartCamLerp(new Vector3(-30, 0, 16.5f));
            train.Arrive();
        }
    }

    public void TrainArrive()
    {
        //train arrives on station
        TrainScenario scenario = TrainScenarios[0];
        resources.food.value += scenario.food;

        CampResources.instance.morale.value = 100;

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
                int peopleTaken = 0;
                for (int i = WorkerManager.workers.Count - 1; i >= 0; --i)
                {
                    Worker w = WorkerManager.workers[i];
                    w.DieSilent();
                    peopleTaken++;
                    if (peopleTaken >= -scenario.numberOfPeople)
                        break;
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
