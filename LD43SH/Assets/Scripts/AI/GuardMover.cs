using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMover : MonoBehaviour
{
    public LayerMask guardMask;
    public LayerMask groundMask;

    Guard heldGuard;
    LineRenderer lr;
    Vector3 offset = Vector3.up * 0.1f;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MouseDown();

        if (Input.GetMouseButtonUp(0))
            MouseUp();

        if (heldGuard)
        {
            Vector3[] positions = new Vector3[]
            {
                heldGuard.transform.position + offset,
                GetMouseWorldPos() + offset
            };
            lr.positionCount = 2;
            lr.SetPositions(positions);
        }
        else
        {
            lr.positionCount = 0;
            lr.SetPositions(new Vector3[0]);
        }
    }

    private void MouseDown()
    {
        heldGuard = TryGetGuard();
    }

    private void MouseUp()
    {
        if (heldGuard)
            heldGuard.GoToSpot(GetMouseWorldPos());
        heldGuard = null;
    }

    Guard TryGetGuard()
    {
        RaycastHit rh;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rh, 1000, guardMask, QueryTriggerInteraction.Collide))
            return null;

        return rh.collider.transform.parent.GetComponent<Guard>();
    }

    Vector3 GetMouseWorldPos()
    {
        RaycastHit rh;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rh, 1000, groundMask))
            return Vector3.zero;

        return rh.point;
    }
}
