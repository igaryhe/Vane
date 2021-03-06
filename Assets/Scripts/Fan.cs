﻿using System;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public GameObject windParticle;
    private float i;
    private GameManager _gm;
    public Command command { get; set; }
    private void Start()
    {
        _gm = GameManager.Instance;
    }

    private void Update()
    {
        GenerateWind(1);
    }

    private void GenerateWind(float interval)
    {
        if (i > interval)
        {
            i = 0;
            var instance = Instantiate(windParticle, transform.position + new Vector3(0, 0.5f, 0) - transform.forward * 5,
                transform.rotation);
            instance.transform.parent = _gm.winds.transform;
            // instance.GetComponent<Wind>().ci = this;
        }
        else
        {
            i += Time.deltaTime;
        }
    }
}