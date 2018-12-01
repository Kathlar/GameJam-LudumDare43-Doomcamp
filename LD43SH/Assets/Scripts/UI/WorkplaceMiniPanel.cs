using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkplaceMiniPanel : MiniPanel
{
    public Workplace workplace;
    private int maxValue = 10;

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
        base.Update();
        float greenValue = workplace.workers.Count - workplace.workersWithoutTools;
        float yellowValue = workplace.workersWithoutTools;
        float desiredValue = workplace.desiredWorkersCount;

        greenImage.fillAmount = greenValue / maxValue;
        yellowImage.fillAmount = yellowValue / maxValue;
        //slider.value = desiredValue / maxValue;
        currentValueText.text = Mathf.Floor(slider.value * maxValue).ToString();
    }

    public void SetWorkers()
    {
        workplace.desiredWorkersCount = (int)Mathf.Floor(slider.value * maxValue);
    }
}
