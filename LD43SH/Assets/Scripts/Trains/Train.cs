using System.Collections;
using System.Collections.Generic;
using Battlehub.SplineEditor;
using UnityEngine;

public class Train : MonoBehaviour
{
    [HideInInspector]
    public TrainManager manager;
    private TrainStopper stopper;
    private TrainWorkerSpawner workerSpawner;
    float lastTrainTime = 0;
    bool isArriving = false;
    bool isLeaving = false;
    float spawnDistance = 200.0F;

    void Awake()
    {
        stopper = FindObjectOfType<TrainStopper>();
        workerSpawner = gameObject.GetComponent<TrainWorkerSpawner>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, stopper.transform.position);
        
        if (isArriving && distance < 2.0F)
        {
            StopTrain();
        }
        else if (isLeaving && distance > spawnDistance)
        {
            DespawnTrain();
        }
        
        if (isArriving || isLeaving)
        {
            Vector3 position = transform.position;

            float speed = Mathf.Clamp(distance, 0.0F, 30.0F);

            position.x += speed * Time.deltaTime;

            transform.position = position;
        }
    }

    private void SpawnTrain()
    {
        // gameObject.SetActive(true);
    }

    private void DespawnTrain()
    {
        // gameObject.SetActive(false);
    }

    public void Arrive()
    {
        SpawnTrain();
        isArriving = true;
        GetComponent<AudioSource>().Play();

        Vector3 position = transform.position;

        position.x = stopper.transform.position.x - spawnDistance;

        transform.position = position;
    }

    private void Leave()
    {
        isLeaving = true;
        GetComponent<AudioSource>().Play();
    }

    private void StopTrain()
    {
        isArriving = false;

        workerSpawner.SpawnWorkers(manager.TrainScenarios[0].numberOfPeople);
        manager.TrainArrive();
        Invoke("Leave", 5);
    }
}
