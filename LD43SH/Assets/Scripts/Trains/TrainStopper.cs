using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainStopper : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        Train train = col.GetComponent<Train>();
        if (train != null)
        {
            train.StopTrain();
            Debug.Log("Train Stopped");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
