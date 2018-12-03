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

    public Slider sliderWood;
    public Slider sliderBoulder;
    public Slider sliderSteel;
    public Slider sliderWorker;
    public Slider sliderTime;

    public GameObject woodOkTag;
    public GameObject boulderOkTag;
    public GameObject steelOkTag;
    public GameObject workerOkTag;
    public GameObject woodFailTag;
    public GameObject boulderFailTag;
    public GameObject steelFailTag;
    public GameObject workerFailTag;

    private void Start()
    {
        InvokeRepeating("UpdateState", 0, 1);
    }

    private void UpdateState()
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

            available = Mathf.Clamp(available, 0, needed);
            woodOkTag.SetActive(available == needed);
            woodFailTag.SetActive(available != needed);
            woodTxt.text = "Lumber: " + (int)available + " / " + (int)needed;
            sliderWood.value = available / needed;
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

            available = Mathf.Clamp(available, 0, needed);
            boulderOkTag.SetActive(available == needed);
            boulderFailTag.SetActive(available != needed);
            boulderTxt.text = "Boulders: " + (int)available + " / " + (int)needed;
            sliderBoulder.value = available / needed;
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

            available = Mathf.Clamp(available, 0, needed);
            steelOkTag.SetActive(available == needed);
            steelFailTag.SetActive(available != needed);
            steelTxt.text = "Steel: " + (int)available + " / " + (int)needed;
            sliderSteel.value = available / needed;
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

            available = Mathf.Clamp(available, 0, needed);
            workerOkTag.SetActive(available == needed);
            workerFailTag.SetActive(available != needed);
            workerTxt.text = "Workers: " + (int)available + " / " + (int)needed;
            sliderWood.value = available / needed;
        }
    }
}
