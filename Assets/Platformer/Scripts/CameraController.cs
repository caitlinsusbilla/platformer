using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followObject;
    public Vector3 followOffset;
    private Vector3 threshold;

    void Start()
    {
        threshold = calculateThreshold();
    }

    void FixedUpdate()
    {
        Vector3 followPosition = followObject.transform.position;
        Vector3 cameraPosition = transform.position;

        float xDifference = Mathf.Abs(followPosition.x - cameraPosition.x);
        float yDifference = Mathf.Abs(followPosition.y - cameraPosition.y);
        float zDifference = Mathf.Abs(followPosition.z - cameraPosition.z);

        Vector3 newPosition = cameraPosition;
        if (xDifference >= threshold.x)
        {
            newPosition.x = followPosition.x;
        }
        if (yDifference >= threshold.y)
        {
            newPosition.y = followPosition.y;
        }
        if (zDifference >= threshold.z)
        {
            newPosition.z = followPosition.z;
        }
        transform.position = newPosition + followOffset;
    }

    private Vector3 calculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        float zThreshold = Mathf.Abs(transform.position.z - followObject.transform.position.z);
        return new Vector3(t.x, t.y, zThreshold);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, border * 2);
    }
}
