using UnityEngine;

public class Vane : MonoBehaviour
{
    private bool _counted;
    private GameManager _gm;
    private Vector3 lastDir;
    private RotateCommand rc;
    private bool _swing;

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
        if (transform.forward == Vector3.forward && !_counted)
        {
            _counted = true;
            _gm.Decrease();
        }

        if (transform.forward != Vector3.forward && _counted)
        {
            _counted = false;
            _gm.Increase();
        }
        // TODO: Make it swing
        if (_swing)
        {
            Debug.Log("SWINGING~~~");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.forward);
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
                rc = new RotateCommand(transform, other.transform.forward);
                if (!_swing) _swing = true;
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