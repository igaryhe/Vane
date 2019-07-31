using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public Vector3 windDir;
    private float i = 0;
    private float t = 0;
    private Material grass;
    private float lastStr = 0.1f;
    private float windStr = 0.1f;
    private Vector3 windDirection;
    private Vector3 lastDirection;

    private void Start()
    {
        grass = GetComponentInChildren<Renderer>().material;
    }

    private void Update()
    {
        if (t < 0f)
        {
            windDir = Vector3.zero;
        }
        t -= Time.deltaTime * 1f;
        if (i > 1)
        {
            i = 0;
            lastDirection = windDirection;
            lastStr = windStr;
            windDirection = Quaternion.Euler(0, 0, 90) * new Vector3(windDir.x, windDir.z, 0);
            windDirection *= 0.2f;
            windStr = 1;
        }
        else
        {
            grass.SetVector("_WindDir", Vector3.Lerp(lastDirection, windDirection, i));
            //grass.SetFloat("_WindSpd", Mathf.Lerp(0.1f, 1f, Vector3.Lerp(lastDirection.normalized, windDirection.normalized, i).magnitude));
            grass.SetFloat("_WindSpd", Mathf.Lerp(0.1f, 1f, windDirection.normalized.magnitude));
        }
        i += Time.deltaTime * 3;
    }

        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            t = 1f;
            windDir = other.transform.right;
        }
    }
}
