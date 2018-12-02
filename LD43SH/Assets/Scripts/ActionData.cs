using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionData : MonoBehaviour
{
    public string animName;
    public float time = 5.0f;
    public Transform place;

    private void Awake()
    {
        place = transform;
    }
}