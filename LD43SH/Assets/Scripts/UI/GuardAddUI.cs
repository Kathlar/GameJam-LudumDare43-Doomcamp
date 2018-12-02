using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardAddUI : MonoBehaviour
{
    public Button startAddBtn;
    public Button noBtn;
    public Button yesBtn;

    public GameObject addPanel;
    public Guard guardPrefab;

    private void Awake()
    {
        startAddBtn.onClick.AddListener(AddClicked);
        noBtn.onClick.AddListener(NoClicked);
        yesBtn.onClick.AddListener(YesClicked);
    }

    void AddClicked()
    {
        addPanel.SetActive(true);
    }

    void NoClicked()
    {
        addPanel.SetActive(false);
    }

    void YesClicked()
    {
        addPanel.SetActive(false);

        Worker workerToConvert = WorkerManager.workers[0];
        Vector3 workerPos = workerToConvert.transform.position;
        Quaternion workerRot = workerToConvert.transform.rotation;
        // turn this into coroutine and lerp cam onto worker

        workerToConvert.DieSilent();
        Instantiate(guardPrefab, workerPos, workerRot);

        // spawn a particle
    }
}
