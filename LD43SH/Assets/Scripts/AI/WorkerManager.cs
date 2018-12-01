using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static List<Worker> workers;

    private void Awake()
    {
        workers = new List<Worker>();
    }


}
