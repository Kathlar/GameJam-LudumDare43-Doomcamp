using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerHP : MonoBehaviour
{
    public Renderer ren;
    Worker worker;
    
    private void Awake()
    {
        worker = GetComponent<Worker>();
    }

    private void Update()
    {
        if (ShouldDisplay())
        {
            ren.enabled = true;
            UpdateColor();
        }
        else
        {
            ren.enabled = false;
        }
    }

    private void OnDestroy()
    {
        ren.enabled = false;
    }

    bool ShouldDisplay()
    {
        Vector3 mouseForward = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        Vector3 dirToMe = transform.position - Camera.main.transform.position;

        return Vector3.Angle(mouseForward, dirToMe) < 10f;
    }

    void UpdateColor()
    {
        float hp = Mathf.Clamp(worker.health, 0, 1);
        ren.material.SetColor("_EmissionColor", new Color(1 - hp, hp, 0));
    }
}
