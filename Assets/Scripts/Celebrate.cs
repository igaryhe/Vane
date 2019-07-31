using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebrate : MonoBehaviour
{
    private float i;
    void Start()
    {
        i = -Random.Range(0,500);
    }

    // Update is called once per frame
    void Update()
    {
        i += Time.deltaTime * 500f;
        if(i > 0)
        {
            transform.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(0, Mathf.PingPong(i,100));
        }
    }

    private void OnDisable()
    {
        transform.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
    }
}
