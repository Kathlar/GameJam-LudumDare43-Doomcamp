using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    protected CampResources resources;
    public List<TrainScenario> TrainScenarios;
    public Train train;

    void Awake()
    {
        resources = GetComponent<CampResources>();
        train.manager = this;
    }

    public void TrainSpawn()
    {
        //create train object
    }

    public void TrainArrive()
    {
        //train arrives on station
        TrainScenario scenario = TrainScenarios[0];
        resources.food.value += scenario.food;
        //spawn people
        if (TrainScenarios.Count > 1) TrainScenarios.RemoveAt(0);
    }
}


[CreateAssetMenu(menuName = "Train Scenario")]
public class TrainScenario : ScriptableObject
{
    public float food;
    public int numberOfPeople;
}
