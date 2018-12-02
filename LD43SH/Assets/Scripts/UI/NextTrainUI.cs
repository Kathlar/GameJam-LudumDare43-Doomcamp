using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTrainUI : MonoBehaviour
{
    public Text woodTxt;
    public Text boulderTxt;
    public Text steelTxt;
    public Text workerTxt;

    public GameObject woodOkTag;
    public GameObject boulderOkTag;
    public GameObject steelOkTag;
    public GameObject workerOkTag;

    private void Update()
    {
        TrainScenario info = TrainManager.Instance.TrainScenarios[0];
        float available, needed;
        
        // wood
        if (info.minimalWoodValue == 0)
        {
            woodTxt.gameObject.SetActive(false);
        }
        else
        {
            woodTxt.gameObject.SetActive(true);
            available = CampResources.instance.wood.value;
            needed = info.minimalWoodValue;
        }

        // stone
        if (info.minimalStoneValue == 0)
        {
            boulderTxt.gameObject.SetActive(false);
        }
        else
        {
            boulderTxt.gameObject.SetActive(true);
            available = CampResources.instance.stone.value;
            needed = info.minimalStoneValue;
        }

        //steel
        if (info.minimalMetalValue == 0)
        {
            steelTxt.gameObject.SetActive(false);
        }
        else
        {
            steelTxt.gameObject.SetActive(true);
            available = CampResources.instance.metal.value;
            needed = info.minimalMetalValue;
        }

        //workers
        if (info.numberOfPeople >= 0)
        {
            workerTxt.gameObject.SetActive(false);
        }
        else
        {
            workerTxt.gameObject.SetActive(true);
            available = WorkerManager.workers.Count;
            needed = -info.numberOfPeople;
        }
    }
}
