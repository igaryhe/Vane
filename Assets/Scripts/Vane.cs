using System.Collections;
using UnityEngine;

public class Vane : MonoBehaviour
{
    private bool _counted;
    private GameManager _gm;
    private Vector3 lastDir;

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
