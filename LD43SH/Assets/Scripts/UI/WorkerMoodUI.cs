using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerMoodUI : MonoBehaviour
{
    Worker worker;

    public GameObject slackPrompt;
    public GameObject runPrompt;

    private void Awake()
    {
        worker = GetComponent<Worker>();
    }

    private void Start()
    {
        InvokeRepeating("UpdateState", Random.Range(0.0f, 1.0f), 1.0f);
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    void UpdateState()
    {
        if (worker.currentActivity == null)
            return;

        if (worker.currentActivity.name == "Slacking")
        {
            slackPrompt.SetActive(true);
        }
        else if (worker.currentActivity.name == "Attempting escape")
        {
            runPrompt.SetActive(true);
        }
        else
        {
            slackPrompt.SetActive(false);
            runPrompt.SetActive(false);
        }
    }

    void Cleanup()
    {
        if (slackPrompt) Destroy(slackPrompt);
        if (runPrompt) Destroy(runPrompt);
    }
}
