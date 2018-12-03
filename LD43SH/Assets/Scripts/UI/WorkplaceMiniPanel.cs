using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkplaceMiniPanel : MiniPanel
{
    public Workplace workplace;
    public Text text;
    public GameObject needMoreToolsMenu;

    protected override void Start()
    {
        if(bigPanel == null)
            bigPanel = FindObjectOfType<WorkplaceBigPanel>();

        if (workplace == null) workplace = FindObjectOfType<Workplace>();
        objectPanel = workplace.transform;
        base.Start();
    }

    protected override void Update()
    {
        maxValue = workplace.maxWorkersCount;
        base.Update();
        float greenValue = workplace.workers.Count - workplace.workersWithoutTools;
        float yellowValue = workplace.workersWithoutTools;
        float desiredValue = workplace.desiredWorkersCount;

        greenImage.fillAmount = greenValue / maxValue;
        yellowImage.fillAmount = yellowValue / maxValue;
        slider.value = desiredValue / maxValue;
        currentValueText.text = Mathf.Floor(slider.value * maxValue).ToString();

        text.text =
            "Working: " + workplace.workers.Count;
        if (workplace.workersWithoutTools > 0)
            text.text += "\n(" + workplace.workersWithoutTools + " with no tools)";

        if (workplace.workersWithoutTools * 2 >= workplace.workers.Count && workplace.workers.Count > 0)
            needMoreToolsMenu.SetActive(true);
        else
            needMoreToolsMenu.SetActive(false);
    }

    public void SetWorkers()
    {
        workplace.desiredWorkersCount = (int)Mathf.Floor(slider.value * maxValue);
    }
}
