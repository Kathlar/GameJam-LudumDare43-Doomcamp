using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkplaceBigPanel : BigPanel
{
    void Awake()
    {
        miniPanel = FindObjectOfType<WorkplaceMiniPanel>();
    }
}
