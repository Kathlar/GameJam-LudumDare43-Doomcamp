using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkplaceBigPanel : BigPanel
{
    public Workplace workplace;

    void Awake()
    {
        miniPanel = FindObjectOfType<WorkplaceMiniPanel>();
        if (workplace == null) workplace = FindObjectOfType<Workplace>();

        maxValue = workplace.maxWorkersCount;
        transform.name = "Workplace (" + workplace.resourceType.ToString() + ")";
    }

    protected override void Update()
    {
        base.Update();
        float greenValue = workplace.workers.Count - workplace.workersWithoutTools;
        float yellowValue = workplace.workersWithoutTools;
        float desiredValue = workplace.desiredWorkersCount;

        greenImage.fillAmount = greenValue / maxValue;
        yellowImage.fillAmount = yellowValue / maxValue;
        slider.value = desiredValue / maxValue;
        currentValueText.text = Mathf.Floor(slider.value * maxValue).ToString();
    }

    public void SetWorkers()
    {
        workplace.desiredWorkersCount = (int)Mathf.Floor(slider.value * maxValue);
    }
}
