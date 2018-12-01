using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public Transform spawnPoint;

    public void SpawnTrain()
    {
        transform.position = spawnPoint.position;
    }
}
