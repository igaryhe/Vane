using System;
using UnityEngine;

public class Fan : MonoBehaviour, CommandInterface
{
    public GameObject windParticle;
    private float i = 0f;
    public Command command { get; set; }
    /*
    public GameObject ball;
    private const float speed = 2f;
    */
    
    /*
    private void Start()
    {
        var trans = transform.position;
        trans.y += 0.5f;
        var ins = Instantiate(ball, trans, Quaternion.LookRotation(transform.forward, Vector3.up));
        ins.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        ins.GetComponent<Ball>().ci = this;
    }
    */

    private void Update()
    {
        GenerateWind(0.7f);
    }

    private void GenerateWind(float interval)
    {
        if (i > interval)
        {
            i = 0;
            var instance = Instantiate(windParticle, transform.position + new Vector3(0, 0.5f, 0),
                transform.rotation);
            // instance.GetComponent<Wind>().ci = this;
        }
        else
        {
            i += Time.deltaTime;
        }
    }
}