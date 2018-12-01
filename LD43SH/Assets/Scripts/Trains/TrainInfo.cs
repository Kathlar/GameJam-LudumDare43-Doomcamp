using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainInfo : MonoBehaviour
{
    private bool showing;
    public GameObject parentObj;
    public Text takenText, givenText;
    public Text infoText;

    void Start()
    {
        parentObj.SetActive(false);
    }

    void Update()
    {
        if (showing)
        {
            if (Input.GetKeyDown(KeyCode.Space)) HideTrainInfo();
        }
    }

    public void ShowTrainInfo(TrainScenario scenario)
    {
        showing = true;
        Time.timeScale = 0;
        parentObj.SetActive(true);

        takenText.text = "TAKEN:\nSteel: " + CampResources.lastMetalTaken + "\nLumber: " + CampResources.lastWoodTaken +
                         "\nBoulders:" + CampResources.lastStoneTaken;
        givenText.text = "GIVEN:\nPeople: " + scenario.numberOfPeople + "\nFood: " + scenario.food;

        infoText.text = scenario.comradeText;
    }

    public void HideTrainInfo()
    {
        showing = false;
        Time.timeScale = 1;
        parentObj.SetActive(false);
        FindObjectOfType<Train>().StartTrain();
    }
}
