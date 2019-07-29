using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTrigger : MonoBehaviour
{
    public Vector3 windDir;
    private float i = 0;
    private Collider lastCol;
    private Collider currentCol;

    private void Start()
    {
    }

    private void Update()
    {
        if(i < 0f)
        {
            windDir = Vector3.zero;
        }
        i -= Time.deltaTime * 2f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            i = 1f;
            windDir = other.transform.right;
        }
    }

}
