using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkplaceTester : MonoBehaviour
{
    private void Start()
    {
        Invoke("Test", 1.0f);
    }

    void Test()
    {
        foreach (Worker worker in WorkerManager.workers)
            worker.SetWorkplace(GetComponent<Workplace>());
    }
}