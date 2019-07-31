using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private float speed = 0.01f;
    private float angleOffset = 45;
    Vector3 lastMousePos;
    private float minSize = 1f;
    private float maxSize = 5.5f;
    private readonly float _overTime = .5f;
    private bool _running;
    private GameManager _gm;

    private void Start()
    {
        _gm = GameManager.Instance;
    }


    // Update is called once per frame
    void Update()
    {
        if (_gm._count == 0) return;
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
                offset = Quaternion.AngleAxis(angleOffset, Vector3.forward) * offset;
                transform.position += new Vector3(offset.x, 0, offset.y);
            }
        }
        if (Input.GetMouseButtonUp(2))
        {
            lastMousePos = Vector3.zero;
        }
        lastMousePos = Input.mousePosition;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_running) return;
            StartCoroutine(Rotate(45));
            // transform.Rotate(Vector3.up, 90, Space.World);
            angleOffset -= 45;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_running) return;
            StartCoroutine(Rotate(-45));
            // transform.Rotate(Vector3.up, -90, Space.World);
            angleOffset += 45;
        }
    }
    
    private IEnumerator Rotate(float degree)
    {
        _running = true;
        var rotation = Quaternion.Euler(0, degree, 0);
        var postRotation = transform.rotation * rotation;
        var startTime = Time.time;
        while (Time.time < startTime + _overTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, postRotation, 
                (Time.time - startTime) / _overTime);
            yield return null;
        }
        transform.rotation = postRotation;
        _running = false;
    }
}