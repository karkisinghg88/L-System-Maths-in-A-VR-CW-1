using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    public LSystem lSystem;  // assign LSystem object in inspector
    public float padding;

    void Start()
    {
        // Subscribe to event fired when L-system finishes generating
        lSystem.OnLSystemGenerated += FitCameraToTree;
    }

    void FitCameraToTree()
    {
        // FIX: Use FindObjectsByType with FindObjectsSortMode.None to avoid obsolete warning
        LineRenderer[] lines = Object.FindObjectsByType<LineRenderer>(FindObjectsSortMode.None);

        if (lines.Length == 0)
            return;

        Vector3 min = lines[0].GetPosition(0);
        Vector3 max = lines[0].GetPosition(0);

        // Compute bounds of ALL branches
        foreach (var lr in lines)
        {
            for (int i = 0; i < lr.positionCount; i++)
            {
                Vector3 p = lr.GetPosition(i);
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
        }

        Vector3 center = (min + max) / 2f;

        float height = max.y - min.y;
        float width = max.x - min.x;
        float size = Mathf.Max(height, width) * padding;

        Camera cam = GetComponent<Camera>();

        // orthographic cam (best for 2D tree)
        if (cam.orthographic)
        {
            cam.orthographicSize = size;
            transform.position = new Vector3(center.x, center.y, transform.position.z);
        }
        else
        {
            float distance = size / Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f);
            Vector3 target = center - transform.forward * distance;

            transform.position = new Vector3(target.x, target.y, target.z);
        }
    }
}



