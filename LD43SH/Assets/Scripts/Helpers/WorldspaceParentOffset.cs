using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldspaceParentOffset : MonoBehaviour
{
    public Vector3 offset = Vector3.up;

    private void Update()
    {
        transform.position = transform.parent.position + offset;   
    }
}
