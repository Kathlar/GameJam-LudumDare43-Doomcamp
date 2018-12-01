using UnityEngine;

public class EscapingManager : MonoBehaviour
{
    public static Vector3 GetEscapePoint(Worker worker)
    {
        Vector2 point = Random.insideUnitCircle.normalized;
        point.x += worker.transform.position.x / 25;
        point.y += worker.transform.position.z / 25;
        point.Normalize();

        return new Vector3(point.x, 0, point.y) * 100.0f;
    }
}
