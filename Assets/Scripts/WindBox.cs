using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBox : MonoBehaviour
{
    private Vector3 windDir, lastDir;
    private float i, t;
    private Material windBox;
    // Start is called before the first frame update
    void Start()
    {
        windBox = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (i < 0f && i > -0.5f)//do if there no wind for 1 second
        {
            windDir = Vector3.zero;
        }
        i -= Time.deltaTime;
        if (t < 1)
        {
            windBox.SetVector("_WindDir", Vector3.Lerp(lastDir, windDir, t));
            windBox.SetFloat("_WindSpd", Mathf.Lerp(0.1f, 1f, windDir.normalized.magnitude));
        }
        else
        {
            t = 0;
            lastDir = windDir;
        }
        t += Time.deltaTime * 3f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            i = 1.5f;
            windDir = Quaternion.Euler(0, 0, 90) * new Vector3(other.transform.right.x, other.transform.right.z, 0) * 0.2f;
        }
    }
}
