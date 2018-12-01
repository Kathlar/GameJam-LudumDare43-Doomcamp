using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour
{
    protected float defaultPositionY;

	private void Start()
    {
        defaultPositionY = transform.position.y;
    }
	
	private void Update()
    {
        Vector3 position = transform.position;

        position.y = defaultPositionY;

        transform.position = position;
    }
}
