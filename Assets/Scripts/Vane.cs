using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Vane : MonoBehaviour
{
    private bool _counted;
    private GameManager _gm;
    private Vector3 lastDir;
    public RotateCommand rc;
    public bool _swing;
    public Quaternion first, second;
    private bool clockWise;
    private float t;
    private float i = 0;

    private void Start()
    {
        _gm = GameManager.Instance;
        if (transform.forward == Vector3.forward)
        {
            _counted = true;
            _gm.Decrease();
        }
        rc = new RotateCommand(transform, Vector3.zero);
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
        
        if (_swing)
        {
            if (transform.rotation == first)
            {
                clockWise = true;
                t = 0;
            }
            else if (transform.rotation == second)
            {
                clockWise = false;
                t = 0;
            }

            t += Time.deltaTime;
            if (clockWise)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, second, t);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, first, t);
            }
            if(i < 0f && rc.direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(rc.direction, Vector3.up);
                _swing = false;
            }
            i -= Time.deltaTime * 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            if (rc.direction == Vector3.zero)
            {
                // rc = new RotateCommand(transform, other.transform.forward);
                rc.direction = other.transform.forward;
                rc.Execute();
            }
            else if (other.transform.forward == rc.direction)
            {
                if (_swing)
                {
                    _swing = false;
                    rc = new RotateCommand(transform, other.transform.forward);
                    // rc.direction = other.transform.forward;
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
                    first = Quaternion.LookRotation(rc.direction, Vector3.up);
                    second = Quaternion.LookRotation(other.transform.forward, Vector3.up);
                    // rc = new RotateCommand(transform, other.transform.forward);
                    rc.direction = other.transform.forward;
                    // rc.Execute();
                    _swing = true;
                }
            }
            // ci.command.affected.Push(rc);
        }
    }

    public void ResetDirection()
    {
        _swing = false;
        if (rc.running) rc.Stop();
        rc.direction = Vector3.zero;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            i = 2f;
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