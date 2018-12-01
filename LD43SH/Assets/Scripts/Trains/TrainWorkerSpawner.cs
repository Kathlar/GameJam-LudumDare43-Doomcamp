using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrainWorkerSpawner : MonoBehaviour
{
    public List<Transform> workerVagons;
    public Worker workerPrefab;
        
    public void SpawnWorkers(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            Transform vagon = workerVagons[i % workerVagons.Count];
            Worker worker = Instantiate(workerPrefab, vagon.transform.position, Quaternion.identity);
            Vector3 destination = worker.transform.position;

            destination -= worker.transform.position.normalized * 10;
            destination.x += Random.Range(-5.0f, 5.0f);
            destination.z += Random.Range(-5.0f, 5.0f);

            worker.GetComponent<NavMeshAgent>().SetDestination(destination);
        }
    }
}
