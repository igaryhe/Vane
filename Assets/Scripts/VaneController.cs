using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaneController : MonoBehaviour
{
    private Vector3 lastDir;
    private bool isStatic;
    private float deltaAngle;
    private Vector3 dir;
    private float rotateSpeed = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        deltaAngle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        if (deltaAngle > 5)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed);
        }
        else if (deltaAngle < -5)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * -rotateSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wind"))
        {
            if(lastDir != null)
            {
                if (lastDir == other.attachedRigidbody.velocity.normalized)
                {
                    transform.forward = -other.attachedRigidbody.velocity.normalized;
                }
                else if (lastDir != other.attachedRigidbody.velocity.normalized)
                {
                    transform.forward = -other.attachedRigidbody.velocity.normalized + lastDir;
                }
            }
            else
            {
                    transform.forward = -other.attachedRigidbody.velocity;
            }
            lastDir = other.attachedRigidbody.velocity.normalized;
        }
    }

    private void Rot(Vector3 dir)
    {
        //transform.forward = Vector3.Lerp()
    }
}
