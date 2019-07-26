using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float minSize = 2f;
    float maxSize = 7.5f;
    float speed = 0.01f;
    Vector3 lastMousePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < maxSize)
        {
            Camera.main.orthographicSize += 0.5f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > minSize)
        {
            Camera.main.orthographicSize -= 0.5f;
        }
        if (Input.GetMouseButton(2))
        {
            if(lastMousePos != Vector3.zero)
            {
                Vector3 offset = (lastMousePos - Input.mousePosition) * speed;
                Debug.Log(offset);
                offset = Quaternion.AngleAxis(45, Vector3.forward) * offset;
                Debug.Log(offset);
                transform.position += new Vector3(offset.x, 0, offset.y);
            }
        }
        if (Input.GetMouseButtonUp(2))
        {
            lastMousePos = Vector3.zero;
        }
        lastMousePos = Input.mousePosition;
    }
}
