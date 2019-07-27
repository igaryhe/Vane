using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Vane : MonoBehaviour
{
    private bool _counted;
    private GameManager _gm;
    private Vector3 lastDir;
    private RotateCommand rc;
    public bool _swing;
    public Quaternion low, high;
    private bool clockWise;
    private float t;

    private void Start()
    {
        _gm = GameManager.Instance;
        if (transform.forward == Vector3.forward)
        {
            _counted = true;
            _gm.Decrease();
        }
    }

    private void Update()
    {
        if (transform.forward == _gm.win && !_counted && !_swing)
        {
            _counted = true;
            _gm.Decrease();
        }

        if (transform.forward != _gm.win && _counted)
        {
            _counted = false;
            _gm.Increase();
        }
        // TODO: Make it swing
        if (_swing)
        {
            if (transform.rotation == low || transform.rotation == high)
            {
                clockWise = !clockWise;
                t = 0;
            }

            t += Time.deltaTime;
            if (clockWise)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, high, t);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, low, t);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            if (rc == null)
            {
                rc = new RotateCommand(transform, other.transform.forward);
                rc.Execute();
            }
            else if (other.transform.forward == rc.direction)
            {
                if (_swing)
                {
                    _swing = false;
                    rc = new RotateCommand(transform, other.transform.forward);
                    rc.Execute();
                }
            }
            else
            {
                if (_swing)
                {
                    rc = new RotateCommand(transform, other.transform.forward);
                }
                else
                {
                    // rc.Stop();
                    low = Quaternion.LookRotation(rc.direction, Vector3.up);
                    high = Quaternion.LookRotation(other.transform.forward, Vector3.up);
                    rc = new RotateCommand(transform, other.transform.forward);
                    // rc.Execute();
                    _swing = true;
                }
            }
            // ci.command.affected.Push(rc);
        }
    }

    /*
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
    */
}